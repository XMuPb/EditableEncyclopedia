# Editable Encyclopedia

A Mount & Blade II: Bannerlord singleplayer mod that lets you rewrite the encyclopedia. Open any Hero, Clan, Kingdom, or Settlement page, hit a `Ctrl+<key>` shortcut, and edit it: the description, the name and title, the banner, a hero's culture or occupation, tags, lore fields, journal notes, hero-to-hero relation notes, and a personal timeline. Everything you write is saved in the campaign file and survives save/load.

It also keeps a running chronicle on its own. The auto-journal watches campaign events (battles, sieges, deaths, diplomacy, marriages, births, tournaments and more) and logs them as dated, colour-tagged notes on the entities involved. There's a campaign-map panel that aggregates all of that into one filterable history.

No config files to touch. Everything is driven by hotkeys on encyclopedia pages, with the rest exposed through MCM.

## The basics

- **Module Id:** `EditableEncyclopedia`
- **Display name:** Editable Encyclopedia
- **Version:** v2.5.2
- **Type:** Singleplayer only (`SingleplayerModule=true`, `MultiplayerModule=false`, `Official=false`)
- **Target:** .NET Framework 4.7.2 / C# 9.0, x64. Tested on Bannerlord 1.3.13 and newer, including v1.4.5 (War Sails).
- **Entry point:** `EditableEncyclopedia.SubModuleClassEntry` in `EditableEncyclopedia.dll`
- **License:** Open source, credit the original author (XMuPb). (Earlier releases shipped under MIT; the change is recorded in the CHANGELOG.)

### Load order

`DependedModules` in `SubModule.xml`, in order. Note that **EE-Core is now the first dependency** — it's a custom module and it's declared before the game's own `Native`.

1. `EE-Core`
2. `Native` (e1.0.0)
3. `SandBoxCore` (e1.0.0)
4. `Sandbox` (e1.0.0)
5. `StoryMode` (e1.0.0)
6. `Bannerlord.Harmony` (v2.4.2.225+)
7. `Bannerlord.ButterLib`
8. `Bannerlord.UIExtenderEx`
9. `Bannerlord.MBOptionScreen` (v5.0.0) — this is MCM v5; it's declared by its module Id, not `Bannerlord.MCMv5`

All nine are non-optional. In the launcher, enable Editable Encyclopedia after Harmony, ButterLib, UIExtenderEx, and MCM. Then start or load a campaign. Default key to open the encyclopedia is `N`.

## What it does

Open the encyclopedia (`N`), go to a page, and use a shortcut. The poller only acts when a real encyclopedia object is being tracked, so the keys do nothing on the campaign map proper.

| Shortcut | Action                                          | Page types                                     |
| -------- | ----------------------------------------------- | ---------------------------------------------- |
| `Ctrl+E` | Edit description, or open the hero field picker | Heroes, Clans, Kingdoms, Settlements           |
| `Ctrl+N` | Edit name / title                               | Heroes, Clans, Kingdoms, Settlements, Concepts |
| `Ctrl+B` | Edit banner / flag (paste a banner code)        | Heroes, Clans, Kingdoms, Settlements           |
| `Ctrl+U` | Change culture (clickable carousel)             | Heroes                                         |
| `Ctrl+O` | Change occupation (friendly names)              | Heroes                                         |
| `Ctrl+G` | Add / remove tags                               | All                                            |
| `Ctrl+J` | Add a chronicle note (auto-dated)               | All                                            |
| `Ctrl+F` | Write / edit a hero relation note               | Heroes                                         |
| `Ctrl+R` | Reset to the original description               | All                                            |
| `Ctrl+Z` | Undo the last change                            | All                                            |

`Ctrl+N` on a hero is a two-step flow: name first (the button reads "Next (Edit Title)"), then title. On other entity types it's one step ("Done"). Names are sanitised (control characters stripped, length capped, default 100), and the original is shown in the popup so you can blank the field to reset. If the custom Gauntlet popup fails to build, it falls back to the engine's native text inquiry.

What's editable per page type:

- **Heroes** — description, 7 lore fields, name, title, banner, culture, occupation, tags, journal, relation notes, timeline
- **Clans** — description, name, banner, tags, journal
- **Kingdoms** — description, name, banner, tags, journal
- **Settlements** — description, name, banner, tags, journal
- **Concepts** — name only

### Hero lore fields and templates

