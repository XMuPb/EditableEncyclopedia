# Changelog

All notable changes to Editable Encyclopedia are documented here.

---

## v2.5.1 — Threading, Diagnostics & Custom-Culture Hardening (2026-05-09)

A correctness, stability, and UX release. Multiple cross-cutting bug patterns were identified across all 34 source files via multi-round code review and fixed in-place: a thread-visibility race on deferred work payloads, a `System.Threading.Timer` reassignment leak, a permanent encyclopedia input-lock when popup setup throws, silent loss of stack traces in catch-block diagnostics, and six round-4 critical findings (malformed banner-code crashes, custom culture data corruption on save reload, ID collisions on rapid culture creation, per-frame allocation hotspots, settlement-nameplate VM mutation off the main thread, redundant project-wide assembly scans). Two custom-culture flow bugs caught in follow-up testing — a CTD on culture change and the recruit roster silently reverting after a hire — are also fixed. Plus a Harmony-based MCMv5 patch that properly hides the EEWebExtension subgroup when the extension isn't installed and a log-spam cleanup in the lore-section renderer. No new features, no save format changes, no migration required.

### Threading & Crash Fixes

- **Settlement nameplate refresh no longer risks CTDs.** `SettlementNameplateMixin.ScheduleDeferredRefresh` previously called `vm.RefreshValues()` and walked the Gauntlet widget tree directly inside a `System.Threading.Timer` callback — Bannerlord's UI is single-threaded, so this could crash the game. The timer callback now only sets a `volatile bool` flag; the actual VM/widget refresh runs on the main thread via a new `TickMainThread()` method called from `OnApplicationTick`. Same fix also restores the broken retry path: the previous code assigned a no-op `_ => { }` lambda for retries 2 and 3, silently dropping them.
- **Deferred-work payload race fixed in 11 files.** `JournalSectionInjector`, `LoreSectionInjector`, `TagWidgetInjector`, `GlobalChroniclePanel`, `HeroTimelineSectionInjector`, `RelationNotesSectionInjector`, `DescriptionFormatter`, `SettlementNameplateMixin`, `EditPopupInjector`, `LoreStoryLoader`, `HeroDescriptionScrollPatch`, and `Localization` all had a `volatile bool _xxxPending` flag paired with non-volatile string/object/Timer payload fields. The JIT/CPU was free to observe the flag flipped while the consumer thread still saw stale payload values, producing wrong-entity injection (lore from previous hero appearing on next hero) and unreproducible NREs. All paired payload fields are now `volatile`.
- **Timer handle leaks fixed in 5 files.** `_retryTimer = new System.Threading.Timer(...)` was reassigned across 8 sites without disposing the previous instance. Rapid page navigation could fire stale timers out of order and double-fire `_retryPending`. Each site now calls `_retryTimer?.Dispose()` (or the existing `DisposeRetryTimer()` helper) before reassignment.
- **CTD on custom-culture change.** Picking a custom culture for a hero could crash with `Collection was modified; enumeration operation may not execute` inside `GauntletGamepadNavigationManager.Update`. Root cause: the popup-confirm click handler ran inside `OnApplicationTick`'s input-processing phase, then synchronously called `EncyclopediaPageTracker.CurrentRefreshAction()` to re-render the page. The Refresh added/removed widgets while the gamepad-navigation dictionary was mid-iteration on the same thread — classic single-thread reentrance race. Three culture-apply sites (`ApplyCustomCultureWithTroops`, the built-in `ApplyCulture`, and the reset-to-original path) now defer their `refresh()` and confirmation `DisplayMessage` calls to the next tick via a new `EncyclopediaPatchHelper.SchedulePostApplyRefresh(...)` queue, drained from `OnApplicationTick`.

### Custom Culture — Recruit Roster & Volunteer Persistence

- **Notable recruit roster no longer reverts to default culture after pick.** Setting a notable's culture to a custom culture correctly wrote `Hero._culture` and `CharacterObject._culture` via reflection, but the volunteer-troop slots displayed in the settlement recruit panel kept showing default-culture troops. Root cause: `RefreshNotableVolunteers` mutated the `volArray` (`Hero.VolunteerTypes`) in-place but never wrote the array reference back via reflection. Bannerlord's `VolunteerTypes` property getter returns a defensive copy, so in-place mutation didn't reach the underlying storage. Now the method also probes for the backing field (`_volunteerTypes` / `<VolunteerTypes>k__BackingField`) and calls `writableField.SetValue(hero, volArray)` after the slot-fill loop, so the new culture's troops actually populate the recruit panel.
- **Volunteer slots stay on the chosen culture across hires.** Even with the write-back fix, slots silently reverted to default-culture troops once the player hired the existing volunteers and the game's replenishment logic refilled empty slots — the replenishment uses the notable's home-settlement culture rather than the notable's own `Hero.Culture`, so our reflection write was bypassed. `EncyclopediaEditBehavior` now subscribes to `CampaignEvents.HourlyTickEvent` and re-runs `RefreshNotableVolunteers` for every notable that has a custom-culture assignment. Look for `OnHourlyTickReapplyCustomCultures: re-applied volunteer override for N notable(s)` in `debug.log` to confirm it's running. Worst-case visible-default window: ~1 in-game hour.
- **Custom culture troop trees no longer silently overwritten on save reload.** `GetOrCreateCulture` previously ran `CloneAllCultureFields` + `SetTroopField` unconditionally on the cache-miss path — including when the `CultureObject` was already in `MBObjectManager` from a Phase-1 save reload. Any subsequent UI path that re-called `GetOrCreateCulture` with default troop IDs silently clobbered the persisted assignments. Initialization now runs only when the `CultureObject` is freshly created.
- **Custom culture IDs are now collision-proof.** `GenerateCultureId` previously appended `DateTimeOffset.UtcNow.ToUnixTimeSeconds()` — back-to-back culture creation within the same wall-clock second (UI double-click, batch import) produced duplicate IDs and the second culture silently received the first culture's stale cached object with the wrong name and troops. Now uses `ToUnixTimeMilliseconds()` + 8-char `Guid` suffix.

### Encyclopedia No Longer Locks Up

- **`EncyclopediaInputBlocker.Block()` paired with `try/catch + Unblock()` at 7 sites.** Across `HeroTimelineSectionInjector`, `RelationNotesSectionInjector`, and `RelationNotesInjector`, popup-launch code called `Block()` to disable encyclopedia navigation while a popup was open, but if `EditPopupInjector.ScheduleShow(...)` threw before installing its `onConfirm`/`onCancel` handlers, the blocker stayed on permanently — the encyclopedia became unusable until the player restarted the entire game session. Catch blocks now release the blocker on the synchronous exception path.

### Save Data Integrity

- **Banner deserialization can no longer corrupt clan visuals.** `new Banner(bannerCode)` and `Banner.Deserialize(bannerCode)` were called with user-supplied strings (clipboard paste) at 5 unprotected sites — malformed input threw `FormatException` / `ArgumentOutOfRangeException` and could leave a clan's banner in a half-written state with the visual cache still pointing at the old image. All 5 sites now wrapped in `try/catch` with explicit logging; the watchdog path keeps `Deserialize` + `_bannerVisual` cache-clear atomic.

