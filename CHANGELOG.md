# Changelog

All notable changes to Editable Encyclopedia are documented here.

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