`Ctrl+E` on a hero opens a field picker with seven fields: Description, Backstory, Personality, Goals, Relationships, Rumors, and Chronicle. The five narrative ones render in a dedicated "Lore" section, left-aligned. Description is the main encyclopedia text. Chronicle is a running journal that auto-stamps the in-game date on every entry.

Each field ships with writing prompts for five character roles: Lord, Merchant, Wanderer, Gang Leader, Preacher. The templates carry placeholders (`{name}`, `{culture}`, `{settlement}`, `{clan}`, `{faction}`, `{occupation}`, `{date}`) that get filled from the hero. When a lore field is empty, the matching role template shows as the default content.

### Auto-chronicle

The auto-journal subscribes to 16+ campaign event types and writes dated notes onto the entities involved. Entries carry a coloured category tag: `[War]` (red), `[Politics]` (blue), `[Crime]` (orange), `[Family]` (green). Deduplication stops the same event being logged twice in one game day, and each entity is trimmed to a cap of 30 entries.

The auto-journal subscribers are gated on the `EE-ChronicleNoters` peer module via `PeerRegistry`. With it disabled, no new chronicle entries are created, but existing `_journalEntries` data still saves and loads. Auto-tags and custom-culture reapply run regardless.

### Journal, global chronicle, relation notes, timeline

- **Journal section** — collapsible, paginated (default 10 per page, configurable), with category filter toggles. Hero names render gold, settlement names cyan.
- **Global Chronicle Panel** — a "Chronicle Notes" button sits bottom-right on the campaign-map HUD. It opens a slide-up overlay aggregating chronicle notes across all kingdoms, clans, settlements, and heroes, with category filters (War / Politics / Crime / Family / Other), source-type filters (Kingdom / Clan / Settlement / Hero), pagination, and clickable entity names that jump you to the page. Navigation goes through `entity.EncyclopediaLink`, which works even under War Sails where some page handlers are missing.
- **Relation Notes section** — a collapsible "Relation Notes" block on a hero page lists every hero with a note about the one you're viewing. Click to edit, with a delete button and the current relation score shown. Relation score changes are also tracked automatically with dated history entries.
- **Timeline section** — a collapsible "Timeline" on hero pages, always visible even at zero events ("No events recorded yet…"). It's the hero's personal biography: only events they took part in (battles, sieges, raids, captures, kills, family events, clan changes, tournaments). World-level events stay in the Chronicle. Data is pulled from three sources: the hero's own journal, cross-referenced journals, and the native game log.

### Tags

`Ctrl+G` puts player-defined tags (`ally`, `enemy`, `target`, whatever) on any entity. Placement is page-type-aware (left panel for Kingdom/Clan, after the anchor for Settlement/Hero) and tags are included in exports. Display size is configurable from 50% to 300% (default 130%).

Auto-tags are generated from live game state and shown dimmed (75% opacity, 85% font scale) to set them apart from manual ones: `Auto: Friend` / `Auto: Enemy` (relation thresholds), `Auto: Dangerous` (large party), `Auto: Prisoner`, `Auto: Rich`, `Auto: At War`, `Auto: Ruler`, and others, plus settlement tags like `Auto: Nearby` and `Auto: Under Siege`. Thresholds are configurable globally and per-hero. Auto-tags are runtime-only and never saved.

### Cultures and occupations

`Ctrl+U` swaps a hero's culture through a carousel (no typing) and can create a brand-new custom culture with a display name, a base culture, and a troop tree cloned from that base. Changing a culture actually writes `hero.Culture` and `hero.CharacterObject.Culture` and propagates to settlements owned by clan-leader heroes. Custom cultures register as real `CultureObject` instances with the engine. Because the serializer can't write references to a culture that doesn't exist at load, a save sanitizer swaps custom settlement cultures back to vanilla just before save and restores them after; the assignment itself survives in the save dictionaries.

`Ctrl+O` changes occupation with friendly names ("Gang Leader" instead of "GangLeader"). Occupation types are discovered at runtime, so types added by other mods show up too.

Both have manager dialogs in MCM to view and delete assignments and restore the originals.

### Undo, reset, import/export

Undo is a 10-level stack (it was single-level in early versions) and works for edits and resets alike. Reset takes you back to the original game description with a confirmation. "Reset All Descriptions" from MCM wipes every custom description in the current campaign, with a confirmation, and can't be undone.