### Performance

- **Encyclopedia page navigation no longer hitches from per-frame reflection.** `OverrideCultureInStats` allocated `new[] { "Stats", "HeroInfo", "Information", ... }` arrays at 5 call sites on every page Refresh — promoted to `internal static readonly string[]` constants. `TryGetNativeStatsAdder` did a full `GetProperties()` + `GetConstructors()` scan per Refresh for Hero/Clan/Kingdom/Settlement pages — now caches `(PropertyInfo, ConstructorInfo, MethodInfo, ParameterInfo[])` per VM type in a `ConcurrentDictionary` after first discovery. `TickNameReapply` did 4 reflection lookups per renamed settlement — `typeof(TextObject)` reflection handles now cached as `static readonly`.
- **Popup open / encyclopedia inquiry no longer stalls on full-process type scans.** Six inline `AppDomain.CurrentDomain.GetAssemblies()` + `asm.GetTypes()` blocks scanning for `InquiryElement` and `MultiSelectionInquiryData` types were consolidated into a single new `InquiryTypeCache` class that scans once per session (lazy + lock-protected). Affected sites: `HeroEditPicker`, occupation MultiSelection, troop pickers, `QuickTagMenu`, Journal category picker.

### MCM Settings — Web Extension Subgroup Auto-Hide

- **EEWebExtension subgroup now properly hides when the extension isn't installed or `Enable Web Extension` is off.** New `WebExtensionVisibilityPatch` installs a Harmony postfix on MCMv5's `SettingsPropertyGroupDefinition.SubGroups` getter. When MCM asks the "9. Extensions" group for its subgroups, the postfix filters out the "EEWebExtension" leaf if either (a) the EEWebExtension assembly isn't loaded, or (b) the user-controlled `EnableWebExtension` toggle is off. The filter is dynamic — toggling the setting in the MCM panel makes the subgroup appear/disappear on the next render, no reload required.
- **`EnableWebExtension` toggle auto-locks off** when the EEWebExtension assembly isn't loaded. The setter refuses to accept `true`, and the getter returns `false` regardless of stored state. The "Web Extension Status" line at the top level continues to display `Not installed — place EEWebExtension module in Modules/ folder` so the user knows what's missing.
- **MCMv5-version-fragile**: tested against MCMv5 v5.11.3.0. If MCMv5 renames `SettingsPropertyGroupDefinition` or its `SubGroups`/`GroupNameRaw` accessors, the patch logs a warning to `debug.log` and bails out gracefully — settings still work, the subgroup just stays visible.

### LoreSection — Log Spam Cleanup

- `LoreSectionInjector.ForceTextAlignLeft` no longer throws a caught exception on every encyclopedia lore-section render. The method tried to read `TextWidget._text` via reflection, which failed for `RichTextWidget` (a sibling subclass of `BrushWidget`, not a descendant of `TextWidget`). The exception was always caught (no functional impact), but spammed `debug.log` with multi-line stack traces after the `ex.ToString()` change made them visible. Method now type-checks via `_twoDimTextField.DeclaringType.IsInstanceOfType(widget)` before the GetValue call and skips silently for non-TextWidget widgets.

### Diagnostics

- **Catch-block stack traces preserved.** ~981 `catch` blocks across 25 files previously logged `ex.Message` only — discarding the stack trace and inner exception chain, which made `TargetInvocationException` failures show as just "Exception has been thrown by the target of an invocation" with no source line. Project-wide replace converted `+ ex.Message)` to `+ ex.ToString())` in debug-log call sites. User-visible `InformationManager` notifications kept `ex.Message` intentionally to avoid dumping stack traces in the player's notification feed.

### Code Review Hardening (Post-Implementation Pass)

A targeted C# review surfaced 9 issues in `MCMSettings.cs` after the main batch of work above; all 9 were fixed in-place before shipping.

- **`IsEEWebExtensionLoaded` cache removed (HIGH).** The original `_eewebExtensionLoadedCache` had a startup TOCTOU: the first probe could run before EEWebExtension finished registering in the AppDomain, latching `false` and force-locking the toggle off for the entire session even if the extension loaded immediately afterward. Compounded by the cache field being non-volatile (no memory barrier for the visibility-patch thread). Fix: drop the cache. The `AppDomain.CurrentDomain.GetAssemblies()` scan walks ~50–200 assemblies per call (~1–5 µs); the function only runs on toggle interactions and the visibility-patch postfix, never in a per-tick path.
- **`WriteToLogFile` initialization race (HIGH).** `_logInitialized` was checked outside the `_logLock`, so the `EncyclopediaKeyPoller` timer thread and the main thread could both observe `false` and race the path-compute writes. Fix: moved the init check inside the lock with a proper double-checked pattern. Latent torn-string-reference risk on non-x86 JIT eliminated.
- **`WebExtensionStatus` had unreachable code (MEDIUM).** When the EEWebExtension assembly wasn't loaded, the smart-setter forced `EnableWebExtension == false`, so the getter always took the "Disabled (toggle above to enable)" branch — the user never saw the actually-helpful "Not installed — place EEWebExtension module in Modules/ folder" message, and toggling the switch did nothing because the setter refused. Fix: check `IsEEWebExtensionLoaded()` first; the three messages now correctly correspond to "not installed" / "user-disabled" / "active".
- **Bare `catch { }` in `DebugLog` and `WriteToLogFile` annotated (MEDIUM).** Both bare catches now use `catch (Exception)` with explanatory comments — they can't recursively log their own failures (the diagnostic channel IS what's failing), but the suppression is now intentional and documented rather than indistinguishable from a typo.
- **`OpenWebBrowser` silent fallback chain logs each failure (MEDIUM).** The 3-method browser-launch chain (`cmd /c start` → `ShellExecute URL` → `explorer.exe`) previously swallowed each exception silently, so support reports could only see "all 3 failed" without indicating WHICH method failed. Each fallback now logs to `debug.log` with `ex.ToString()`. Antivirus blocking, group policy, and shell-handler issues now have distinct fingerprints.
- **`RestoreHeroCulture`/`RestoreHeroOccupation` reflection cached (MEDIUM).** 8 `GetProperty`/`GetField` lookups per restore-original click consolidated into 8 `Lazy<T>` static fields resolved once and reused. Per-click cost: ~80–160 µs → ~0.5 µs after first warmup. Matches the project's existing reflection-caching convention.
- **`ExportByType` no longer allocates a dictionary just to read `.Count` (LOW).** Signature changed from `Func<Dictionary<string,string>>` to `Func<int>`; 4 export-button call sites updated to pass `() => API.GetAllX().Count`. Same allocation but clearer intent — the helper is no longer "give me the data and I'll throw it away."
- **5 carousel `index <= xxx.Count` redundant guards simplified (LOW).** After `RemoveAt(index)`, the guard's intent was "wrap to 0 when the last item was deleted." The check `index <= xxx.Count` was redundant with the trailing ternary `index < xxx.Count ? index : 0` that already handled the case. Simplified to `if (xxx.Count > 0)` plus the ternary, with a one-line comment explaining the wrap-around.