Export writes all custom data to `descriptions_export.json` (format **v11**, 19 data sections) and import merges it into another campaign, overwriting entries with the same id. You can export by type (Heroes / Clans / Kingdoms / Settlements / Banners only), and toggle Auto-Export on Save and Auto-Import on Load. The importer reads v1 through v11; missing sections are skipped.

```
Documents\Mount and Blade II Bannerlord\Configs\ModSettings\Global\EditableEncyclopedia\descriptions_export.json
```

### War Sails (v1.4.5)

Everything works under the Naval DLC, which replaces encyclopedia initialization and drops some page handlers. The mod restores the missing handlers, the Chronicle Panel can still navigate to any Clan/Hero/Settlement page, and Harmony finalizers on `SetEncyclopediaPage`, `ExecuteLink`, and `OnTick` keep the missing handlers from crashing. Compat detection runs at startup and logs to `Logs\debug-compat.log`.

## Settings (MCM)

Configuration lives under Mod Options → Editable Encyclopedia, ~71 options. Settings are an MCM v5 `AttributeGlobalSettings<MCMSettings>`; read them at runtime via `MCMSettings.Instance`. If MCM isn't installed, `Instance` is null and the mod falls back to hard-coded defaults everywhere.

Nine groups:

1. **Info / About** — Author, Version, Join Discord, Encyclopedia Edit Statistics (a breakdown popup of every custom edit), Open Config Folder
2. **General** — Hints (per-shortcut hint toggles, all on by default) and Display (confirmation messages, `[Edited]` indicator, edit timestamp)
3. **Editing** — Pages (enable per entity type), Features (enable per feature, plus auto-tag thresholds, tag font scale, journal/chronicle page sizes, kingdom-colour protection, Create Custom Banner), and Management (Manage Custom Cultures / Occupations / Tags / Tag Presets / Tag Categories, Filter by Tag)
4. **Export** — Auto-Export on Save plus the per-type export buttons
5. **Import** — Auto-Import on Load, Import All, Import Banners Only
6. **Reset** — Reset All Descriptions
7. **Advanced** — key poll delay (default 500ms) and interval (default 50ms), max description / narrative / stats lengths (default 5000 each, 0 = unlimited), max name length (default 100), and the language dropdown (12 languages)
8. **Debug** — Debug Mode (verbose logging) and Show Debug On Screen
9. **Extensions** — EEWebExtension settings; the toggle locks off and the section reports "Not installed" unless the optional `EEWebExtension` module is present

Localization covers 12 languages: English, Turkish, German, French, Spanish, Chinese, Russian, Portuguese, Korean, Japanese, Polish, Ukrainian. The MCM labels themselves are also translated.

**Debug log** goes to `…\EditableEncyclopedia\Logs\debug.log`, written only when Debug Mode is on. It rotates to `debug.old.log` past 5 MB. The static `MCMSettings.DebugLog(string)` is the logging entry point used across the codebase, and it's safe to call when MCM isn't installed.

## How it works

### Startup

`SubModuleClassEntry.OnSubModuleLoad()` creates and registers the UIExtenderEx mixins (which is how settlement nameplates get their banner/name refresh on the map), then initializes localization from the MCM language setting (fallback `en`). `OnGameStart()` registers `EncyclopediaEditBehavior` as a campaign behavior and, on first start, runs Harmony setup: `PatchAll()` for the attribute-based page patches plus four manual patches — encyclopedia list names, clan banner reversion, the save sanitizer, and the settlement tooltip culture line. `OnSubModuleUnloaded()` unpatches everything.

### The core loop

`OnApplicationTick(dt)` runs every frame and drives all the deferred, main-thread work: banner refresh and watchdog, the deferred Gauntlet popup show, every `*SectionInjector`'s `TickMainThread()`, the char-limit override, custom-culture reapply after a save load, and deferred settlement cultures. This indirection exists because Harmony patches fire on background threads and the key poller runs on a timer thread, but Gauntlet UI is not thread-safe and must be touched on the main thread. So everything queues a flag and gets picked up here.

### UI injection

Each `*SectionInjector` follows one pattern. A Harmony **postfix** on a page's `Refresh()` walks the live Gauntlet widget tree, finds an anchor (usually `EncyclopediaDividerButtonWidget`), and injects the custom widgets — timestamp, tags, lore section, journal, relation notes, timeline — as siblings or children. Anything that can't run during the patch is deferred to `TickMainThread()`. On non-map screens (Clan / Party / Inventory) where the encyclopedia overlay isn't ready yet, injection is deferred ~200ms and retried until the layer appears.

### Persistence

`EncyclopediaEditBehavior` is a `CampaignBehaviorBase` singleton (`EncyclopediaEditBehavior.Instance`). It holds 18 `[SaveableField]` dictionaries (IDs 1–18), all keyed by entity `StringId`, often with a typed key prefix:

`_customDescriptions`, `_customNames`, `_editTimestamps`, `_customBannerCodes`, `_customCultures`, `_customOccupations`, `_customCultureDefs`, `_heroInfoFields`, `_customTags`, `_journalEntries`, `_relationNotes`, `_relationHistory`, `_tagNotes`, `_tagCategories`, `_tagPresets`, `_perHeroAutoTagThresholds`, `_relationNoteTags`, `_relationNoteTagLocks`.

A 19th field, `_battleStats` (per-hero counters for wins, losses, captures, hero-kills, troop-kills and tournaments, which don't decay with the journal-trim cap), was added later out of sequence as ID 19. `SyncData(IDataStore)` is the entry point; it syncs all of these plus a non-attributed timeline-collapse dictionary, detects load via `dataStore.IsLoading`, and queues the reapply flags (`NeedsBannerReapply`, `NeedsCustomCultureReapply`, `NeedsNameReapply`) for the next tick. The save definer registers under ID 82946102.

### Threading

The key poller runs on a `System.Threading.Timer` and reads hotkeys via the `GetAsyncKeyState` P/Invoke (initial delay 500ms, poll interval 50ms, both configurable). When it detects a combo, it queues an action for the main thread rather than touching UI directly. Close detection uses a three-tier strategy (cached `IsEncyclopediaOpen` field, then layer-reference check, then layer-count fallback) with a 6-poll / 300ms debounce so page transitions don't read as "closed". The encyclopedia itself is a layer on the map screen, not a separate screen, so `ScreenManager.TopScreen` is always the map.

## For modders

`EditableEncyclopediaAPI` is a static class. Reference `EditableEncyclopedia.dll` (with `<Private>False</Private>`), declare the `EditableEncyclopedia` dependency in your `SubModule.xml`, and `using EditableEncyclopedia;`. Check the gate first:

```csharp
if (EditableEncyclopediaAPI.IsAvailable)
{
    string notes = EditableEncyclopediaAPI.GetHeroDescription(hero);
}
```

`IsAvailable` is true only when the mod is loaded and a campaign is active. Every method returns a safe default (null, 0, -1, or an empty collection) when it isn't, so a missing dependency won't throw.

Events:

- `OnDescriptionChanged` — `(objectId, newDescription)`, null description means removed
- `OnTagsChanged` — `(objectId, newTags)`
- `OnJournalChanged` — `(objectId)`
- `OnRelationNoteChanged` — `(heroId, targetHeroId, note)`

Read/write surface, grouped:

- **Descriptions** — `GetDescription` / `SetDescription` / `HasDescription`, typed getters (`GetHeroDescription`, `GetClanDescription`, `GetKingdomDescription`, `GetSettlementDescription`), bulk (`GetAllDescriptions`, the per-type `GetAll*Descriptions`), `ResetAllDescriptions`
- **Culture / occupation** — `GetHeroCulture`, `GetHeroOccupation`, `GetOccupationDisplayName`, `GetAllCustomCultures`, `GetAllCustomOccupations`
- **Hero lore fields** — `GetHeroInfoField` / `SetHeroInfoField`, `GetAllHeroLoreFields`, `HasHeroLore`, `GetInfoFieldKeys`
- **Lore templates** — `GetLoreTemplate`, `GetRoleTemplate`, `GetAllRoleTemplates`, `GetAvailableRoles`, `GetTemplateFieldKeys`
- **Tags** — `GetTags` / `SetTags`, query (`GetObjectsWithTag`, `GetObjectsWithAnyTag`, `GetObjectsWithAllTags`, usage counts), bulk ops (`RenameTagGlobal`, `RemoveTagGlobal`, `MergeTags`, `AddTagToMultiple`, `RemoveTagFromMultiple`, `ClearAllTags`), categories and presets
- **Journal / chronicle** — `GetJournalEntries`, `AddJournalEntry`, `GetHeroChronicle`, `GetAllChronicleEntries`
- **Relation notes** — `GetRelationNote` / `SetRelationNote` (main-hero overloads and arbitrary-pair overloads), `GetRelationHistory`, `GetRelationHistoryForHero`
- **Computed stats** (live game state, every key always present via a `SafeGet` fallback) — `GetHeroInfoStats`, `GetClanInfoStats`, `GetKingdomInfoStats`, `GetSettlementInfoStats`
- **Files** — `ExportToSharedFile`, `ImportFromSharedFile`, `ImportFromSharedFileDetailed` (per-section `ImportResult`), `GetSharedFilePath`, per-type export helpers

Custom names, titles, and banners are read off the `EncyclopediaEditBehavior.Instance` singleton, not the static API: `GetCustomName`, `HasCustomName`, `GetAllCustomNames`, `GetCustomBannerCode`, `HasCustomBanner`, `GetAllCustomBanners`. If you don't want a DLL reference, parse the export JSON directly via `SharedFileExporter.ImportAll()`.

Full method signatures, the JSON section formats, and the two integration approaches are in **INTEGRATION.md**.

## File map

| File                                            | ~Lines  | What it does                                                                                                                                                                                                                         |
| ----------------------------------------------- | ------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `EditableEncyclopediaPatches.cs`                | ~20,300 | The monolith. 27+ classes: Harmony page patches, key polling, page tracking, edit/reset/undo, name/culture/occupation/banner/tag editing, nav guard, save sanitizer, custom culture manager, hero portrait + banner emblem rendering |
| `EncyclopediaEditBehavior.cs`                   | ~3,090  | Persistence. The SaveableField dictionaries, `SyncData`, the auto-journal event handlers (16+ event types), import/export helpers, the singleton                                                                                     |
| `EditableEncyclopediaAPI.cs`                    | ~995    | The public static cross-mod API: events, getters/setters, computed stats                                                                                                                                                             |
| `MCMSettings.cs`                                | ~1,560  | MCM v5 settings, the management dialogs, and `DebugLog()`                                                                                                                                                                            |
| `GlobalChroniclePanel.cs`                       | ~3,829  | The campaign-map history overlay                                                                                                                                                                                                     |
| `JournalSectionInjector.cs`                     | ~2,727  | The collapsible journal section                                                                                                                                                                                                      |
| `TagWidgetInjector.cs`                          | ~1,835  | The tag display row, with reflection caching                                                                                                                                                                                         |
| `Timeline/HeroTimelineSectionInjector.cs`       | ~1,739  | The hero timeline section                                                                                                                                                                                                            |
| `LoreSectionInjector.cs`                        | ~1,806  | The narrative lore section                                                                                                                                                                                                           |
| `RelationNotes/RelationNotesSectionInjector.cs` | ~1,224  | The relation-notes section                                                                                                                                                                                                           |
| `Localization.cs`                               | ~1,531  | 12-language text system                                                                                                                                                                                                              |
| `SharedFileExporter.cs`                         | ~530    | Hand-rolled JSON v11 export/import (no external library)                                                                                                                                                                             |
| `SubModuleClassEntry.cs`                        | ~228    | Startup chain                                                                                                                                                                                                                        |

Total is roughly 49,600 lines of C# across 28 source files. The `.csproj` sets `EnableDefaultCompileItems=false`, so every `.cs` file is listed in explicit `<Compile>` items — add a new file there or it silently won't compile.

## Build

Open `MY Mod.sln` in Visual Studio 2017+ and build, or run `dotnet build EditableEncyclopedia.csproj` (it targets .NET Framework 4.7.2 and builds fine that way). MSBuild compiles the sources and the post-build step copies the DLLs, the Gauntlet GUI prefab XMLs, and the LoreStory templates into the Bannerlord Modules folder.

Game references and the copy target use `$(GameFolder)`, hardcoded in the `.csproj` to `H:\steam\steamapps\common\Mount & Blade II Bannerlord`. Change it if your install is elsewhere. Game DLLs are referenced from the Steam install via `HintPath`.

One gotcha after rebuilding: the launcher caches mod DLLs. Quitting only the game isn't enough — kill `BannerlordLauncher.exe` too, or it keeps serving the old code.

## Other docs

- **QUICKSTART.md** — install and a first-edit walkthrough, in a couple of minutes
- **CODEBASE.md** — the deeper architecture: file map, runtime flows, the injector pattern, the threading model
- **INTEGRATION.md** — the full cross-mod API reference and the JSON export format
- **CHANGELOG.md** — version history