### Extra Notes

- **Existing saves are fully compatible.** No `[SaveableField]` changes, no migration needed, no JSON format changes, no language-file changes.
- **No new features.** This is a stability release — the upgrade path is "install over v2.5.0, restart Bannerlord."
- **Reduced random crashes and hangs.** If you previously experienced rare CTDs when navigating the encyclopedia, settlement names not updating after rename, or the encyclopedia "freezing" after a popup error — those classes of bugs should be gone. If you still see them after upgrading, the new `ex.ToString()` logging in `Documents\Mount and Blade II Bannerlord\Configs\ModSettings\Global\EditableEncyclopedia\Logs\debug.log` should now contain a real stack trace to share in a bug report.
- **Slightly faster encyclopedia page navigation.** The reflection caching changes eliminate multi-millisecond stalls per page open. The improvement is most visible on large clans with many heroes per page.
- **The confirmation banner ("Custom culture applied: …") now appears one tick later** than before. This is the deferred-refresh fix at work — visually identical, just on the next frame instead of mid-click.
- **EEWebExtension users:** if you don't have the companion mod installed, "9. Extensions" now contains only the `Enable Web Extension` toggle and the `Web Extension Status` row — clean and tidy. If you do have EEWebExtension and toggle it off, the subgroup vanishes; toggle it on, it returns.

---

## v2.5.0 — Full MCM Localization (2026-04-23)

Maintenance release focused on the Mod Configuration Menu. Every visible MCM label — section headers, property names, button contents, and hover tooltips — now renders in the player's selected language across all 12 supported languages (English, Turkish, German, French, Spanish, Chinese, Russian, Portuguese, Korean, Japanese, Polish, Ukrainian). Also fixes a legacy contamination bug that caused mixed-language MCM display with English selected, and a silent bypass of the "Enable Relation Notes" setting.

### MCM Translation Infrastructure

- **Per-segment group header translation** — MCM splits nested group paths like "1. Info/About" into separate `GroupName` getter calls at render time; the `McmLocalizer` now uses per-segment slug keys (`mcm_group_1_info`, `mcm_group_about`) instead of full-path slugs, making nested group translations actually reach the UI.
- **Button `Content` translation** — added a fourth Harmony postfix targeting `MCM.Abstractions.Wrapper.PropertyDefinitionButtonWrapper.get_Content`, so button labels ("Open Invite", "Show Stats", etc.) now localize alongside row labels.
- **Hint (tooltip) translation fix** — `HintTextPostfix` now captures the raw English `DisplayName` from the wrapper instance during `DisplayNamePostfix` (before it's itself translated) and derives the hint lookup key from that slug, matching the `mcm_<slug>_hint` scheme used by language dicts. Previously the postfix slugified the hint text itself, producing keys that never matched the dicts — hints were English in every language despite translations being present in `GetTurkishDefaults` / `GetGermanDefaults`.
- **`DumpDiscoveredKeys` diagnostic dump** now emits per-segment group keys and button-Content keys, so future translators see the correct keys to localize.

### Translation Data

- **English canonical override block** at the end of `GetEnglishDefaults()` — the dict had accumulated 10 historical per-language legacy blocks inside the English defaults; because C# dict initializers use last-write-wins indexer syntax, the final Ukrainian block was winning for `mcm_*` keys and producing mixed-language display when English was selected. The new canonical block re-asserts correct English values last, without deleting the legacy cruft.
- **Disk-file heal-on-load** — the localization merge logic (`Localization.Initialize`) now overwrites on-disk `mcm_*` keys with the latest in-code value when they differ. Auto-heals any user's `en.json` / other language JSON that got populated with contaminated values from a prior version. User-customized non-`mcm_*` keys (popup/message text) remain disk-wins to preserve edits.
- **Full 12-language MCM coverage** — all 87 names, 87 hints, 14 per-segment group keys, 14 button contents translated for Turkish, German, French, Spanish, Chinese, Russian, Portuguese, Korean, Japanese, Polish, Ukrainian. English remains the source of truth (fallback). Community translators can refine phrasing by editing the JSON files in `Documents\Mount and Blade II Bannerlord\Configs\ModSettings\Global\EditableEncyclopedia\Languages\<lang>.json`.
- **Single-segment group keys** (`mcm_group_4_export` through `mcm_group_9_extensions`) added for all 9 languages that were missing them.
- **Russian banner typo** — `создать свой знамёнa` → `создать своё знамя` (mixed-script typo had a trailing Latin `a`; fixed to neuter singular).

### Bug Fixes

- **"Enable Relation Notes" now respected on capture/execute popups.** The `SuggestRelationNote()` auto-popup fires when the player captures a prisoner or executes a lord. It only gated on `EnableJournal`, missing the `EnableRelationNotes` gate — users who had relation notes off still saw the add-note popup. Added the missing `EnableRelationNotes` check.

### Notes for Users

- Existing saves are fully compatible. No `[SaveableField]` changes; no migration needed.
- On first launch with v2.5.0, `debug.log` will show `[Localization] Merge: added N missing keys, overwrote M mcm_* keys (healing legacy contamination)` — `overwrote > 0` means your disk JSON had legacy English-block contamination that was auto-healed. Cosmetic diagnostic, no action required.
- If you've manually edited language JSON files (non-MCM strings), those edits are preserved. Only `mcm_*` keys are code-authoritative now.

---

## v2.4.0 — The Immersion Overhaul (2026-04-15)

The largest release since the v2.3.0 web extension launch. Every web page was rebuilt with cinematic backdrops, themed panels, and live game data. A full keyboard shortcut system was added, Quick Search became a universal command palette, and the map page got a dramatic immersion pass with teleport mechanics, a functional compass, drifting embers, day/night tinting, and a golden arrival burst animation.

### Web Extension — New Features

**Keyboard Shortcut System**

- Gmail-style two-key navigation: `G` then `H/C/K/S/M/O/R/T/A/P` to jump to Heroes / Clans / Kingdoms / Settlements / Map / cOmmander / Rankings / sTats / Api / homePage
- Detail view: `E` edit, `N`/`P` next/previous in list, `B` back to list, `J`/`K` scrub timeline entries
- Global: `/` focus search, `T` cycle theme, `D` cycle density, `H` toggle HUD, `R` refresh live data
- Compare: `C` add current hero, `Shift+C` open compare modal
- "Scribe's Codex" help overlay (`?`) rebuilt as an immersive parchment scroll with carved wooden finials, embossed gold keys, and a 3-column layout
- Floating G-prefix hint chip with pulsing pressed-key animation shows available options mid-sequence
- Shortcuts auto-disable while typing in any text input, toggleable from Preferences

**Quick Search Rewrite** (`Ctrl+K`)

- Fuzzy subsequence scorer with 5 priority tiers (exact → prefix → word-boundary → substring → subsequence with gap penalty)
- 6 categories in one palette: Heroes, Clans, Kingdoms, Holds, Pages, Actions
- Filter chips switchable via click, `Tab`/`Shift+Tab`, `Ctrl+1..7`, or inline prefixes (`/h`, `/c`, `/k`, `/s`, `/p`, `/a`, `>` for actions)
- 11 page-navigation targets + 9 action commands (Cycle Theme, Refresh Live Data, Toggle HUD, Open Preferences, Compare Heroes, etc.)
- Recently-opened entities (last 6) shown when the input is empty
- Gold match highlighting on contiguous + subsequence matches
- Grouped results with section headers and counts when filter = "All"
- Keyboard nav: ↑/↓ / Home / End / PageUp / PageDown, Enter to execute

**Map Page Immersion**

- Title cartouche (top-center) — aged parchment banner showing `CALRADIA — [KINGDOM]` with live game date / season
- Functional compass rose (top-right) — gradient gold needle, engraved N/S/E/W, gentle ±3° wander, double-click to reset
- Four filigree corner ornaments framing the map
- Drifting ember particles (15 CSS spans rising at staggered speeds)
- Edge vignette + parchment fiber grain overlay (multiply blend)
- Day/night tint — reads `gameHour` from API, applies blue-night / orange-dawn / orange-dusk / transparent-day radial overlay with 1.5s fade
- Live coordinate + zoom readout (bottom-left)
- Removed: map screenshot/PNG export button

**Teleport System**

- Double-click any settlement on the map for instant teleport
- Slideout "Teleport Here" button with gold glow and shine-sweep hover animation
- Dramatic arrival burst: 3 concentric expanding gold rings + rotating gold star + 12 ember spokes flying outward at the target point
- Auto-refresh 350ms after teleport so the player marker visibly jumps to the new location
- C# backend uses a **4-path reflection fallback**:
  1. `MobileParty.Position2D` setter via `GetSetMethod(nonPublic:true)`
  2. `MobileParty.Party.Position2D` (PartyBase) setter, same technique
  3. Hierarchy walk searching for `_position2D` / `_position` / `_mapPosition` Vec2 fields
  4. Any instance method named `SetPosition*` / `Teleport*` taking a single Vec2 param
- Works across all Bannerlord versions (previous `SetMoveGoToSettlement`-only code broke on 1.2.x)

**Clan Parties Roles**

- Assign Quartermaster / Scout / Surgeon / Engineer roles to **any** party a companion leads, not just the player's main party
- New endpoint `GET /api/player/partyroles/detail?partyId=X` returns current role assignments for pre-selection
- `POST /api/player/assignrole` now accepts a `partyId` field and resolves the target party from `MobileParty.All` or `clan.WarPartyComponents`

**Context-Aware Player HUD**

- Auto-hides on browsing pages (Heroes / Clans / Kingdoms / Settlements lists)
- Visible on Home / Commander / Map / Detail pages
- Smooth opacity + Y-translate transition
- Force-hide override via `H` shortcut

**Layout & Chrome**

- Detail views (Hero / Settlement / Clan / Kingdom) get content length limits — Timeline capped at 10 entries with expandable scrollable box, Journal rail with max-height + entry count badge
- Topbar and footer slimmed — reclaimed ~80px of vertical chrome
- Footer decorative elements hidden, gradient-clip invisibility bug fixed on `.footer-title`

### Web Extension — Earlier v2.4.0 Work

- **9 color themes** — Parchment, Iron, Oak + 6 Calradian culture themes (Empire, Aserai, Battania, Khuzait, Sturgia, Vlandia)
- **32 Achievements** across 7 categories with real reward grants (Gold / Influence / Renown / Glory)
- **9-tier Glory rank progression** (Wanderer → Legend) with claim-all support
- **Commander Hub** — 7 sub-tabs (Character, Inventory, Party, Quests, Clan, Kingdom, Chronicle) with auto-generated honorifics, SVG gauges, animated counters
- **Stats Dashboard** with 9 charts (donuts, leaderboards, age pyramid, prosperity heatmap, wealth sparkline)
- **Clan Power Rankings** with top-3 podium, filters, group-by-kingdom, CSV export, player rank
- **Caravan Ledger** trade routes with category icons, animated arrows, profit tiers, best-market recommendations
- **Compare Heroes** with skill radar overlay (up to 4 heroes)
- **Unified Preferences modal** (density, typography, themes, navigation, sound)
- **Sound system** with 4 presets (Default, Horn, Parchment, Steel), volume, per-event toggles, animated equalizer
- **Themed Notifications panel** with filter chips, search, snooze, dismiss
- **Sidebar rail navigation** with collapse-to-icon-only mode
- **Universal themed tooltips** replacing native `title` attributes everywhere
- **Cultural Pantheon panels** with per-culture color theming
- **Kingdoms War Room** mode (battle theatre cards) + **Hall of Peace** mode (peaceful realms grid)
- **Home page widgets** — quick actions, royal calendar, treasury sparkline, faction power bars, weekly highlights, news, quote of the day, season particles
- **API docs rebuild** — try-it console, Postman JSON export, animated terminal, sticky section rail, copy-as-cURL
- **Changelog/About modal rebuild** — version timeline, features grid, credits, links
- New endpoints: `POST /api/player/grantreward`, `POST /api/player/travel`, `GET /api/player/partyroles/detail`

### Main Mod — Bug Fixes

- **Battle wins counter no longer decreases over time** — battles, hero kills, troop kills, captures, and tournaments are now tracked in a dedicated persistent counter (`_battleStats`) instead of being recomputed by scanning the journal text. The 30-entry-per-hero journal trim (kept for save-size hygiene) was silently pushing old `[War] Victory` lines off the front, causing the displayed `W/L` to drop on every new entry once the cap was hit. Existing saves are auto-migrated on first load with the new build, so no progress is lost
- **Custom-culture save load timing window closed** — split `CustomCultureManager.ReregisterAllFromSaveData()` into a Phase 1 (rebuild `CultureObject`s in `MBObjectManager`) and Phase 2 (re-apply hero/settlement culture references) and now runs Phase 1 synchronously inside `SyncData`. Previously the entire re-registration was deferred to the first `OnApplicationTick`, leaving a tiny gap between save load and the first tick where `settlement.Culture` could touch a stale custom culture object. Phase 2 still defers because it depends on `Hero.AllAliveHeroes` / `Clan.Settlements` being populated

### Internal

- New `[SaveableField(19)] _battleStats` per-hero counter dictionary (format `W:N|L:N|C:N|HK:N|TK:N|T:N`)
- `IncrementBattleStatsFromText()` runs after `AddJournalEntry` dedup passes — only successful adds increment, dedup-rejected duplicates don't double-count
- `MigrateBattleStatsFromJournal()` one-time seed for existing saves on first load with the new version
- `GetBattleStats()` public getter used by both `EditableEncyclopediaPatches` (encyclopedia stat panel) and `EditableEncyclopediaAPI` (cross-mod API)
- `CustomCultureManager.RegisterOnlyFromSaveData()` — Phase 1 entry point safe to call from `SyncData`
- `HandlePlayerTravel()` in `EncyclopediaWebServer.cs` rewritten with 4-path reflection fallback for teleport position writes

### License

- Changed from "MIT · Source code will be released in future updates" to **Open Source — credits must be given to the original author (XMuPb)**

### Version

- Bumped from v2.3.6 to v2.4.0

---

## v2.3.5 — Bug Fix Release (2026-04-11)

### Critical Fix: Save/Load Detection

- **Fixed `isLoading` detection** — replaced broken `ReferenceEquals` approach with `dataStore.IsLoading`. The old method failed when saves had no prior mod data (empty dictionaries aren't replaced by `SyncData`), causing the entire load-time initialization block to be skipped
- **Fixed call order assumption** — discovered `SyncData()` fires BEFORE `RegisterEvents()` on load (opposite to code comments). Added `_loadedFromSave` flag for reliable cross-method communication

### Bug Fixes

- **Fixed Living Chronicle popup on every load** — intro dialog no longer appears when loading saved games, only on new campaign start
- **Fixed Relation Notes section visible when disabled** — added `EnableRelationNotes` MCM check before `InjectRelationNotesSection()` on Hero pages
- **Fixed EnableLoreSection not enforced** — Lore section injection now respects MCM toggle on Clan, Kingdom, and Settlement pages (was only checked on Hero pages)
- **Fixed EnableJournal not enforced** — Journal section injection now respects MCM toggle at caller site on Clan, Kingdom, and Settlement pages
- **Fixed settlement/clan/kingdom names not persisting visually** — added `TickNameReapply()` that pushes saved custom names onto game objects after load via `SetGameObjectName()` / `SetHeroName()` for all Heroes, Settlements, Clans, and Kingdoms
- **Fixed stale lore edit replaying on description confirm** — added `_popupGeneration` counter to prevent old closure callbacks from firing when a newer popup has opened
- **Fixed Timeline duplicate entries** — added `IsDuplicate()` check in `CollectOwnJournalEntries()` and preserved auto-log dedup set during load (only clears after `_campaignFullyLoaded` on first DailyTick)
- **Fixed Chronicle duplicate entries (per-page)** — added display-level dedup using `StripEntityMarkers()` to catch entries that differ only in `«h:id»Name«/h»` marker formatting
- **Fixed Global Chronicle duplicate entries** — added cross-object dedup via `HashSet` keyed on `date + stripped_text` during collection (82 → 40 entries in test save)
- **Fixed journal duplicate accumulation** — added `DeduplicateJournalEntries()` on load that removes duplicate lines from stored journal data using marker-stripped text comparison
- **Fixed `AddJournalEntry` near-duplicate bypass** — added `StripEntityMarkers` comparison so entries with different entity tags but same display text are caught before storage

### Settlement Map Nameplates

- **Custom names show on map after save+load** — Harmony Prefix on `SettlementNameplateVM.RefreshValues()` sets `VM.Name` before the game reads it. `SyncData` also modifies `TextObject.Value` in-place with cache invalidation at the earliest possible moment
- **Mid-game rename info message** — after renaming a settlement via Ctrl+N, a grey message informs the player that the map nameplate will update after saving & reloading or pressing Esc → Options → Back (Bannerlord Gauntlet engine limitation: nameplate text widgets are write-once at creation)
- **Mid-game banner edit info message** — same guidance shown after editing clan banners

### Internal

- `StripEntityMarkers()` promoted to `internal static` on `EncyclopediaEditBehavior` for cross-class access
- `TickNameplateRefresh()` added to `SubModuleClassEntry.OnApplicationTick()` for deferred nameplate refresh
- `SettlementNameplateMixin.RefreshAllNameplatesMainThread()` added for main-thread VM refresh
- Game uses `NavalMapScreen` (not `MapScreen`) when Naval DLC is active — `MapViewsContainer._mapViewsCopyCache` holds `GauntletMapSettlementNameplateView`

### Version

- Bumped from v2.3.0 to v2.3.5

---

## v2.3.0 — Culture Overhaul, Bug Fixes & New Game Intro (2026-04-08)

### New: Culture Display Overhaul

- **Culture now visually updates everywhere** — Hero, Settlement, Clan, and Kingdom Info sections all show the custom culture after changing via Ctrl+U
- `ApplyCulture()` now sets `hero.Culture` and `hero.CharacterObject.Culture` via reflection, and updates owned settlement cultures automatically
- Works on all code paths: native ViewModel stats, NavalDLC fallback widget injection, and `CultureText` property override
- Culture display resolves through `ResolveCustomCultureDisplay()` helper — checks custom display name first, then persisted culture ID, then game object

### New: Party Name Update

- Custom names now update `PartyBase.<CustomName>k__BackingField` for the tooltip title
- Calls `PartyBase.SetVisualAsDirty()` to refresh the campaign map nameplate
- Searches `MobileParty` and `PartyBase` type hierarchies for all TextObject name fields with fallback logging

### New: Description Name Replacement

- When a hero has a custom name but no custom description, the game-generated description text automatically replaces the original name with the new name (e.g., "Gylf is a head of..." becomes "Testing Name is a head of...")

### New: New Game Intro Dialog

- "The Living Chronicle" dialog appears on every new campaign start (not on save loads)
- Pauses the game while displayed, unpauses on dismiss
- Lists all keyboard shortcuts (Ctrl+E, N, B, U, G, J, F)
- Includes bug report reminder with Discord link
- Waits for campaign to be unpaused (not during character creation)

### New: Culture Safety System

- **Validation on save** — `SetCustomCulture()` validates culture ID exists in game's `MBObjectManager` before persisting
- **Orphan cleanup on load** — `ValidateCustomCulturesOnLoad()` scans all culture entries against game data and removes dead references
- **Null guard on settlement culture** — `UpdateOwnedSettlementsCulture()` refuses to apply null cultures, preventing broken militia/garrison
- **Thread-safe settlement tracking** — `OriginalSettlementCultures` dictionary accessed under `lock()` with snapshot iteration in `SaveSanitizerPatch`
- **Original culture validation** — `StoreOriginalCulture()` rejects invalid values ("null", "None", strings shorter than 2 chars)
- **Custom culture definition validation** — `SetCustomCultureDefinition()` validates required fields before saving

### Bug Fixes

- **Fixed 5 Substring crashes** — empty tag `.Substring(0,1)` in RelationNotesSectionInjector (5 locations), missing index checks in SearchPanel (4 locations), hardcoded offsets in EncyclopediaEditBehavior purge methods
- **Fixed dictionary enumeration crash** — `PurgeOrphanedCustomEntries()` now snapshots `.Keys` before iterating to prevent `InvalidOperationException`
- **Fixed reflection null** — `DescriptionFormatter` FontSize property `GetValue()` result checked for null before casting
- **Fixed empty name crash** — `TimelineReflectionCache` camelCase builder handles empty strings
- **Fixed browser launch** — MCM "Open Web Encyclopedia" button now tries 3 methods: `cmd /c start`, `UseShellExecute`, `explorer.exe`
- **Fixed banner export timing** — waits for `Campaign.TimeControlMode != Stop` (not during character creation dialogs)
- **Fixed banner cleanup** — old banners now deleted on new session/save load before re-exporting

### MCM Group Ordering

- Extensions section now correctly appears at bottom of MCM (GroupOrder fix for parent group)

### Version

- Bumped from v2.2.0 to v2.3.0

---

## v2.2.0 — Web Extension Support & Party Name Fix (2026-04-07)

### New: EEWebExtension MCM Settings

Added 17 new MCM settings under **9. Extensions / EEWebExtension** to configure the companion web mod directly from the in-game settings menu:

#### Web Server Settings

- **Web Server Port** (1024-65535, default 8080) — change if port 8080 is in use
- **Auto-Open Browser** — automatically launch the web encyclopedia when a campaign loads
- **Allow External Access** — open the server to your local network so you can browse on your phone or tablet
- **Enable Editing from Web** — toggle read-only mode (safe for streaming or sharing with viewers)
- **Enable Portrait Extraction** — toggle game portrait rendering and export

#### Live Sync Settings

- **Live Sync on Detail Views** — auto-refresh detail pages with game data in real-time
- **Live Sync Interval** (2-60 seconds, default 8) — how often detail views poll for updates
- **Live Chronicle Updates** — push new chronicle events to the web home page feed

#### Web Display Settings

- **Show HUD Stats** — player stats bar (gold, influence, health, troops)
- **Show Intro Screen** — the cinematic Viking intro animation on first load
- **Show Ember Particles** — floating fire motes (disable for performance)
- **Show Gold Spark Trail** — cursor spark effect (disable for performance)
- **Enable Sound Effects** — UI hover and click sounds
- **Enable Scroll Animations** — card reveal animations on scroll
- **Cards Per Page** (20-200, default 60) — list view pagination

#### Quick Actions

- **Open Web Encyclopedia** — one-click button to launch the browser

### New: Web Extension Settings API Endpoint

- `GET /api/web-settings` — returns all MCM web extension settings as JSON, consumed by the web app on startup to apply user preferences

### Bug Fixes

#### Party Name Fix

- **Fixed: Custom names now update the MobileParty name on the campaign map**
  - When renaming a hero via Ctrl+N or the web interface, the party tooltip on the campaign map (e.g. "Hrothnir's Party") now correctly shows the new name
  - Uses three fallback strategies: `SetCustomName()` method, `_customName` field, `_name` field — with diagnostic logging if none found
  - Fixes the disconnect where the encyclopedia showed the new name but the map tooltip still showed the old one

#### Custom Data Merging in API Responses

- **Hero list API** (`GET /api/heroes`) now returns custom names and cultures from `EncyclopediaEditBehavior` instead of the game's original values
- **Hero detail API** (`GET /api/hero/{id}`) now returns custom names, titles, and cultures
- **Clan list API** (`GET /api/clans`) now returns custom names and banner codes
- **Clan detail API** (`GET /api/clan/{id}`) now returns custom names and banner codes
- **Kingdom list API** (`GET /api/kingdoms`) now returns custom names and banner codes
- **Kingdom detail API** (`GET /api/kingdom/{id}`) now returns custom names and banner codes
- **Settlement list API** (`GET /api/settlements`) now returns custom names
- **Settlement detail API** (`GET /api/settlement/{id}`) now returns custom names
- Previously, editing a name/culture/banner via the web would save successfully but the web interface wouldn't reflect the change until the game was restarted

### MCM Group Ordering Fix

- Extensions section now correctly appears at the bottom of the MCM settings list (after 8. Debug)
- Fixed `GroupOrder` not being respected when properties were declared in subgroups — moved `EnableWebExtension` and `WebExtensionStatus` to the parent group level

### Version

- Bumped version from v2.1.0 to v2.2.0 in MCMSettings and SubModule.xml

---

## v2.1.0 — Living Encyclopedia, Web UI & LoreStory Expansion

### New: EEWebExtension — Live Web Encyclopedia

- Extracted web server into a **separate `EEWebExtension.dll`** module (install/remove independently)
- Local REST API on `http://127.0.0.1:8080/` with **40+ endpoints** covering all data
- **Cinematic intro sequence** — animated "Entering the Living Chronicle" with synthesized orchestral audio (war horn drone, taiko drums, choir pad, wind atmosphere)
- **Click to Enter** prompt solves browser autoplay policy for audio

#### Web UI — Home Page

- **Player Summary panel** — portrait, clan banner, culture bar, 6 stats (Gold, Troops, Morale, Fiefs, Health, Influence)
- **World Status panel** — active wars with pulsing count + war matchup ticker, strongest kingdom, most fiefs, wealthiest clan — all clickable to navigate
- **Grand Archive** — animated counting numbers on scroll, theme-colored hover glows per section (Heroes=red, Clans=purple, Settlements=gold, Kingdoms=blue)
- **Browse Sections** — category cards with hover arrow animation
- **Live Chronicle** — scrolling event feed on home page, auto-refreshes every 15 seconds

#### Web UI — Heroes Page

- **Custom hero card template** — portrait header (120px), culture-colored accent bar, occupation badge (color-coded: Lord=purple, Wanderer=green, Notable=orange, Companion=blue), clan banner watermark, dead state (grayscale + "Deceased" label), age display
- **Hero portrait extraction from game** — `CharacterImageTextureProvider` renders 3D hero faces at 256×174, saved as PNG to `Portraits/` folder with culture-specific color correction (adaptive per faction)
- **Filters** — Relation (Met/Not Met), Gender, Culture, Occupation, Marital Status with dynamic counts
- **Detail page** — relation intensity bars (sorted by strength), family cards with relationship icons (ring for spouse, baby for child), section count badges, improved empty states

#### Web UI — Clans Page

- **Custom clan card template** — banner (48px), tier stars with gold glow, kingdom badge with faction color, strength/members/fiefs stats, war pulse animation, bandit/minor faction labels
- **Clan detail** — tier badge with gold stars, Clan Power gauges (Renown, Influence, Strength, Wealth), territory summary (Towns/Castles/Villages cards)
- **Filters** — Type (Kingdom/Minor/Bandit/Independent), Status (Active/Eliminated), Diplomacy (Ally/Enemy/Neutral), Tier, Culture, Kingdom
- **C# API** — clan list now includes: tier, leader, members, fiefs, troops, garrisons, strength, renown, influence, wealth, isMinorFaction, isBandit, wars[]

#### Web UI — Settlements Page

- **Custom settlement card template** — type image header, culture-tinted background, garrison shield badge, prosperity bar (heat-colored: green/orange/red), war badge, type-specific border glow (gold for towns, silver for castles, green for villages), thriving prosperity pulse
- **Settlement Dashboard** (filter=All) — overview counters, Prosperity Rankings, Culture Distribution donut, Kingdom Territories (stacked bars), Garrison Strength rankings, Economy Board (trade tax, workshops, village production)
- **Settlement detail** — SVG gauge rings (Prosperity/Loyalty/Security/Food) with animated fill, Garrison vs Militia split bar with segment percentages, workshop cards with type icons, "Part of" Kingdom panel, cinematic banner header
- **Filters** — Type, Culture, Kingdom, Owner Clan, Prosperity Range (Thriving/Growing/Struggling), Garrison Size (Heavy/Light/Undefended)
- **C# API** — settlement list now includes: prosperity, loyalty, security, foodStocks, garrison, militia, tradeTax, workshopCount, governor, kingdom, villageProduces

#### Web UI — Kingdoms Page

- **Kingdom Dashboard** (filter=All) — Power Rankings with fallback (fiefs+clans when strength=0), Power Balance dual-ring donut (240px), War Network (enlarged nodes r=24, 480×420 SVG), Kingdom Comparison (5-metric pentagon radar with distinct colors + winner highlighting), Live ticker at top
- **War Board** (filter=At War) — VS split cards with faction-colored backgrounds, diagonal slash, animated crossed swords, power balance bar with faction colors
- **Kingdom Comparison** — dropdown select, side-by-side banners, animated bar metrics, radar chart, verdict badge with score
- **Filters** — Diplomacy (Own Kingdom/Allies/At War/Neutral), World Wars, Culture
- **Kingdom strength** — fallback calculation: sums party troops + garrison troops when `TotalStrength` returns 0

#### Web UI — Chronicle Page

- **Date-grouped events** with gold timeline dots and separator lines
- **Category-colored event cards** — War (red), Death (dark red), Family (pink), Politics (blue), Siege (tan), Crime (orange), Diplomacy (green), Victory (gold)
- **Larger timeline icons** (28px) color-matched to category with glow
- **Sidebar stats** in 2×2 grid with icons (Events, Journal, Descriptions, Tags)

#### Web UI — Sound System (Bannerlord-style)

- **UI sounds** — leather click (filters), page turn whoosh (nav), scroll chime (card click), subtle tick (hover)
- **War sounds** — battle ambiance on war card hover: 8 randomized sword clash strikes (metallic ring at 3000-6000Hz) + crowd noise + war horn drone
- **Kingdom sounds** — royal horn (peaceful kingdom hover), ascending scale notes (power rankings), chime (donut segments), deep thud (network nodes), regal chord (comparison select)
- **Settlement sounds** — wooden creak (card hover), coin clink (prosperity rankings), stone thud (territory), armor rattle (garrison), market bustle (economy)
- **Hero/Clan sounds** — leather+chainmail equip (hero hover), banner unfurl whoosh (clan hover)
- **Home page sounds** — quill scratch (archive stats), book open (browse sections), soft coin (player stats), torch flicker (world status), parchment unfold (chronicle events)
- **HUD sounds** — coin clink (gold up), armor rattle (troops up), warning tone (HP drop), changes only play after first load

#### Web UI — Player HUD Bar

- Fixed bottom bar: Gold, Speed, HP, Troops, Food, Morale, Influence, Daily Wage
- Ship Health conditionally shown (Naval DLC)
- Styled tooltips matching in-game hints
- Value change flash animation, staggered entrance, hover effects
- Auto-updates every 5 seconds

#### Web UI — Top Bar & Navigation

- Scroll-aware compact mode (72px → 56px on scroll)
- Gold highlight line, nav hover underline preview, active glow
- Brand logo hover scale, smooth transitions

#### Web UI — Live Auto-Refresh

- **Every 15 seconds** — reloads all data, re-renders only the current page
- **Smart refresh** — detail views protected from disruption
- **Page navigation** — immediately fetches fresh data when switching tabs
- **HUD** — updates every 5 seconds independently

#### Web UI — Banner System

- Banner colors use raw XML values (no sRGB conversion)
- Background patterns at 30% opacity, mesh 11 skipped (was causing green tint)
- 251 colors extracted via `BannerManager.GetColor(int)` at runtime
- Missing icons auto-extracted from game texture atlases

#### Web UI — Hero Portrait Extraction

- `POST /api/extract-portraits` endpoint
- Uses `CharacterImageTextureProvider` + `CharacterCode.CreateFrom()` to render 3D hero faces
- Multi-frame processing via `OnApplicationTick()` queue (one hero per ~30 frames)
- `TransformRenderTargetToResource()` called before save for render target textures
- `SaveToFile(string, bool)` with `GetPixelData` fallback
- Culture-specific color correction (Sturgia/Nord=heavy, Khuzait/Battania=moderate, Empire=balanced, Vlandia=light, Aserai=minimal)
- Auto-clears portraits on new session/save load
- Auto-extracts on first web page load if fewer than 10 portraits exist

### New: MCM Extensions Tab

- Added "9. Extensions" settings group in MCM
- **Enable Web Extension** toggle — controls whether EEWebExtension starts the web server (requires restart)
- **Web Extension Status** read-only field — dynamically detects if EEWebExtension assembly is loaded

### New: LoreStory System

- **16 lore field categories** across 4 entity types:
  - Heroes: Backstory, Personality, Goals, Relationships, Rumors & Secrets
  - Clans: Founding, Territory, Traditions, Rivals
  - Kingdoms: History, Laws, Culture & Customs, Military
  - Settlements: History, Economy, Landmarks, Legends
- **19 occupation-specific template files** (Lord, Merchant, Wanderer, GangLeader, Headman, etc.)
- **60+ dynamic placeholders** — identity, affiliation, location, family, stats, social, gender-aware, time
- **Rich text formatting** — `**bold**`, `*italic*`, `__underline__`, `~~strikethrough~~`, `# Heading`, `> Quote`, `---` divider, `{small}text{/}`, `{red}text{/}`, `{#RRGGBB}text{/}` custom hex colors
- **12 named color presets** — red, blood, gold, amber, blue, green, purple, silver, white, orange, teal, brown
- **Cross-reference links** — `[[Name]]` renders as golden highlight
- **Deterministic assignment** — stable hash ensures same hero always gets same template entry
- **Duplicate prevention** — tracks assigned entries per session

### New: Auto-Chronicle System

- **16+ game event types** auto-logged: battles, sieges, raids, war declarations, peace treaties, clan movements, settlement ownership changes, hero deaths, prisoner captures/releases, marriages, births, tournaments
- **Battle reports** — logs attackers/defenders with troop counts, casualties, location, involved heroes
- **Settlement timeline** — who attacked, who owned, what raids happened
- **Kingdom timeline** — wars fought, rulers changed, clans joined/left
- **Duplicate prevention** — checks for exact match and same-text-different-date duplicates
- **30 entries max per entity** — prevents save bloat

### New: Multi-Level Undo

- **10-level undo stack** (was 1 level) — `Ctrl+Z` repeatedly to undo up to 10 edits
- Shows remaining undo count in status message
- Stack persists across page navigation within session

### New: Public API Enhancement

- Added `SetDescription()` method to `EditableEncyclopediaAPI` for cross-mod description writes

### Bug Fixes

- **Fixed description edit popup not pre-populating input field** — now passes existing text via deferred reflection injection
- **Fixed `ShowNativeTextInquiry` fallback** — now also pre-populates the input field
- **Fixed green banner tint** — `BannerIcons/11.png` was a solid green square causing overlay on all banners
- **Fixed hero portrait scope bug** — `getPortraitSrc()` moved to top-level function (was scoped inside `openDetail`, caused crash)
- **Fixed settlement owner banner** — checks both `d.owner.clan` and `d.clan` paths
- **Fixed filter active state** — now clears across ALL sidebar sections, not just current
- **Fixed kingdom strength** — fallback sums party+garrison troops when TotalStrength property returns 0

### Code Cleanup

- Removed `EncyclopediaWebServer.cs` and `StoryExporter.cs` from core mod (moved to extension)
- Removed `Ctrl+P` export handler from core mod
- Removed web server start/stop calls from `SubModuleClassEntry.cs`
- Removed `Resources/EncyclopediaTemplate.html` build copy target from `.csproj`

---

## v2.0.0 — Major Release

### Code Quality Overhaul (28 files)

- Replaced 200+ empty `catch { }` blocks with diagnostic logging via `MCMSettings.DebugLog()`
- Added null checks after 100+ reflection lookups before calling GetValue/SetValue/Invoke
- Introduced `SafeGet()` helper in EditableEncyclopediaAPI replacing 50+ bare try-catch stat lines
- Added reflection caching in TagWidgetInjector (15+ lookups cached with lazy initialization)
- Fixed thread safety: `_transitionLock` for page transitions, `_stateLock` for GlobalChroniclePanel
- Added game object validity checks on 14 event handler arguments in EncyclopediaEditBehavior
- Replaced string concatenation with StringBuilder in 10+ loops across 6+ files
- Extracted 100+ magic numbers to named constants across all major files
- Added Substring bounds checks at 15+ locations across 5+ files

### Ctrl+N Name/Title Editing Overhaul

- Now uses EditPopupInjector (same popup as Ctrl+E) with portrait, character counter, and proper layout
- Added configurable max name length (MCM setting, default 100, range 10-200)
- Added character filtering: strips control characters, newlines, tabs with user warning
- Preview shows "Native Name: Skorin" / "Native Title: Lord" for reference
- Custom button text: Hero name shows "Next (Edit Title)", others show "Done"
- Custom tip text: "Enter a new name below. Max 100 characters. Leave empty to reset to native name."
- Fixed shared `_popupOpen` flag bug — split into `_namePopupOpen` and `_titlePopupOpen`
- Title cancel now shows yellow message: "Title edit cancelled. Name change was saved."
- Deduplicated code: extracted `ApplyHeroNameOnObject()`, `RefreshPageAndTimestamp()`, `ShowConfirmationIfEnabled()`, `RefreshSettlementNameplate()` helpers
- Added Concept page type support (5 entity types total)
- Added `SettlementNameplateMixin.ScheduleDeferredRefresh()` with retry logic

### Timeline Improvements

- Timeline section now always visible on hero pages (even with 0 events)
- Shows "No events recorded yet" empty state message when no events exist
- `IsTimelineWorthy()` now includes all [War] and [Politics] entries from hero's own journal
- Timeline data collected from 3 sources: own journal, cross-referenced journals, native game log

### JSON Export/Import

- Bumped format from v10 to v11 (19 data sections)
- Added `relationNoteTags` and `relationNoteTagLocks` to export/import
- These were previously saved in game saves (SaveableField 17 & 18) but lost on JSON transfer

### Bug Fixes

- Fixed 5 missing combo state resets in `KeyEventPoller.EnsureRunning()` (Ctrl+N/B/G/J/F could fire on encyclopedia reopen)
- Fixed CS1503 `out _` discard type mismatch in SettlementNameplateMixin
- Fixed CS0169 unused cached reflection fields in TagWidgetInjector
- Fixed CS0103 `Timeline.` namespace prefix error
- Made `ShowNativeTextInquiry` internal for cross-class fallback access

### EditPopupInjector Enhancements

- Added `description` parameter: separate preview text from input text
- Added `confirmText` parameter: custom confirm button text
- Added `tipText` parameter: custom tip text with `PendingCustomTipText` deferred injection

### New MCM Settings

- Max Name Length (10-200, default 100)

### New Localization Keys

- `name_edit_title_concept`, `name_edit_desc_hero`, `name_edit_desc_nonhero`, `title_edit_desc`
- `name_edit_cancelled_title_kept`, `name_invalid_chars`
- `edit_tips_name`, `edit_tips_title`, `timeline_no_events`

### Documentation

- Created QUICKSTART.md — 2-minute onboarding guide
- Created STEAM_DESCRIPTION.md — Steam Workshop listing
- Created NEXUS_DESCRIPTION.md — Nexus Mods listing
- Created CHANGELOG.md
- Updated README.md, CODEBASE.md, CODEBASE_EXPLANATION.md, INTEGRATION.md

### Relation Notes & Timeline

- Hero Timeline section injected on hero pages with pagination, category filters, and search
- Relation History tracking with dated entries
- Auto-Chronicle system logging 16+ game event types
- Global Chronicle Panel on campaign map
- Tag categories, presets, and per-hero auto-tag thresholds

### Data Management

- Export/Import expanded to 19 data sections (v11 format)
- Auto-export includes all data sections
- Auto-import includes all data sections
- Backward compatible with v1 through v11 files

---

## v1.1.4

- Auto-Import now loads Cultures & Occupations from JSON
- Culture Editing Fix — CharacterObject-first reflection with full fallback chain
- Occupation Editing Fix — dual-set reflection pattern across all 5 code paths
- Native Search in Carousel — culture and occupation selection carousels have built-in search
- Reflection Guard — if culture/occupation reflection fails, change is not saved
- Reset Safety — warning if original culture/occupation no longer exists
- Display Name Validation — custom culture display names capped at 40 characters
- Bulk Delete for cultures and occupations via MCM
- Custom Names for Settlements, Kingdoms & Clans via Ctrl+N
- Settlement Hover Tooltip Culture
- Villages Included in Culture Updates

## v1.1.3

- Culture Editing (Ctrl+U) — clickable carousel, custom culture names
- Occupation Editing (Ctrl+O) — clickable selection, friendly display names
- Dynamic Occupation Discovery from game's enum at runtime
- Manage Custom Cultures/Occupations in MCM
- JSON Export v3 with cultures and occupations sections
- Culture/Occupation API methods

## v1.1.2

- Localization System — 11 built-in languages
- Language Selection via MCM
- Fix: Hotkeys firing when encyclopedia is closed
- Fix: Hotkeys blocked on non-standard map screens
- Fix: Encyclopedia close detection after edit/reset/undo

## v1.1.1

- Undo Last Edit (Ctrl+Z)
- Auto-Export on Save
- Description Statistics in MCM
- Reset All Descriptions
- Visual Indicator ([Edited] prefix)
- Export by Type
- Character Limit (default 5000)

## v1.1.0

- Reset to Default (Ctrl+R) with confirmation
- Export/Import via MCM (JSON format)
- Cross-mod Import API
- Dynamic hint text

## v1.0.0

- Initial release
- Hero, Clan, Kingdom, Settlement page editing
- Persistent storage in campaign save files
- MCM v5 settings
- Debug mode
