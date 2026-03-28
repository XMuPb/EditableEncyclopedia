<div align="center">
<img src="https://i.imgur.com/M7iApFw.png" alt="Editable Encyclopedia Overview">
<!-- Replace with an actual banner image when available -->
<!-- <img src="assets/banner.png" alt="Editable Encyclopedia Banner" width="800"> -->

# Editable Encyclopedia

**Custom lore & descriptions for Mount & Blade II: Bannerlord**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Game](https://img.shields.io/badge/Mount%20%26%20Blade%20II-Bannerlord-blue)](https://www.taleworlds.com/en/Games/Bannerlord)
[![Discord](https://img.shields.io/discord/1234567890?color=7289da&label=Discord&logo=discord&logoColor=white)](https://discord.com/users/404393620897136640)

</div>

A **Mount & Blade II: Bannerlord** mod that allows you to edit encyclopedia page descriptions for Heroes, Clans, Kingdoms, and Settlements. Create your own lore, write custom character backgrounds, or document your campaign story directly in the encyclopedia!

---

## Table of Contents

- [Quick Start (QUICKSTART.md)](QUICKSTART.md)
- [Features](#features)
- [Requirements](#requirements)
- [Installation](#installation)
- [Usage](#usage)
  - [Lore Story Templates](#lore-story-templates)
- [Configuration (MCM Settings)](#configuration-mcm-settings)
- [Technical Details (API)](#technical-details-for-developers)
- [Troubleshooting](#troubleshooting)
- [Discord & Support](#discord--support)
- [License](#license)
- [Credits](#credits)
- [NavalDLC Compatibility](#navaldlc-compatibility)
- [Changelog](#changelog)

---

## Features

### Core Editing
- **Edit Encyclopedia Descriptions** — Press `Ctrl+E` on any encyclopedia page to open a text editor and write your own custom description
- **Edit Hero Names/Titles (Ctrl+N)** — Change any hero's display name and title directly from the encyclopedia using the same EditPopupInjector as Ctrl+E (with portrait display, character counter, and proper layout). Also works on Clans, Kingdoms, Settlements, and Concepts. Features name validation (max length configurable via MCM, default 100), control character filtering, character count enforcement shown in popup, and original name display in popup description (leave empty to reset). Custom button text: Hero name shows "Next (Edit Title)", others show "Done". Custom tip text: "Enter a new name below. Max 100 characters. Leave empty to reset to native name." Preview shows "Native Name: Skorin" / "Native Title: Lord". Falls back to native TextInquiry if custom popup fails
- **Edit Banners/Flags (Ctrl+B)** — Paste a banner code to change the banner for Heroes, Clans, Kingdoms, or Settlements
- **Edit Hero Culture (Ctrl+U)** — Change any hero's culture using a clickable carousel — no typing needed. Supports creating custom cultures with troop tree assignments
- **Edit Hero Occupation (Ctrl+O)** — Change any hero's occupation with friendly display names (e.g., "Gang Leader" instead of "GangLeader")
- **Reset to Default (Ctrl+R)** — Restore the original game description for any page (with confirmation dialog)
- **Undo Last Edit (Ctrl+Z)** — Instantly revert your most recent edit or reset (one level of undo)

### Hero Lore System
- **Hero Lore Fields (Ctrl+E on Heroes)** — Press `Ctrl+E` on a hero page to open a field picker with 7 editable fields: Description, Backstory, Personality, Goals, Relationships, Rumors, and Chronicle. The 5 narrative fields (Backstory, Personality, Goals, Relationships, Rumors) are displayed in a dedicated "Lore" section; Description and Chronicle are handled separately
- **Lore Story Templates** — Built-in writing prompts for 5 character roles (Lord, Merchant, Wanderer, Gang Leader, Preacher) with structured sections for Backstory, Goals, Personality, Relationships, and Rumors — helping you craft rich character lore with consistent structure
- **Custom Lore Editor** — A dedicated Gauntlet popup editor with multiline support, character counter, and proper layout (falls back to native dialog gracefully)
- **Lore Section Widget Injection** — Hero lore fields are injected as a dedicated section in the encyclopedia page widget tree with left-aligned text and proper styling

### Chronicle & Journal
- **Chronicle with Auto-Dating** — The Chronicle field auto-prepends the in-game date to each new entry, creating a running journal for any character
- **Auto-Chronicle System** — Automatically logs 16+ game event types (battles, sieges, deaths, captures, diplomacy, marriages, births, tournaments, etc.) as dated chronicle notes with colored category tags: `[War]` (red), `[Politics]` (blue), `[Crime]` (orange), `[Family]` (green). Anti-spam deduplication prevents duplicate entries within the same game day
- **Journal Section** — Collapsible journal section on encyclopedia pages with pagination (10 entries per page with Prev/Next), category filter toggles, and colored hero/settlement names (golden for heroes, cyan for settlements)
- **Manual Chronicle Notes (Ctrl+J)** — Add, delete, or clear chronicle notes manually on any encyclopedia page with auto-dated in-game timestamps
- **Global Chronicle Panel** — A campaign map overlay that displays all world history events. A "Chronicle Notes" button on the campaign map HUD opens a full panel with aggregated chronicle notes from all kingdoms, clans, settlements, and heroes. Features animated slide-up panel, pagination, category filters (War/Politics/Crime/Family/Other), source-type filters (Kingdom/Clan/Settlement/Hero), colored clickable entity names, and Escape/click-outside dismissal

### Relation Notes & Timeline
- **Hero Relation Notes (Ctrl+F)** — Write personal notes about hero relationships. Press `Ctrl+F` on a hero page to search for another hero and write a note about their relationship. Also works via Ctrl+Click on hero portrait links in Friends/Enemies sections
- **Relation Notes Section** — A collapsible "Relation Notes" section injected into hero encyclopedia pages showing all heroes with notes for the currently viewed hero, with clickable hero names, edit-on-click, delete button, relation score display, and persistent collapse state
- **Hero Timeline Section** — A collapsible "Timeline" section on hero pages, always visible even with 0 events. Shows the hero's personal biography — only events where they personally participated (battles, sieges, raids, captures, kills, family events, clan changes, tournaments). When empty, displays "No events recorded yet. Events appear as this hero participates in battles, sieges, captures, and other activities." World-level events are excluded (those appear in Chronicle). `IsTimelineWorthy()` now includes ALL [War] and [Politics] entries from the hero's own journal. Timeline data comes from 3 sources: hero's own journal, cross-referenced journals, native game log
- **Relation History Tracking** — Automatic tracking of relation score changes between heroes with dated history entries

### Tags
- **Tag System (Ctrl+G)** — Add player-defined tags (e.g., "ally", "enemy", "target") to any encyclopedia entry — heroes, clans, kingdoms, settlements. Tags display on the page with page-type-aware placement and are included in exports
- **Auto-Tags** — Automatically generate tags for heroes based on game state. Auto-tags include "Auto: Friend" / "Auto: Enemy" (relation-based), "Auto: Dangerous" (large party), "Auto: Prisoner" (currently captured). Auto-tags are displayed with dimmed styling (75% opacity, 85% font scale) to distinguish from manual tags. Configurable thresholds via MCM settings
- **Tag Font Scale** — Configurable tag display size (50%–300%) via MCM settings

### Data Management
- **Export / Import** — Export all custom data (descriptions, names, titles, banners, cultures, occupations, lore fields, tags, journal entries, relation notes, tag notes, relation note tags) to a JSON file (v11 format, 19 data sections) and import into other campaigns via MCM
- **Export by Type** — Export only Heroes, Clans, Kingdoms, Settlements, or Banners separately
- **Auto-Export on Save** — Optionally keep the JSON export file always up-to-date for other mods
- **Auto-Import on Load** — Optionally import data from the JSON file when loading a save
- **Reset All Descriptions** — Clear all custom descriptions at once from MCM (with confirmation)

### UI & Display
- **Edit Timestamp** — Optionally show the in-game date when a description was last edited
- **Visual Indicator** — Optionally show `[Edited]` prefix on customized pages for easy identification
- **Description Statistics** — MCM popup showing breakdown of all custom edits: descriptions, names, titles, banners, cultures, occupations, lore fields, tags, journal entries
- **Navigation Guard** — Encyclopedia navigation is blocked while an edit popup is open, preventing accidental data loss
- **Input Blocker** — A transparent layer absorbs input while edit popups are open, with force-encyclopedia-open protection

### Persistence & Storage
- **Persistent Storage** — All custom data is saved in your campaign save file via 18 SaveableField dictionaries
- **Multiple Page Types Supported:**
  - Heroes — descriptions, lore fields (7 fields), names, titles, banners, cultures, occupations, tags, journal, relation notes, timeline
  - Clans — descriptions, names, banners, tags, journal
  - Kingdoms — descriptions, names, banners, tags, journal
  - Settlements — descriptions, names, banners, tags, journal
  - Concepts — names

### Configuration & Localization
- **Localization** — Full multi-language support with 12 built-in languages (English, Turkish, German, French, Spanish, Chinese, Russian, Portuguese, Korean, Japanese, Polish, Ukrainian)
- **Flexible MCM Settings** — 71 configuration options via the MCM v5 settings menu organized in 8 groups (14 Enable toggles, 8 Show toggles, 19 Action buttons, and more)
- **Character Limit** — Configurable max character limits for descriptions, narrative fields, and stats fields (default 5000 each)
- **Debug Log File** — Debug output written to a log file with automatic 5MB rotation, with optional on-screen display

### NavalDLC / War Sail Compatibility
- **Full Naval DLC Support** — All features work seamlessly with the Naval DLC (War Sail mode), which replaces the encyclopedia initialization
- **Auto-Fix Missing Page Types** — Automatically detects and restores missing encyclopedia page handlers (Clan, ListPage, Home) removed by NavalDLC
- **Chronicle Panel Navigation** — Navigate from the Chronicle Panel directly to any Clan/Hero/Settlement page, even on NavalDLC where encyclopedia page registration is different
- **Safe Error Recovery** — Harmony finalizers on `SetEncyclopediaPage`, `ExecuteLink`, and `OnTick` prevent crashes from missing page types

### Developer Features
- **Cross-Mod API** — Other mods can read cultures, occupations, descriptions, lore fields, tags, banners, journal entries, relation notes, and subscribe to changes
- **Dynamic Occupation Discovery** — Automatically discovers all occupation types at runtime, including those added by other mods
- **Prevent Kingdom Color Overwrites** — Re-applies your custom banners when the game forces them back to default colors

---

## Requirements

| Dependency | Required? | Notes |
|---|---|---|
| **Mount & Blade II: Bannerlord** | Yes | Tested on v1.3.13 and Newer Versions |
| **Harmony** | Yes | Usually bundled with Bannerlord |
| **ButterLib** | Yes | Required dependency |
| **UIExtenderEx** | Yes | Required for settlement nameplate refresh |
| **[MCM v5](https://www.nexusmods.com/mountandblade2bannerlord/mods/612)** (Mod Configuration Menu) | Yes | Required dependency — provides the full settings panel with 71 configuration options |

---

## Installation

### Manual Installation

1. Download the [latest release](https://github.com/XMuPb/EditableEncyclopedia/releases)
2. Extract the `EditableEncyclopedia` folder
3. Copy it to your Bannerlord modules folder:
   ```
   \Steam\steamapps\common\Mount & Blade II Bannerlord\Modules\
   ```
4. Launch the Bannerlord launcher
5. Enable **"Editable Encyclopedia"** in the mods list
6. Make sure it loads **after** Harmony, ButterLib, UIExtenderEx, and MCM (if installed)
7. Start or load a **Campaign / Sandbox**

---

## Usage

### Keyboard Shortcuts

| Shortcut | Action | Page Types |
|---|---|---|
| **Ctrl+E** | Edit description (heroes show a field picker: Description, Backstory, Personality, Goals, Relationships, Rumors, Chronicle) | Heroes, Clans, Kingdoms, Settlements |
| **Ctrl+N** | Edit name/title | Heroes, Clans, Kingdoms, Settlements, Concepts |
| **Ctrl+B** | Edit banner/flag code | Heroes, Clans, Kingdoms, Settlements |
| **Ctrl+U** | Change culture | Heroes |
| **Ctrl+O** | Change occupation | Heroes |
| **Ctrl+G** | Add/remove tags | All |
| **Ctrl+J** | Add a chronicle note (with auto-dated in-game timestamp) | All |
| **Ctrl+F** | Add/edit hero relation note | Heroes |
| **Ctrl+R** | Reset to default description | All |
| **Ctrl+Z** | Undo last change | All |

### Editing Descriptions

1. Open the Encyclopedia in-game (default key: `N`)
2. Navigate to any **Hero**, **Clan**, **Kingdom**, or **Settlement** page
3. Press `Ctrl+E` to open the edit dialog
4. For heroes, a field picker appears — choose Description, Backstory, Personality, Goals, Relationships, Rumors, or Chronicle
5. Type your custom text
6. Click **Save** to apply, or **Cancel** to discard changes

### Hero Relation Notes

1. Navigate to a Hero encyclopedia page
2. Press `Ctrl+F` — a search dialog appears
3. Type the name of another hero to search for
4. Write your note about the relationship
5. Notes appear in the "Relation Notes" section on the hero page

### Global Chronicle Panel

1. Look for the "Chronicle Notes" button on the bottom-right of the campaign map HUD
2. Click it to open the panel showing all world history events
3. Use category filters (War/Politics/Crime/Family) and source filters (Kingdom/Clan/Settlement/Hero) to narrow results
4. Navigate pages with Prev/Next buttons
5. Press Escape or click outside to close

### Lore Story Templates

The mod includes built-in writing prompts for **5 character roles** to help you craft rich, structured lore:

| Role | Description |
|---|---|
| **Lord** | Nobility — lineage, first battles, vassals, court rivalries, political ambitions |
| **Merchant** | Trade — specialty goods, trade routes, business rivals, shady deals |
| **Wanderer** | Adventurer — why they left home, places traveled, skills acquired, mysterious past |
| **Gang Leader** | Underworld — street origins, territory, enforcers, protection rackets |
| **Preacher** | Faith — spiritual mission, followers, conflicts with authority, heretical beliefs |

Each role provides structured prompts across all 5 narrative fields:

- **Backstory** — Origins, birthplace (`{settlement}`), culture (`{culture}`), key life events
- **Goals** — Short-term objectives, long-term ambitions, threats to counter
- **Personality** — Temperament, leadership style, fatal flaw, inner doubt
- **Relationships** — Allies, rivals, family, enemies, love interests
- **Rumors** — Gossip, legends, scandals, secrets

Use these as inspiration when editing hero lore fields via `Ctrl+E` — fill in the prompts to create consistent, immersive character backgrounds.

### Resetting to Default

1. Navigate to an encyclopedia page that has a custom description
   - The hint will show `[Ctrl+E to Edit | Ctrl+R to Reset]`
2. Press `Ctrl+R`
3. Confirm the reset in the dialog
4. The original game description is restored immediately

### Undo Last Change

1. After editing or resetting a description, the hint will show `Ctrl+Z to Undo`
2. Press `Ctrl+Z` to revert your most recent change
3. Works for both edits and resets (one level of undo)

### Export & Import

#### Export

1. Open MCM settings (**Options -> Mod Options**)
2. Navigate to **"Editable Encyclopedia" -> "4. Export"**
3. Click the **Export** button
4. Your descriptions are saved to:
   ```
   Documents\Mount and Blade II Bannerlord\Configs\ModSettings\Global\EditableEncyclopedia\descriptions_export.json
   ```

#### Import

1. Place a valid `descriptions_export.json` file in the location above
2. Open MCM settings
3. Navigate to **"Editable Encyclopedia" -> "5. Import"**
4. Click the **Import** button
5. All imported descriptions are merged with your current descriptions (existing entries are overwritten)

---

## Configuration (MCM Settings)

If you have MCM v5 installed, you can configure the mod via **Options -> Mod Options -> Editable Encyclopedia**.

### 1. Info/About

| Setting | Description |
|---|---|
| **Author** | Displays the mod author (XMuPb) |
| **Version** | Current mod version (v2.0.0) |
| **Join Discord** | Opens the Discord invite link in your browser |
| **Encyclopedia Edit Statistics** | Shows a popup with total descriptions, breakdown by type, names, titles, banners, cultures, occupations, lore fields, tags |
| **Open Config Folder** | Opens the folder containing MCM settings and export files |

### 2. General

#### Hints

| Setting | Default | Description |
|---|---|---|
| **Show Edit Hint** | `true` | Append `[Ctrl+E to Edit Description]` to encyclopedia pages |
| **Show Name Edit Hint** | `true` | Show `[Ctrl+N to Edit Name]` hint on encyclopedia pages |
| **Show Banner Edit Hint** | `true` | Show `[Ctrl+B to Edit Banner]` hint on encyclopedia pages |
| **Show Culture Edit Hint** | `true` | Show `[Ctrl+U to Edit Culture]` hint on hero pages |
| **Show Occupation Edit Hint** | `true` | Show `[Ctrl+O to Edit Occupation]` hint on hero pages |
| **Show Tag Edit Hint** | `true` | Show `[Ctrl+G to Edit Tags]` hint on encyclopedia pages |
| **Show Journal Hint** | `true` | Show `[J Journal]` in the keyboard shortcut hint |
| **Show Relation Note Hint** | `true` | Show `[F Friend Note]` in the keyboard shortcut hint on hero pages |

#### Display

| Setting | Default | Description |
|---|---|---|
| **Show Confirmation Messages** | `true` | Display green success messages when saving descriptions |
| **Show Edited Indicator** | `false` | Prepend `[Edited]` to customized descriptions for easy identification |
| **Show Edit Timestamp** | `true` | Display the in-game date when a description was last edited (e.g., "Edited: Day 15 of Spring, 1084") |

### 3. Editing

#### Pages

| Setting | Default | Description |
|---|---|---|
| **Enable Hero Editing** | `true` | Allow editing descriptions on Hero encyclopedia pages |
| **Enable Clan Editing** | `true` | Allow editing descriptions on Clan encyclopedia pages |
| **Enable Kingdom Editing** | `true` | Allow editing descriptions on Kingdom/Faction pages |
| **Enable Settlement Editing** | `true` | Allow editing descriptions on Settlement pages |

#### Features

| Setting | Default | Description |
|---|---|---|
| **Enable Name Editing (Ctrl+N)** | `true` | Allow editing names on encyclopedia pages using Ctrl+N |
| **Enable Banner/Flag Editing** | `true` | Allow editing banners/flags using `Ctrl+B` |
| **Enable Culture Editing** | `true` | Allow changing a hero's culture using `Ctrl+U` |
| **Enable Occupation Editing** | `true` | Allow changing a hero's occupation using `Ctrl+O` |
| **Enable Tag Editing** | `true` | Allow adding player-defined tags to any encyclopedia entry using `Ctrl+G` |
| **Tag Font Scale (%)** | `130` | Scales the font size of the tag display (50–300%). 100 = normal, 130 = default |
| **Enable Journal** | `true` | Allow adding chronicle notes to any encyclopedia entry using `Ctrl+J` |
| **Enable Auto-Chronicle** | `true` | Automatically log game events as chronicle notes |
| **Enable Global Chronicle Panel** | `true` | Show a "Chronicle Notes" button on the campaign map |
| **Enable Relation Notes** | `true` | Allow writing personal notes about hero relationships using `Ctrl+F` |
| **Enable Auto-Tags** | `true` | Automatically generate tags for heroes based on game state (relation, party size, prisoner status). Auto-tags appear alongside manual tags with dimmed styling |
| **Auto-Tag Enemy Relation** | `-30` | Heroes with relation at or below this threshold get an "Auto: Enemy" tag |
| **Auto-Tag Friend Relation** | `30` | Heroes with relation at or above this threshold get an "Auto: Friend" tag |
| **Auto-Tag Dangerous Party Size** | `200` | Heroes leading parties with at least this many troops get an "Auto: Dangerous" tag |
| **Enable Lore Section** | `true` | Show the collapsible 'Lore' section on hero pages (Backstory, Personality, Goals, Relationships, Rumors) |
| **Enable Info Stats** | `true` | Show the Info stats panel on encyclopedia pages (Hero/Clan/Kingdom/Settlement) |
| **Show Edit Popup Portrait** | `true` | Display hero face portrait or clan/kingdom banner emblem in the edit popup title |
| **Journal Page Size** | `10` | Number of journal/chronicle entries per page on encyclopedia pages (5–50) |
| **Chronicle Panel Page Size** | `22` | Number of entries per page in the Global Chronicle Panel (5–50) |
| **Prevent Kingdom Color Overwrites** | `true` | Re-applies custom banners when the game forces them back to default |
| **Create Custom Banner** | Button | Opens bannerlord.party/banner in your browser |

#### Management

| Setting | Description |
|---|---|
| **Manage Custom Cultures** | View and delete custom culture assignments. Restores heroes to original culture |
| **Manage Custom Occupations** | View and delete custom occupation assignments. Restores heroes to original occupation |
| **Manage Tags** | View, rename, and delete tags across all entries |

### 4. Export

| Setting | Description |
|---|---|
| **Auto-Export on Save** | Automatically export all data to the shared JSON file every time a save occurs |
| **Export All to JSON** | Exports all custom data to a shareable JSON file |
| **Export Heroes Only** | Exports only Hero descriptions |
| **Export Clans Only** | Exports only Clan descriptions |
| **Export Kingdoms Only** | Exports only Kingdom descriptions |
| **Export Settlements Only** | Exports only Settlement descriptions |
| **Export Banners Only** | Exports only custom banner codes |

### 5. Import

| Setting | Description |
|---|---|
| **Auto-Import on Load** | Automatically import all data from the shared JSON file when a save is loaded |
| **Import All from JSON** | Imports all data from a JSON file and merges into the current campaign |
| **Import Banners Only** | Imports only custom banner codes from the JSON file |

### 6. Reset

| Setting | Description |
|---|---|
| **Reset All Descriptions** | Removes ALL custom descriptions from the current campaign (with confirmation) |

### 7. Advanced

| Setting | Default | Description |
|---|---|---|
| **Initial Key Poll Delay (ms)** | `500` | Delay before hotkeys start working on a page. Prevents accidental triggers. |
| **Key Poll Interval (ms)** | `50` | How often the mod checks for hotkey presses. Lower = more responsive. |
| **Max Description Length** | `5000` | Maximum characters for the main Description field. 0 = unlimited. |
| **Max Narrative Field Length** | `5000` | Maximum characters for narrative fields (Backstory, Rumors/Secrets). 0 = unlimited. |
| **Max Stats Field Length** | `5000` | Maximum characters for short stats fields (Personality, Goals, Relationships). 0 = unlimited. |
| **Max Name Length** | `100` | Maximum characters allowed when editing names/titles via Ctrl+N (range 10–200). |
| **Language** | `en` | Language for UI text. 12 options: en, tr, de, fr, es, zh, ru, pt, ko, ja, pl, uk. Requires restart. |

### 8. Debug

| Setting | Default | Description |
|---|---|---|
| **Debug Mode** | `false` | Enable verbose logging to a debug.log file in the EditableEncyclopedia config folder (with 5MB auto-rotation). |
| **Show Debug On Screen** | `false` | When Debug Mode is enabled, also display debug messages as yellow in-game text. |

---

## Technical Details (For Developers)

### API Usage

Other mods can integrate with Editable Encyclopedia using the public API:

```csharp
// Check if Editable Encyclopedia is loaded
if (EditableEncyclopediaAPI.IsAvailable)
{
    // ── Descriptions ──────────────────────────────────────────
    string notes = EditableEncyclopediaAPI.GetHeroDescription(hero);
    string clanNotes = EditableEncyclopediaAPI.GetClanDescription(clan);
    string kingdomNotes = EditableEncyclopediaAPI.GetKingdomDescription(kingdom);
    string settlementNotes = EditableEncyclopediaAPI.GetSettlementDescription(settlement);

    // Bulk retrieval
    Dictionary<string, string> allNotes = EditableEncyclopediaAPI.GetAllDescriptions();
    var heroNotes = EditableEncyclopediaAPI.GetAllHeroDescriptions();
    var clanDescs = EditableEncyclopediaAPI.GetAllClanDescriptions();
    var kingdomDescs = EditableEncyclopediaAPI.GetAllKingdomDescriptions();
    var settlementDescs = EditableEncyclopediaAPI.GetAllSettlementDescriptions();

    // ── Custom Names, Titles & Banners (via Behavior Instance) ─
    var behavior = EncyclopediaEditBehavior.Instance;
    string customName = behavior.GetCustomName(hero.StringId);
    bool hasName = behavior.HasCustomName(hero.StringId);
    string bannerCode = behavior.GetCustomBannerCode(clan.StringId);
    bool hasBanner = behavior.HasCustomBanner(clan.StringId);

    // Bulk export (used by SharedFileExporter)
    var allNames = behavior.GetAllCustomNames();
    var allBanners = behavior.GetAllCustomBanners();

    // ── Info Stats (computed from live game state) ─────────────
    var heroStats = EditableEncyclopediaAPI.GetHeroInfoStats(hero);
    // Returns: Culture, Occupation, Kingdom, Location, Status, Spouse,
    //          Troops, Morale, Companions, Caravans, Towns, Castles,
    //          Garrisons, Workshops, Kills, Battles, Tournaments,
    //          Hall Rank, Influence

    var clanStats = EditableEncyclopediaAPI.GetClanInfoStats(clan);
    // Returns: Kingdom, Leader, Culture, Renown, Influence, Troops,
    //          Parties, Lords, Companions, Towns, Castles, Villages

    var kingdomStats = EditableEncyclopediaAPI.GetKingdomInfoStats(kingdom);
    // Returns: Ruler, Culture, Clans, Lords, Towns, Castles, Villages,
    //          Total Troops, Total Garrisons, Active Wars, At War With

    var settlementStats = EditableEncyclopediaAPI.GetSettlementInfoStats(settlement);
    // Returns: Owner, Clan, Kingdom, Culture, Prosperity, Loyalty,
    //          Security, Food, Garrison, Militia, Wall Level,
    //          Workshops, Governor, Bound Villages, Notables

    // ── Hero Lore (all fields at once) ───────────────────────
    var allLore = EditableEncyclopediaAPI.GetAllHeroLoreFields(hero.StringId);
    // Returns: backstory, personality, goals, relationships, rumors, chronicle
    bool hasLore = EditableEncyclopediaAPI.HasHeroLore(hero.StringId);

    // ── Lore Story Templates (Character Roles) ──────────────
    string[] roles = EditableEncyclopediaAPI.GetAvailableRoles();
    // Returns: ["Lord", "Merchant", "Wanderer", "GangLeader", "Preacher"]

    // Get resolved template for a specific hero (placeholders filled in)
    string backstoryTemplate = EditableEncyclopediaAPI.GetLoreTemplate("backstory", hero.StringId);
    // Returns: "Born in: Epicrotea\nNoble House: Argoros\nCulture: Empire\n..."

    // Get raw template for a role (with placeholders)
    string lordBackstory = EditableEncyclopediaAPI.GetRoleTemplate("Lord", "backstory");
    // Returns: "Born in: {settlement}\nNoble House: {clan}\nCulture: {culture}\n..."

    // Get ALL templates for a role
    var lordTemplates = EditableEncyclopediaAPI.GetAllRoleTemplates("Lord");
    // Returns: { "backstory": "...", "personality": "...", "goals": "...",
    //            "relationships": "...", "rumors": "..." }

    string[] templateFields = EditableEncyclopediaAPI.GetTemplateFieldKeys();
    // Returns: ["backstory", "personality", "goals", "relationships", "rumors"]

    // ── Chronicle (world history) ────────────────────────────
    string heroChronicle = EditableEncyclopediaAPI.GetHeroChronicle(hero.StringId);
    var allChronicle = EditableEncyclopediaAPI.GetAllChronicleEntries();
    // Returns flat list of all world events with EntityId, Date, Text

    // ── Hero Lore Fields (v2.0.0+) ───────────────────────────
    // Fields: "backstory", "personality", "goals", "relationships", "rumors", "chronicle"
    string backstory = EditableEncyclopediaAPI.GetHeroInfoField("backstory", hero.StringId);
    string personality = EditableEncyclopediaAPI.GetHeroInfoField("personality", hero.StringId);
    string goals = EditableEncyclopediaAPI.GetHeroInfoField("goals", hero.StringId);
    string relationships = EditableEncyclopediaAPI.GetHeroInfoField("relationships", hero.StringId);
    string rumors = EditableEncyclopediaAPI.GetHeroInfoField("rumors", hero.StringId);
    string chronicle = EditableEncyclopediaAPI.GetHeroInfoField("chronicle", hero.StringId);
    var allLoreFields = EditableEncyclopediaAPI.GetAllHeroInfoFieldsForExport();

    // ── Culture & Occupation ──────────────────────────────────
    string culture = EditableEncyclopediaAPI.GetHeroCulture(hero.StringId);
    string occupation = EditableEncyclopediaAPI.GetHeroOccupation(hero.StringId);
    string displayName = EditableEncyclopediaAPI.GetOccupationDisplayName("GangLeader");
    var allCultures = EditableEncyclopediaAPI.GetAllCustomCultures();
    var allOccupations = EditableEncyclopediaAPI.GetAllCustomOccupations();

    // ── Tags (v2.0.0+) ───────────────────────────────────────
    string tags = EditableEncyclopediaAPI.GetTags(hero.StringId);

    // Advanced tag queries
    var allUniqueTags = EditableEncyclopediaAPI.GetAllUniqueTags();
    var allies = EditableEncyclopediaAPI.GetObjectsWithTag("ally");
    var anyMatch = EditableEncyclopediaAPI.GetObjectsWithAnyTag(new[] { "ally", "friend" });
    var allMatch = EditableEncyclopediaAPI.GetObjectsWithAllTags(new[] { "ally", "king" });
    var tagCounts = EditableEncyclopediaAPI.GetTagUsageCounts();
    int allyCount = EditableEncyclopediaAPI.GetTagUsageCount("ally");

    // Bulk tag operations
    EditableEncyclopediaAPI.RenameTagGlobal("oldTag", "newTag");
    EditableEncyclopediaAPI.RemoveTagGlobal("obsoleteTag");
    EditableEncyclopediaAPI.MergeTags("sourceTag", "targetTag");
    EditableEncyclopediaAPI.AddTagToMultiple(objectIds, "newTag");
    EditableEncyclopediaAPI.RemoveTagFromMultiple(objectIds, "oldTag");
    EditableEncyclopediaAPI.ClearAllTags();

    // ── Journal / Chronicle (v2.0.0+) ────────────────────────
    var journal = EditableEncyclopediaAPI.GetJournalEntries(hero.StringId);

    // ── Relation Notes & History (v2.0.0+) ────────────────────
    var history = EditableEncyclopediaAPI.GetRelationHistory();
    var heroHistory = EditableEncyclopediaAPI.GetRelationHistoryForHero(hero.StringId);

    // ── Import with detailed results ──────────────────────────
    ImportResult result = EditableEncyclopediaAPI.ImportFromSharedFileDetailed();
}
```

### Save Data

- Custom data is stored in your campaign save file via `CampaignBehaviorBase` with 18 `[SaveableField]` dictionaries (IDs 1-18)
- Each entry is keyed by the object's `StringId`
- Data survives save/load cycles automatically

| # | Field | Contents |
|---|---|---|
| 1 | `_customDescriptions` | Main descriptions for all entity types |
| 2 | `_customNames` | Custom display names and titles |
| 3 | `_editTimestamps` | In-game date when each entry was last edited |
| 4 | `_customBannerCodes` | Custom banner/flag codes |
| 5 | `_customCultures` | Custom hero culture assignments |
| 6 | `_customOccupations` | Custom hero occupation assignments |
| 7 | `_customCultureDefs` | Custom culture troop tree definitions |
| 8 | `_heroInfoFields` | Hero lore fields (backstory, personality, goals, relationships, rumors) |
| 9 | `_customTags` | Player-defined tags for all entity types |
| 10 | `_journalEntries` | Chronicle/journal entries (auto and manual) |
| 11 | `_relationNotes` | Hero-to-hero relationship notes |
| 12 | `_relationHistory` | Automatic relation score change tracking |
| 13 | `_tagNotes` | Per-tag annotations on entities |
| 14 | `_tagCategories` | Custom tag category groupings |
| 15 | `_tagPresets` | Saved tag preset configurations |
| 16 | `_perHeroAutoTagThresholds` | Per-hero auto-tag threshold overrides |
| 17 | `_relationNoteTags` | Tags assigned to relation notes |
| 18 | `_relationNoteTagLocks` | Lock state for relation note tags |

### Export Format (JSON v11)

The JSON export includes metadata and all custom data (19 sections):

```json
{
  "version": 11,
  "exportedAt": "2026-03-16T12:00:00.0000000Z",
  "descriptionCount": 2,
  "descriptions": {
    "lord_1_1": "Derthert is the aging king of Vlandia...",
    "settlement_town_V1": "Pravend is a coastal town..."
  },
  "nameCount": 1,
  "names": {
    "lord_1_1": "King Derthert the Bold"
  },
  "titleCount": 1,
  "titles": {
    "lord_1_1": "High King of Vlandia"
  },
  "bannerCount": 0,
  "banners": {},
  "cultureCount": 1,
  "cultures": {
    "main_hero": "nord|Viking Warrior"
  },
  "occupationCount": 1,
  "occupations": {
    "main_hero": "GangLeader"
  },
  "cultureDefCount": 1,
  "cultureDefs": {
    "nord": "nord_recruit,nord_footman,nord_warrior"
  },
  "heroInfoFieldCount": 1,
  "heroInfoFields": {
    "backstory_lord_1_1": "Born in the highlands..."
  },
  "tagCount": 1,
  "tags": {
    "lord_1_1": "ally, king"
  },
  "timestampCount": 1,
  "timestamps": {
    "lord_1_1": "Day 15 of Spring, 1084"
  },
  "journalCount": 1,
  "journal": {
    "lord_1_1": "Day 15 of Spring, 1084|[War] Defeated Ragnar near Varcheg (150 vs 200)\nDay 16 of Spring, 1084|[Politics] Became ruler of Vlandia"
  },
  "relationNoteCount": 1,
  "relationNotes": {
    "main_hero_lord_1_1": "Trusted ally and king"
  },
  "tagNoteCount": 1,
  "tagNotes": {
    "lord_1_1|ally": "Helped me in the siege of Pravend"
  },
  "relationHistoryCount": 1,
  "relationHistory": {
    "main_hero_lord_1_1": "Day 5 of Spring, 1085|+5|Helped in battle"
  },
  "tagCategoryCount": 1,
  "tagCategories": {
    "Diplomacy": "ally, enemy, neutral"
  },
  "tagPresetCount": 1,
  "tagPresets": {
    "War Leaders": "king, general, commander"
  },
  "perHeroAutoTagThresholdCount": 1,
  "perHeroAutoTagThresholds": {
    "lord_1_1": "-50|50"
  }
}
```

### How It Works

1. Uses **Harmony** to patch encyclopedia page `Refresh()` methods (postfix patches)
2. Polls for 10 hotkey combos (`Ctrl+E/N/B/U/O/G/J/F/R/Z`) via a background timer using Win32 `GetAsyncKeyState`
3. Displays native Bannerlord dialogs for editing (text inquiry) and custom Gauntlet popups where supported
4. All widget operations are deferred to the main game thread (Gauntlet is not thread-safe)
5. Navigation guard patches 10+ methods to prevent page changes during edits
6. Poller runs for the duration of the campaign and only acts when an encyclopedia object is tracked

---

## Troubleshooting

<details>
<summary><strong>The Ctrl+E prompt doesn't appear</strong></summary>

- Make sure **"Show Edit Hint"** is enabled in MCM settings
- Check that the page type is enabled (e.g., **"Enable Hero Editing"**)
- Try increasing the **"Initial Key Poll Delay"** if it happens immediately after opening a page

</details>

<details>
<summary><strong>Ctrl+E doesn't work</strong></summary>

- Ensure the encyclopedia page has fully loaded
- Try enabling **Debug Mode** to see detailed logging in the debug.log file
- Check that no other mod is interfering with the `E` key

</details>

<details>
<summary><strong>My descriptions disappeared after loading a save</strong></summary>

- Make sure the mod is enabled when you load the save
- Descriptions are tied to the `StringId` of objects — if the game changes an ID (rare), the description may not match

</details>

<details>
<summary><strong>Import/Export doesn't work</strong></summary>

- Ensure you have an active campaign loaded (not in main menu)
- Check that the export file exists in the correct location
- Verify the JSON format is valid if manually editing the file

</details>

<details>
<summary><strong>Global Chronicle Panel button doesn't appear</strong></summary>

- Ensure **"Enable Global Chronicle Panel"** is enabled in MCM settings (3. Editing/Features)
- The button injects into the campaign map HUD — it may take a few seconds after loading

</details>

<details>
<summary><strong>Encyclopedia crashes or freezes with Naval DLC</strong></summary>

- This mod includes full NavalDLC compatibility patches that are applied automatically
- If you still experience issues, enable **Debug Mode** and check the log for `SetEncyclopediaPage` or `ExecuteLink` errors
- The first time you navigate from the Chronicle Panel to a Clan page, the mod installs safety patches — a brief delay is normal
- Ensure you're using the latest version (v2.0.0+) which includes all NavalDLC fixes

</details>

<details>
<summary><strong>I see red error messages</strong></summary>

- Enable **Debug Mode** for detailed logging to the debug.log file
- Check the log at: `Documents\Mount and Blade II Bannerlord\Configs\ModSettings\Global\EditableEncyclopedia\Logs\debug.log`
- Report issues on the [Discord server](https://discord.com/users/404393620897136640)

</details>

---

## Discord & Support

Join the community Discord for support, suggestions, and discussion:

[![Discord](https://img.shields.io/badge/Join%20Discord-7289DA?style=for-the-badge&logo=discord&logoColor=white)](https://discord.com/users/404393620897136640)

---

## License

This mod is licensed under the [MIT License](https://opensource.org/licenses/MIT).

Copyright (c) 2024-2026 XMuPb

---

## Credits

| | |
|---|---|
| **Author** | [XMuPb](https://github.com/XMuPb) |
| **Harmony** | [Pardeike](https://github.com/pardeike/Harmony) |
| **MCM v5** | Mod Configuration Menu team |
| **ButterLib** | ButterLib Team |
| **UIExtenderEx** | UIExtenderEx Team |

---

### v2.0.0
## 📜 v2.0.0 — Major Update

This release took a long time to create and means a lot to finally share. It came with many challenges and issues, but I didn’t give up—I wanted to build something truly unique that hasn’t been done before. I spent countless nights working on this, often putting in around **9 hours each night** to bring this vision to life. Now I’m proud to share it with all of you. I hope you enjoy the mod, support the project, and continue sharing your ideas—your feedback truly helps shape its future.

This update transforms the encyclopedia in **Mount & Blade II: Bannerlord** into a living, fully interactive chronicle of your campaign. At the heart of this release is the **Hero Lore System**, enhanced with structured **Lore Story Templates** that allow you to create rich backstories, personalities, goals, and relationships for every character. Combined with powerful in-game editing tools, you now have full control over descriptions, names, banners, cultures, occupations, tags, and more—directly from the encyclopedia using intuitive hotkeys.

The new **Auto-Chronicle & Journal system** automatically records major world events such as battles, sieges, diplomacy, marriages, births, deaths, and tournaments, all with in-game timestamps and categorized entries. You can also add your own notes manually, building a complete and evolving narrative unique to your campaign. Expanding on this, the **Global Chronicle Panel** provides a full overview of world history through a campaign map interface, while **Relation Notes** and **Hero Timelines** allow you to track personal relationships and individual character journeys in detail.

With additional systems like **player-defined tags**, **export/import (JSON v11, 19 data sections)**, and **71 configurable MCM settings**, this update offers full flexibility and control. Backed by persistent storage, cross-mod API support, and extensive compatibility patches, your data remains stable, shareable, and fully integrated with other mods—delivering a deeper, more immersive RPG experience where your campaign becomes a fully documented legend shaped by your actions.

This update took a long time to create and means a lot to finally share. I faced many challenges and issues during development, but I didn’t give up—I wanted to build something truly unique that hasn’t been done before. I’ve spent countless nights working on this, often putting in around **9 hours each night** to bring this vision to life. Now I can finally share it with all of you. I truly hope you enjoy the mod and continue to support my work. Your feedback and new ideas are always appreciated and help shape the future of this project.

---

## v2.0.0 — Complete Feature & Change Log

### Edit Popup Overhaul
- **Native TextInquiry popup** — Replaced custom Gauntlet popup with the native Bannerlord TextInquiry dialog as primary editor, with automatic fallback chain
- **Hero face portrait** — Edit popups display the hero’s 3D face portrait in the title area using `CharacterImageTextureProvider` (ImageTypeCode=2) with gold-bordered frame
- **Clan/Kingdom banner emblem** — Edit popups display the actual banner sigil using `BannerTableauTextureProvider` (ImageTypeCode=3) with `BannerCodeText` property, falling back to colored blocks
- **Settlement owner portrait** — Settlement edit popups show the owning clan leader’s face portrait
- **Entity name in popup heading** — Clan, Kingdom, and Settlement edit popups display the entity name and type in the title (e.g., "Edit the story of Argoros")
- **Hero title in heading** — Hero edit popups include the hero’s title below the name (e.g., "Vassal of the Khuzaits")
- **Scrollable preview area** — Long description text is displayed in a 350px scrollable preview panel with dark tinted background
- **Native scrollbar** — Vertical scrollbar with `SPOptions.CollapserLine` track brush and `SPOptions.Slider.Handle` handle brush
- **Divider line** — Gold-tinted divider line between title area and content using `Tooltip.Frame` brush
- **Editing tips** — Hint text above the input field: "Tip: The preview above shows your current text. Edit in the field below, then press Done to save."
- **Dark tinted input area** — Edit text field has a semi-transparent dark background for readability
- **Portrait state isolation** — Banner state properly reset between popups to prevent clan banners leaking into hero edit dialogs
- **EditPopupInjector enhanced with 3 new optional parameters** — `description` (separate preview text from input text), `confirmText` (custom confirm button text, default: "Done"), `tipText` (custom tip text overriding default edit_tips). `PendingCustomTipText` field on `EncyclopediaEditPopup` for deferred tip injection
- **Ctrl+N name/title popups now use EditPopupInjector** — Name and title popups use the same edit description popup as Ctrl+E, with portrait display, character counter, and proper layout. Falls back to native TextInquiry if custom popup fails. Custom button text: Hero name shows "Next (Edit Title)", others show "Done". Custom tip: "Enter a new name below. Max 100 characters. Leave empty to reset to native name." Preview shows "Native Name: Skorin" / "Native Title: Lord"
- **`ShowNativeTextInquiry` changed to `internal`** — Changed from `private` to `internal` for cross-class fallback access

### QUICKSTART.md
- **New quick-start guide** — `QUICKSTART.md` created for new users with installation instructions, first edit walkthrough, all 10 keyboard shortcuts, feature overview, configuration summary, troubleshooting tips, and links to the full README for comprehensive documentation

### New Localization Keys
- **`edit_tips_name`** — "Enter a new name below. Max {0} characters. Leave empty to reset to native name."
- **`edit_tips_title`** — "Enter a new title below. Max {0} characters. Leave empty to reset to native title."
- **`timeline_no_events`** — Empty timeline message: "No events recorded yet. Events appear as this hero participates in battles, sieges, captures, and other activities."

### Info Stats Panels
- **Hero Info panel** — Comprehensive stats section with 21+ fields: Culture, Occupation, Kingdom, Location (reflection-based position lookup, v1.3.13 Position2 compatible), Status (Active/Wounded/Prisoner/Fugitive/Dead), Spouse, At War With, Influence, Troops, Mercenaries (non-culture troops counted), Morale (with High/Steady/Low label), Lords (in clan), Companions (in party / in clan), Caravans, Towns, Castles, Garrisons (total garrison troops), Workshops (running only), Alleys, Supporters, Kills (Heroes + Troops via `[slain:N]` tracking), Battles (W/L with capture counts), Tournaments, Hall Rank
- **Clan Info panel** — Stats injected via native `ClanInfo` VM property (`StringPairItemVM` list reflection via `TryGetNativeStatsAdder`): Kingdom, Type (Minor Faction), Leader, Culture, Renown, Influence, Troops, Parties, Lords, Companions, Towns, Castles, Villages, Caravans, Workshops, At War With. Duplicate Clan Tier and Clan Strength removed to avoid native duplication
- **Kingdom Info panel** — Comprehensive stats via native VM list or widget injection fallback: Ruler, Culture, Clans, Lords, Towns, Castles, Villages, Total Fiefs, Strength (field troops), Total Garrisons, Influence, Mercenaries (minor faction count), Active Wars (count + names)
- **Settlement Info panel** — Stats via native VM list or widget injection fallback. Towns/Castles: Owner, Clan, Kingdom, Culture, Prosperity, Loyalty, Security, Food, Garrison, Militia, Wall Level, Workshops, Governor, Bound Villages, Notables, Daily Tax. Villages: Hearths, Bound To, Militia, Notables
- **Auto-discovery of stats lists** — `TryGetNativeStatsAdder()` scans ViewModel properties for `StringPairItemVM` collections, trying named properties (`ClanInfo`, `FactionInfo`, `Stats`, `LeftSideProperties`, `RightSideProperties`) with automatic fallback to `StatsWidgetInjector` widget-based injection
- **Widget injection fallback** — When native VM stats list is not found (Kingdom/Settlement on some game versions), stats are injected as Gauntlet widgets directly into the encyclopedia page widget tree with native `Encyclopedia.Stat.DefinitionText` (golden label) and `Encyclopedia.Stat.ValueText` brushes
- **Native brush styling** — Stat labels use `Encyclopedia.Stat.DefinitionText` brush, values use `Encyclopedia.Stat.ValueText` brush, matching the game's native appearance with colon separators
- **Vertical spacing** — Proper spacing between stat rows with colon separators matching native ClanInfo style
- **Troop kill tracking** — Battle log entries track troop kills with `[slain:N]` metadata hidden from display, accumulated in the Kills stat
- **Hall Rank** — Heroes ranked by composite score (troops + gold/1000 + influence/10) with tier labels: Legendary (top 5%), Elite (5-15%), Renowned (15-35%), Notable (35-60%), Common (60%+)
- **Empty stat handling** — Stats with no data show "Progress the Game" placeholder instead of being hidden
- **Info Stats API** — All stats exposed via `EditableEncyclopediaAPI.GetHeroInfoStats()`, `GetClanInfoStats()`, `GetKingdomInfoStats()`, `GetSettlementInfoStats()` returning `Dictionary<string, string>`

### Auto-Chronicle & Journal System
- **16+ tracked event types** — Battles (with troop counts and location), sieges, village raids, hero captures, prisoner releases (ransom/escape/set free), hero deaths (killer and cause), war declarations, peace treaties, clan defections, settlement ownership changes, clan destruction, tournament victories, births, marriages, marriage proposals, pregnancies
- **War/peace logging to both Kingdom and Clan** — War and peace events are logged to both the kingdom’s and the clan’s chronicle entries
- **Existing war tracking from save load** — Daily tick scan detects wars that existed before the mod was installed and logs them retroactively
- **Category tags with colors** — `[War]` (red), `[Politics]` (blue), `[Crime]` (orange), `[Family]` (green) using separate colored widgets
- **Anti-spam deduplication** — Prevents duplicate entries within the same in-game day
- **Battle location** — Shows nearest settlement instead of generic "the field"
- **Troop counts** — Battle entries include force sizes (e.g., "150 vs 200")
- **Release types** — Prisoner release entries specify ransom, escape, or set free
- **Capture counts** — Battle entries track how many enemies were captured
- **Rescuer names** — Release timeline entries show which hero performed the rescue

### Journal Section UI
- **Collapsible section** — Native-styled collapsible header with expand/collapse indicator arrow
- **Pagination** — 10 entries per page with Previous/Next navigation
- **Category filter toggles** — Filter by War, Politics, Crime, Family, Other
- **Colored entity names** — Hero names in gold, settlement names in cyan
- **Clickable entity names** — Hero and settlement names are clickable encyclopedia links via `RichTextWidget` EventFire hook
- **Chronicle Notes sub-header** — Journal merged with Chronicle under a single section with Chronicle Notes as sub-header
- **Auto-dated entries** — Each entry auto-prepends the in-game date

### Global Chronicle Panel
- **Campaign map overlay** — "Chronicle Notes" button injected into the campaign map HUD bottom-right bar
- **Animated slide-up panel** — Smooth animation when opening/closing
- **22 entries per page** with pagination
- **Category filters** — War, Politics, Crime, Family, Other toggle buttons
- **Source-type filters** — Kingdom, Clan, Settlement, Hero toggle buttons
- **Colored clickable entity names** — Golden hero names, cyan settlement names with encyclopedia link navigation
- **3-second recollection debounce** — Prevents excessive data recollection
- **Escape/click-outside dismissal** — Close panel by pressing Escape or clicking outside
- **NavalDLC navigation support** — Chronicle Panel → Clan/Hero/Settlement navigation works on NavalDLC

### Hero Lore System
- **7 editable fields** — Description, Backstory, Personality, Goals, Relationships, Rumors, Chronicle
- **Field picker** — Ctrl+E on heroes shows MultiSelectionInquiry with all 7 fields
- **Template vs Manual choice** — Empty fields offer "Use Template" or "Write Manually" options
- **Sequential template dialogs** — Template prompts presented one at a time for structured lore creation (parses `label:value` pairs into individual native TextInquiry dialogs)
- **Template placeholder resolution** — `{name}`, `{culture}`, `{settlement}`, `{clan}`, `{faction}`, `{occupation}`, `{date}` tokens resolved to hero's actual values
- **Template lookup priority** — Occupation-specific → Culture-specific → Default (via `Localization.L()` key resolution)
- **Dedicated Lore section** — Narrative fields injected as a collapsible "Lore" section in hero encyclopedia pages
- **Left-aligned text** — Harmony patch on `TextWidget.OnRender()` forces left alignment for lore text
- **Manual word wrap** — Text wrapped at ~90 characters per line for proper display
- **Lore section clearing** — Reset-to-default properly clears all lore fields
- **Lore API** — `GetAllHeroLoreFields()`, `HasHeroLore()`, `GetLoreTemplate()`, `GetRoleTemplate()`, `GetAllRoleTemplates()`, `GetAvailableRoles()`, `GetTemplateFieldKeys()`

#### Lore Story Templates — Character Roles

Each character role has 5 template sections (Backstory, Goals, Personality, Relationships, Rumors) with structured prompts:

**Lord** (Nobility)
- Backstory: Born in `{settlement}`, Noble House `{clan}`, Culture `{culture}`, Lineage, Early Training, First Battle, Current Standing
- Goals: Military Objectives, Political Ambitions, Dynastic Plans, Threats to Counter
- Personality: Temperament, Leadership Style, Code of Honor, Fatal Flaw
- Relationships: Liege, Vassals, Rivals at Court, Marriage Alliances, Sworn Enemies
- Rumors: Court Gossip, Battlefield Legends, Scandals, Secret Alliances

**Merchant** (Trade)
- Backstory: Born in `{settlement}`, Culture `{culture}`, Trade Origins, Specialty Goods, Trade Routes, Business Rivals, Current Ventures
- Goals: Trade Expansion, Wealth Target, New Markets, Competitors to Outmaneuver
- Personality: Negotiation Style, Risk Tolerance, Business Ethics, Private Indulgence
- Relationships: Business Partners, Key Clients, Trade Rivals, Family, Informants
- Rumors: Shady Deals, Hidden Wealth, Smuggling Whispers, Bankruptcy Rumors

**Wanderer** (Adventure)
- Backstory: Born in `{settlement}`, Culture `{culture}`, Why They Left Home, Places Traveled, Skills Acquired, What They Seek
- Goals: Current Quest, Long-term Dream, Skills to Master, Place to Settle
- Personality: Outlook on Life, Survival Instinct, Trust Issues, Driving Passion
- Relationships: Traveling Companions, Old Friends, People Owed Debts, Enemies from the Past, Love Interest
- Rumors: Mysterious Past, Strange Abilities, Bounty on Head, Forbidden Knowledge

**Gang Leader** (Underworld)
- Backstory: Born in `{settlement}`, Culture `{culture}`, Street Origins, Rise to Power, Territory, Enforcement Methods, Current Operations
- Goals: Territory Expansion, Rival Gangs to Crush, Smuggling Routes, Protection Rackets
- Personality: Demeanor, Rule by Fear or Loyalty, Personal Code, Hidden Vulnerability
- Relationships: Lieutenants, Enforcers, Rival Gang Leaders, Corrupt Officials, Family
- Rumors: Bodies Buried, Betrayed Allies, Secret Identity, Hidden Stash

**Preacher** (Faith)
- Backstory: Born in `{settlement}`, Culture `{culture}`, Faith Origins, Teachings, Followers, Conflicts with Authority
- Goals: Spiritual Mission, Convert the Faithless, Build a Following, Enemies of the Faith
- Personality: Piety, Conviction, Tolerance of Others, Inner Doubt
- Relationships: Flock, Fellow Clergy, Patrons, Persecutors, Lost Souls
- Rumors: Heretical Beliefs, Miracle Claims, Forbidden Texts, Past Sins

### Relation Notes & Timeline
- **Ctrl+F relation notes** — Search for another hero and write a relationship note
- **Relation Notes section** — Collapsible section on hero pages showing all noted relationships
- **Clickable hero names** — Click to navigate to the related hero’s page
- **Edit-on-click** — Click a relation note to edit it
- **Delete button** — Remove individual relation notes
- **Relation score display** — Shows current relation value alongside notes
- **Hero Timeline section** — Personal biography events (battles, captures, sieges, raids, kills, family events, clan changes, tournaments). Always visible on hero pages, even with 0 events. Shows "No events recorded yet. Events appear as this hero participates in battles, sieges, captures, and other activities." when empty
- **`IsTimelineWorthy()` expanded** — Now includes ALL [War] and [Politics] entries from hero's own journal (previously some were filtered out). Timeline data comes from 3 sources: hero's own journal, cross-referenced journals, native game log
- **Persistent collapse state** — Timeline collapse/expand state saved between page loads
- **Rescuer attribution** — Release events show which hero performed the rescue
- **Duplicate prevention** — No duplicate capture/release entries in Timeline
- **Relation History tracking** — Automatic tracking of relation score changes with dated entries

### Tag System
- **Quick Tag Menu (Ctrl+G)** — Native MultiSelectionInquiry for fast tag add/remove
- **Auto-Tags** — "Auto: Friend", "Auto: Enemy", "Auto: Dangerous", "Auto: Prisoner" based on game state
- **Auto-tag dimmed styling** — 75% opacity, 85% font scale to distinguish from manual tags
- **Configurable thresholds** — MCM settings for enemy relation, friend relation, dangerous party size
- **Tag notes** — Per-tag annotations on entities (e.g., why you tagged someone as "ally")
- **Tag categories** — Group tags into custom categories (e.g., "Diplomacy: ally, enemy, neutral")
- **Tag presets** — Save/load tag preset configurations
- **Per-hero auto-tag thresholds** — Override global thresholds for individual heroes
- **Tag font scale** — Configurable 50%–300% (default 130%)
- **Multi-row wrapping** — Tags wrap to multiple rows when exceeding parent width
- **Page-type-aware placement** — Left panel for Kingdom/Clan, inline for Hero/Settlement
- **Tag management MCM** — View, rename, delete tags; filter by tag; manage presets and categories
- **Tag icons and colors** — Expanded color palette and sort priority for tag display

### Edit Popup Technical Details
- **Input blocker** — Transparent layer absorbs all input while edit popups are open
- **Navigation guard** — Harmony patches on 10+ navigation methods (GoToLink, ExecuteBack, ExecuteLink, etc.) to prevent page changes during edits
- **Force encyclopedia open** — Watchdog forces `IsEncyclopediaOpen=true` while blocking layer is active
- **Max character limits** — Configurable limits for descriptions (5000), narrative fields (5000), stats fields (5000)
- **EditableTextWidget MaxLength** — Both property and backing field set to enforce character limits

### Persistence & Export/Import
- **18 SaveableField dictionaries** — Up from 7 in v1.x: descriptions, names, timestamps, banners, cultures, occupations, culture definitions, hero info fields, tags, journal entries, relation notes, relation history, tag notes, tag categories, tag presets, per-hero auto-tag thresholds, relation note tags, relation note tag locks
- **JSON v11 export format** — 19 data sections exported via manual `StringBuilder` serialization (no external JSON library). File location: `Documents\Mount and Blade II Bannerlord\Configs\ModSettings\Global\EditableEncyclopedia\descriptions_export.json`
- **JSON v11 import** — `ImportAll()` uses state-machine parsing to extract key-value pairs from nested JSON objects. Reads v1 through v11 files — missing sections are simply skipped (backward compatible)
- **ExportData class** — Holds all 19 imported sections as `Dictionary<string, string>` properties: Descriptions, Names, Titles, Banners, Cultures, Occupations, CultureDefs, HeroInfoFields, Tags, Timestamps, Journal, RelationNotes, TagNotes, RelationHistory, TagCategories, TagPresets, PerHeroAutoTagThresholds, RelationNoteTags, RelationNoteTagLocks
- **Export overloads** — 3 `Export()` methods: basic (descriptions only), named entities (descriptions+names+titles+banners), and full v11 (all 19 sections)
- **Import methods** — `Import()` returns descriptions only, `ImportAll()` returns full `ExportData` object, `ImportFromSharedFileDetailed()` returns per-section import counts via `ImportResult`
- **Auto-Export on Save** — Keeps JSON file always up-to-date on every game save
- **Auto-Import on Load** — Automatically imports all data from JSON file when loading a save
- **Export by type** — MCM buttons to export only Heroes, Clans, Kingdoms, Settlements, or Banners separately
- **Import Banners Only** — MCM button to import only custom banner codes from JSON
- **JSON string escaping** — Full escape/unescape support including `\uXXXX` unicode sequences, newlines, tabs, quotes, backslashes
- **v1 fallback** — If no `"descriptions"` section found, falls back to legacy v1 flat key-value parsing

### NavalDLC Compatibility
- **Full Naval DLC support** — All features work with War Sail mode
- **Auto-fix missing page types** — Detects and restores Clan, ListPage, Home pages removed by NavalDLC
- **SetEncyclopediaPage finalizer** — Catches navigation exceptions
- **ExecuteLink finalizer** — Catches KeyNotFoundException from missing page types
- **OnTick safety patches** — Prefix + finalizer on EncyclopediaData.OnTick
- **ReleaseMovie null guard** — Prevents NullRef during cleanup
- **State tracking flags** — `_encyclopediaCorrupted` and `_encyclopediaHasPage` for error recovery


### Encyclopedia Layer Detection & Cross-Screen Support
- **`EncyclopediaDividerButtonWidget` guard** — All `FindEncyclopediaLayer` implementations across all 6 injectors (Lore, RelNotes, Timeline, Stats, Journal, Timestamp) now verify candidate layers contain `EncyclopediaDividerButtonWidget`, preventing injection into Clan/Party/Inventory/Fleet screen panels
- **Deferred retry for Clan/Party/Inventory screens** — When the encyclopedia is opened from a non-map screen, the game fires `Refresh()` before the encyclopedia layer exists. A deferred retry (200ms intervals, 10 max) waits for `layers > 1` then re-runs the full Postfix
- **Size-fallback retry** — When `FindEncyclopediaLayer` uses size fallback and insertion fails, clears the cached layer reference and retries instead of injecting into a wrong panel
- **Alt-layer scan guard** — All three hero injectors (Lore, RelNotes, Timeline) skip alt layers without `EncyclopediaDividerButtonWidget` during page transitions
- Fixed **Lore/RelNotes missing on late-loading hero pages** (notable characters like merchants, wanderers)
- Fixed **widget injection bleeding into Clan/Party/Inventory/Fleet screens**
- Fixed **encyclopedia opened from Clan/Party screen missing all widgets**
- Fixed **navigating between heroes on Clan screen** injecting into wrong panel
- Fixed **StatsWidget/JournalSection failing on Kingdom/Settlement pages** opened from Clan screen

### Role Templates — Default Lore Display
- **Role templates shown as default lore** — Heroes with occupation-specific templates (Lord, Merchant, Wanderer, Gang Leader, Preacher) now display resolved template text in the Lore section when no custom lore is saved
- Templates have placeholders auto-filled: `{name}`, `{settlement}`, `{culture}`, `{clan}`, `{occupation}`, `{date}`
- Occupation-specific → culture-specific → default template priority chain

### Build & Timer Fixes
- Fixed **CS1061: `Workshop[].Count`** — Changed to `.Length` (arrays don't have `.Count`)
- Fixed **3 build errors** in Info Stats API methods
- Fixed **EditableTextWidget "widget not found" spam** — `ReleasePopup()` now disposes char-limit and preview-constrain timers

---

# Hero Lore System

## Hero Lore Fields (Ctrl+E Field Picker)

Press **Ctrl+E** on a hero encyclopedia page to open the **Field Picker** with seven editable lore fields:

- Description
- Backstory
- Personality
- Goals
- Relationships
- Rumors
- Chronicle

Each field is stored independently and displayed in a dedicated **"Lore" section** on the hero page.

This allows players to build **rich character biographies and role-play histories** without overwriting other narrative information.

---

## Chronicle with Auto-Dating

The **Chronicle field** automatically prepends the **current in-game date** to each new entry, creating a chronological journal for every hero.

Example:


Spring 5, 1085 — Defeated a Sturgian raiding party near Varcheg.


---

## Custom Lore Editor Popup

A new **Gauntlet-based editor popup** (`EncyclopediaEditPopup.xml`) provides a modern interface for editing lore.

**Features**

- Multiline text input
- Character counter
- Proper UI layout
- Integrated encyclopedia styling

If the Gauntlet prefab fails to load, the system **gracefully falls back to the native Bannerlord text inquiry dialog** to maintain compatibility.

---

## Lore Section Widget Injection

Narrative lore fields are injected directly into the hero encyclopedia page using **Harmony patching**, with **left-aligned narrative text rendering**.

---

# Lore Story Templates

Built-in writing prompts for **five character roles** to help craft rich, structured lore.

| Role | Description |
|-----|-----|
| Lord | Nobility, lineage, first battles, vassals, court rivalries |
| Merchant | Trade routes, goods, markets, business rivals |
| Wanderer | Adventurer, travels, mysterious past |
| Gang Leader | Underworld power, territory, enforcers |
| Preacher | Faith, followers, spiritual mission |

Each role includes prompts across the main narrative fields:

- **Backstory** — Origins, birthplace, culture, key life events  
- **Goals** — Short-term objectives, long-term ambitions  
- **Personality** — Temperament, leadership style, fatal flaw  
- **Relationships** — Allies, rivals, family, enemies  
- **Rumors** — Gossip, scandals, secrets  

Use these prompts when editing hero lore with **Ctrl+E**.

---

# Auto-Chronicle & Journal System

## Auto-Chronicle System

The **Auto-Chronicle system** automatically logs major campaign events and converts them into dated chronicle entries.

### Tracked Events

- Battles *(troop counts, location, commanders)*
- Sieges
- Village raids
- Hero captures
- Prisoner releases *(ransom, escape, set free)*
- Hero deaths *(killer and cause)*
- War declarations
- Peace treaties
- Clan defections
- Settlement ownership changes
- Clan destruction
- Tournament victories
- Births
- Marriages
- Marriage proposals
- Pregnancies

### Event Categories

| Category | Color |
|--------|--------|
| War | Red |
| Politics | Blue |
| Crime | Orange |
| Family | Green |

An **anti-spam deduplication system** prevents duplicate entries within the same in-game day.

---

## Journal Section UI

Chronicle entries appear inside a **collapsible “Journal” section** on encyclopedia pages.

**Features**

- Golden header styling
- Pagination *(10 entries per page)*
- Previous / Next navigation
- Category filters
- Color-coded entity names

Hero names appear in **gold**.  
Settlement names appear in **cyan**.

---

## Manual Chronicle Notes (Ctrl+J)

Players can manually create chronicle entries.

Press **Ctrl+J** on any encyclopedia page to add a note.  
The **current in-game date** is automatically inserted.

Management options:

- Delete entries by number
- Clear all entries

---

# Global Chronicle Panel

## Campaign History Viewer

A new **Global Chronicle Panel** displays all world events recorded by the Auto-Chronicle system.

A **“Chronicle Notes” button** appears on the **campaign map HUD (bottom-right)**.

Clicking it opens an animated slide-up panel displaying historical events from:

- Kingdoms
- Clans
- Settlements
- Heroes

### Panel Features

- 22 entries per page
- Pagination
- Category filters
- Source-type filters
- Clickable entity names
- Hover effects
- Escape key dismissal
- Click-outside dismissal
- 3-second recollection debounce

---

# Relation Notes & Timeline

## Hero Relation Notes (Ctrl+F)

Press **Ctrl+F** on a hero page to search for another hero and create a **relationship note**.

Notes can also be created via **Ctrl-Click** on hero portrait links in **Friends** or **Enemies** sections.

Notes are stored in the `_relationNotes` dictionary.

---

## Hero Timeline Section

Each hero page includes a **Timeline** showing events where the hero personally participated.

Examples:

- Battles
- Captures
- Sieges
- Raids
- Kills
- Family events
- Clan membership changes
- Tournament victories

World-level events are excluded to keep timelines focused on **personal hero history**.

---

## Relation History Tracking

Relation score changes between heroes are automatically tracked using  
`TryRegisterRelationChangeEvent()` and stored in the `_relationHistory` dictionary.

---

# Tag System

## Tag System (Ctrl+G)

Players can assign **custom tags** to any encyclopedia entry.

Example tags:

- ally
- enemy
- target
- trade partner
- ally-kingdom

### Tag Placement

| Page Type | Placement |
|-----------|-----------|
| Kingdom / Clan | Left panel |
| Hero / Settlement | Inline |

---

## Auto-Tags

Heroes are automatically tagged based on game state. Auto-tags are evaluated daily and on first encyclopedia page access.

| Auto-Tag | Condition | MCM Setting |
|----------|-----------|-------------|
| **Auto: Friend** | Relation >= threshold (default: 30) | Auto-Tag Friend Relation |
| **Auto: Enemy** | Relation <= threshold (default: -30) | Auto-Tag Enemy Relation |
| **Auto: Dangerous** | Party size >= threshold (default: 200) | Auto-Tag Dangerous Party Size |
| **Auto: Prisoner** | Hero is currently a prisoner | — |

Auto-tags display with **dimmed styling** (75% opacity, 85% font scale) to distinguish from manual tags. They are computed at runtime and not saved to the campaign file.

---

## Tag Font Scale

New **MCM setting** allows adjusting tag size between:

**50% — 300%**
(Default: **130%**)

---

# Data & Persistence

## Expanded Save Data

Persistence expanded from **7 → 18 SaveableField dictionaries** (IDs 1-18).

New dictionaries include:

- `_heroInfoFields` (#8)
- `_customTags` (#9)
- `_journalEntries` (#10)
- `_relationNotes` (#11)
- `_relationHistory` (#12)
- `_tagNotes` (#13)
- `_tagCategories` (#14)
- `_tagPresets` (#15)
- `_perHeroAutoTagThresholds` (#16)
- `_relationNoteTags` (#17)
- `_relationNoteTagLocks` (#18)

---

## Export / Import v11

JSON export format upgraded to **Version 11** with 19 data sections.

All 19 exported data sections:

| # | Section | Key Format | Value Format | Since |
|---|---------|-----------|--------------|-------|
| 1 | `descriptions` | entity StringId | Description text | v1 |
| 2 | `names` | `name_`/`title_`/`origname_` + StringId | Name/title string | v2 |
| 3 | `titles` | entity StringId | Title text | v2 |
| 4 | `banners` | entity StringId | Serialized banner code (e.g., `11.40.40...`) | v2 |
| 5 | `cultures` | heroId | `"cultureId\|displayName"` | v3 |
| 6 | `occupations` | heroId | Occupation enum name (e.g., `"GangLeader"`) | v3 |
| 7 | `cultureDefs` | culture StringId | `"displayName\|baseCultureId\|basicTroopId\|eliteTroopId"` | v4 |
| 8 | `heroInfoFields` | `fieldKey_heroId` | Narrative text (backstory, personality, etc.) | v4 |
| 9 | `tags` | entity StringId | Comma-separated tag list (e.g., `"ally, king"`) | v5 |
| 10 | `timestamps` | entity StringId | In-game date string (e.g., `"Day 15 of Spring, 1084"`) | v6 |
| 11 | `journal` | entity StringId | Newline-separated `"date\|text"` entries | v7 |
| 12 | `relationNotes` | `"heroId_targetHeroId"` | Note text | v8 |
| 13 | `tagNotes` | `"objectId\|tagName"` | Per-tag annotation text | v9 |
| 14 | `relationHistory` | `"heroId_targetHeroId"` | Newline-separated `"date\|change\|description"` | v10 |
| 15 | `tagCategories` | Category name | Comma-separated tag list | v10 |
| 16 | `tagPresets` | Preset name | Comma-separated tag list | v10 |
| 17 | `perHeroAutoTagThresholds` | heroId | `"enemyThreshold\|friendThreshold"` | v10 |
| 18 | `relationNoteTags` | `"viewingHeroId_targetHeroId"` | Tags on relation notes | v11 |
| 19 | `relationNoteTagLocks` | `"viewingHeroId_targetHeroId"` | Lock state for relation note tags | v11 |

**Technical details:**
- Manual `StringBuilder` serialization — no external JSON library (Newtonsoft/System.Text.Json) dependency
- Full JSON string escaping: `\"`, `\\`, `\n`, `\r`, `\t`, `\uXXXX` unicode sequences
- Backward compatible: importer reads v1 through v11 files, missing sections skipped
- New export methods: `GetAllRelationNoteTagsForExport()`, `GetAllRelationNoteTagLocksForExport()`
- New import methods: `ImportRelationNoteTags()`, `ImportRelationNoteTagLocks()`
- v1 fallback: if no `"descriptions"` section found, falls back to legacy flat key-value parsing
- Each section includes a `{sectionName}Count` metadata field for validation
- File: `Documents\Mount and Blade II Bannerlord\Configs\ModSettings\Global\EditableEncyclopedia\descriptions_export.json`

---

## Custom Names for All Entity Types

The **Ctrl+N rename feature** now works for:

- Heroes
- Settlements
- Kingdoms
- Clans
- Concepts (NEW)

---

# MCM Settings Improvements

Settings are now organized into **8 groups**:

- Info / About
- General
- Editing
- Export
- Import
- Reset
- Advanced
- Debug

### New Settings

- Enable Global Chronicle Panel
- Enable Relation Notes
- Show Relation Note Hint
- Tag Font Scale (%)
- Max Narrative Field Length
- Max Stats Field Length
- Show Debug On Screen
- Debug Log File *(Logs/debug.log with 5MB auto-rotation)*

---

# Localization

Added **Ukrainian (uk)** as the **12th supported language**.

---

# Bug Fixes & Improvements

- Fixed **Ctrl+B crash** when clipboard contains non-numeric banner code text
- Fixed **missing timestamps** in JSON export/import
- Fixed **clan navigation freeze** caused by minor/bandit factions
- Fixed **chronicle sidebar text overflow and word wrapping**
- Fixed **non-clickable source labels rendering centered instead of left-aligned**
- Fixed **auto-export missing relation notes** — Auto-Export on Save now correctly includes relation notes in the JSON file
- Fixed **auto-import missing relation notes** — Auto-Import on Load now correctly imports relation notes from the JSON file
- Added **tag notes to export/import** — Tag notes (per-tag annotations on entities) are now included in JSON export/import
- Added **4 missing data sections to export/import** — Relation history, tag categories, tag presets, and per-hero auto-tag thresholds are now included in JSON export/import (format v11)
- Added **2 new data sections to export/import (v11)** — `relationNoteTags` and `relationNoteTagLocks` are now exported/imported. Previously these were saved in game saves (SaveableField 17 & 18) but never exported — data would be lost on JSON transfer between campaigns
- Added **MCM toggle for Relation Notes** — New `Enable Relation Notes` setting to enable/disable the Ctrl+F feature
- Added **MCM hint toggle for Relation Notes** — New `Show Relation Note Hint` setting to show/hide the "[F Friend Note]" hint on hero pages
- **JSON export format upgraded to v11** — Now includes all 19 data sections: descriptions, names, titles, banners, cultures, occupations, culture definitions, hero info fields, tags, timestamps, journal entries, relation notes, tag notes, relation history, tag categories, tag presets, per-hero auto-tag thresholds, relation note tags, and relation note tag locks
- **Persistence expanded to 13 SaveableField dictionaries** — Added `_tagNotes` (#13) for per-tag annotations
- **Auto-Tags system** — Automatically generate tags for heroes based on game state: "Auto: Friend" / "Auto: Enemy" (relation thresholds), "Auto: Dangerous" (large party size), "Auto: Prisoner" (captured heroes). Configurable thresholds via MCM. Auto-tags display with dimmed styling to distinguish from manual tags
- **MCM hint texts updated** — Export, Import, Auto-Export, Auto-Import, and Statistics hint texts now reference all 19 data sections (v11 format)
- **Fix: Hotkey combo state reset** — Fixed 5 missing combo state variable resets (`_nameComboWasDown`, `_bannerComboWasDown`, `_tagComboWasDown`, `_journalComboWasDown`, `_friendNoteComboWasDown`) in `KeyEventPoller.EnsureRunning()`. Could cause Ctrl+N/B/G/J/F to fire immediately on encyclopedia reopen
- **Edit popup portrait rendering** — Hero edit popups now display the hero's 3D face portrait in the title area using `CharacterImageTextureProvider` (ImageTypeCode=2). The portrait is rendered inside a gold-bordered frame with proper scaling
- **Edit popup banner rendering** — Clan and Kingdom edit popups now display the actual banner emblem in the title area using `BannerTableauTextureProvider` (ImageTypeCode=3) with `BannerCodeText` property. Falls back to colored blocks if banner rendering fails
- **Portrait state isolation** — Banner state (`_useBannerColors`, `_bannerClan`) is properly reset between popups to prevent clan banners from leaking into hero edit dialogs
- **Removed dead event handlers** — Removed unused OnMarriageOffered and OnChildConceived event registrations that were no-ops
- **Statistics popup now shows journal and relation note counts** — The MCM Statistics popup now includes journal entry count and relation note count alongside descriptions, names, tags, etc.

 ### Bug Fixes
- Fixed **Ctrl+B crash** when clipboard contains non-numeric banner code text
- Fixed **Ctrl+E ignoring EnableHeroEditing** MCM setting
- Fixed **missing timestamps** in JSON export/import
- Fixed **clan navigation freeze** caused by minor/bandit factions
- Fixed **chronicle sidebar text overflow** and word wrapping
- Fixed **non-clickable source labels** rendering centered instead of left-aligned
- Fixed **auto-export missing relation notes** and tag notes
- Fixed **auto-import missing relation notes** and tag notes
- Fixed **duplicate name** in release events (e.g., "Looters of Looters")
- Fixed **lore field text not wrapping** — uses manual word wrap at 90 chars
- Fixed **lore section not clearing** on reset-to-default
- Fixed **duplicate text** in native TextInquiry + description editor refresh
- Fixed **encyclopedia navigation** still active during editing
- Fixed **backspace navigation** during editing
- Fixed **tag row alignment** — indent rows 2+ to align after "Tags:" label
- Fixed **notes overflow** with word-wrapping using RichTextWidget
- Fixed **game freeze** when navigating to clans without encyclopedia pages
- Fixed **duplicate journal entries** in chronicle section
- Fixed **duplicate capture/release entries** in Timeline
- Fixed **battle location** showing "the field" instead of nearest settlement
- Fixed **word wrap applied to ALL lore text** instead of just long lines
- Fixed **portrait state leak** — banner state from Clan/Kingdom popups no longer leaks into hero edits
- Fixed **`[slain:N]` metadata** hidden from timeline and journal display
- Removed **dead event handlers** (unused OnMarriageOffered and OnChildConceived)

---

# NavalDLC Compatibility

## Full Naval DLC Support

The mod now fully supports the **Naval DLC** (War Sail mode), which replaces the game's encyclopedia initialization and removes several page types. All compatibility patches are applied automatically — no configuration needed.

### Fixes

- **Fixed encyclopedia crash (DefragContainers NullRef)** — Navigating from the Chronicle Panel to a Clan page no longer causes a `NullReferenceException` in `EventManager.DefragContainers()`. The crash was caused by orphaned widget containers left by the error recovery path
- **Fixed missing Clan page type** — NavalDLC removes the "Clan" key from the encyclopedia's `_pages` dictionary. The mod now auto-detects and restores it from `_lists` on the fly using a Harmony prefix on `SetEncyclopediaPage`
- **Fixed ListPage navigation** — Clicking "Clans" or other list breadcrumbs in the encyclopedia now works correctly. NavalDLC's `SetEncyclopediaPage` still contains the vanilla ListPage handling code but was failing because those keys weren't registered in `_pages`. The mod temporarily adds the key so the original method can handle it
- **Fixed Home page navigation** — The encyclopedia Home page now loads correctly on NavalDLC. Same auto-registration approach as ListPage
- **Fixed encyclopedia open via N key** — Pressing N to open the encyclopedia on NavalDLC now works. NavalDLC uses "ListPage" as the default open page type instead of "Home"; both are now handled
- **Fixed LastPage (back) navigation** — Back navigation is gracefully skipped when page history is unavailable
- **Added SetEncyclopediaPage finalizer** — A Harmony finalizer on `SetEncyclopediaPage` catches any exceptions from navigation page handling and suppresses them, preventing crashes while keeping the encyclopedia on the current page
- **Added ExecuteLink finalizer** — A Harmony finalizer on `GauntletMapEncyclopediaView.ExecuteLink` catches `KeyNotFoundException` from missing page types and suppresses them
- **Added OnTick safety patches** — `EncyclopediaData.OnTick` is patched with both a prefix (skip when corrupted) and a finalizer (suppress NullRef) to prevent cascading failures
- **Added ReleaseMovie null guard** — `GauntletLayer.ReleaseMovie` is patched to skip null movie identifiers, preventing NullRef during encyclopedia cleanup
- **State tracking flags** — Added `_encyclopediaCorrupted` and `_encyclopediaHasPage` flags to track encyclopedia state across error recovery and navigation. Flags are cleared on encyclopedia close
  
### Code Quality & Name Editing Overhaul, Code Quality (across entire codebase — 28 files):

- Replaced 200+ empty catch { } blocks with diagnostic logging via MCMSettings.DebugLog()
- Added null checks after 100+ reflection lookups (GetProperty/GetField/GetMethod) before calling GetValue/SetValue/Invoke
- Fixed unsafe type casts in EditableEncyclopediaAPI.cs — introduced SafeGet() helper replacing 50+ bare try-catch stat lines
- Added reflection caching in TagWidgetInjector.cs — 15+ lookups now cached with lazy initialization
- Fixed thread safety: added _transitionLock for page transitions in EditableEncyclopediaPatches.cs, _stateLock for GlobalChroniclePanel state management
- Added game object validity checks — 14 event handlers in EncyclopediaEditBehavior.cs now null-check event arguments
- Replaced string concatenation with StringBuilder in 10+ loops across 6+ files
- Extracted 100+ magic numbers to named constants (MCMSettings: 18, JournalSectionInjector: 27, LoreSectionInjector: 16, EncyclopediaEditBehavior: 16, GlobalChroniclePanel: 30+, TimestampWidgetInjector: 7, HeroTimelineSectionInjector: 5, RelationNotesInjector: 6, RelationNotesSectionInjector: 3, TimelineReflectionCache: 1, TimelineTextProcessor: 2)
- Added Substring bounds checks at 15+ locations across 5+ files
- Ctrl+N Name/Title Editing Overhaul:
  
- Added configurable max name length (MCM setting, default 100, range 10-200)
- Added character filtering: strips control characters, newlines, tabs with user warning
- Added character count enforcement via ScheduleCharLimitOverride
- Popup now shows "Original: " in description — leave empty to reset to original
- Improved hero name+title flow: "Next (Edit Title)" button, title popup shows saved name and original title
- Fixed shared _popupOpen flag bug — split into _namePopupOpen and _titlePopupOpen
- Title cancel now shows yellow message: "Title edit cancelled. Name change was saved."
- Deduplicated code: extracted ApplyHeroNameOnObject(), RefreshPageAndTimestamp(), ShowConfirmationIfEnabled(), RefreshSettlementNameplate() helpers
- Added Concept page type support for name editing (5 entity types total)
- Added SettlementNameplateMixin.ScheduleDeferredRefresh() with retry logic for reliable nameplate updates
- New Localization Keys:
  
- name_edit_title_concept — "Edit Concept Name"
- name_edit_desc_hero — Shows original name with reset instructions
- name_edit_desc_nonhero — Shows original name with reset instructions
- title_edit_desc — Shows original title and saved name
- name_edit_cancelled_title_kept — "Title edit cancelled. Name change was saved."
- name_invalid_chars — Warning when invalid characters are stripped

### How It Works

1. When `SetEncyclopediaPage` is called with a missing page type (e.g., "Clan"), the prefix intercepts it:
   - First tries to find the page handler in `_lists` and auto-adds it to `_pages`
   - For navigation pages (Home, ListPage), temporarily adds the key to `_pages` so the original method can handle it
   - For unknown page types, searches loaded assemblies for matching handler classes
   - If nothing works, skips the call (returns false) to prevent crashes
2. A finalizer catches any exceptions that slip through, cleans up temporary `_pages` entries, and suppresses the error
3. The Chronicle Panel → Clan navigation uses ExecuteLink with full error recovery: save state → close encyclopedia → restore state → retry with patches installed

---

# Full Game Compatibility

**It fully supports both game** `versions1.3, and works seamlessly whether you are playing with or without DLC` (including Naval DLC / War Sail mode). Every patch has been tested and refined to maintain performance and stability, so you can enjoy a consistent gameplay experience regardless of your setup.


### v1.1.4

- **Auto-Import now loads Cultures & Occupations from JSON** — Auto-Import on Load now imports all data sections (descriptions, names, titles, banners, cultures, occupations) matching the full manual import behavior
- **Culture Editing Fix** — Fixed a bug where selecting a new culture via Ctrl+U would not actually apply the change to the hero. Now uses CharacterObject-first reflection with full fallback chain across all 5 code paths
- **Occupation Editing Fix** — Applied the same dual-set reflection pattern to all 5 occupation code paths for full robustness across Bannerlord versions
- **Native Search in Carousel** — Culture and occupation selection carousels now have the game's built-in search enabled
- **Reflection Guard** — If culture/occupation reflection fails to set the value, the change is not saved to persistence (prevents ghost entries)
- **Reset Safety** — If the original culture/occupation no longer exists in the game, shows a warning instead of setting null
- **Display Name Validation** — Custom culture display names are trimmed and capped at 40 characters
- **Bulk Delete** — MCM "Delete Cultures" and "Delete Occupations" now offer "Delete All" with confirmation, in addition to one-by-one review
- **Original Value in Delete Carousel** — MCM delete carousels now show what the hero will be reset to (both culture and occupation)
- **Custom Filter Indicators** — Custom culture and occupation filters in the encyclopedia list view are marked with a star to distinguish from vanilla
- **Filter Performance** — Culture/occupation reference sets for filter detection are now cached (5s) instead of rebuilt every refresh
- **Smart Re-apply** — Culture and occupation re-apply on page refresh now checks current value before doing expensive reflection
- **Statistics Updated** — MCM Statistics popup now reflects all importable data types
- **JSON Integration** — Export/Import and Auto-Import/Auto-Export now handle all data sections consistently
- **Troop Assignment in Culture** — You can now assign Troops to a custom culture, linking them to that culture's roster
- **Custom Names for Settlements, Kingdoms & Clans** — Press `Ctrl+N` on any Settlement, Kingdom, or Clan encyclopedia page to set a custom display name
- **Settlement Hover Tooltip Culture** — Hovering over any settlement on the campaign map now shows a "Culture" line in the tooltip
- **Villages Included in Culture Updates** — When a clan leader's culture is changed, all their villages now also update
- **Fixed Custom Culture Name Setting** — Custom culture names are now correctly set by walking the full type hierarchy
- **Version Bump** — Updated to v1.1.4

### v1.1.3

- **Culture Editing (Ctrl+U)** — Press `Ctrl+U` on any hero page to change their culture using a clickable carousel. Click to select, "Next" to see more. Supports custom culture names (e.g., "Viking Warrior")
- **Occupation Editing (Ctrl+O)** — Press `Ctrl+O` on any hero page to change their occupation. Fully clickable — no typing required
- **Friendly Occupation Names** — Occupations display as readable names everywhere: "Gang Leader" instead of "GangLeader", "Rural Notable" instead of "RuralNotable", etc.
- **Dynamic Occupation Discovery** — Occupation list is built dynamically from the game's enum at runtime, automatically picking up new occupations from game updates or other mods
- **Manage Custom Cultures (MCM)** — New "Delete Cultures" button in MCM settings to view and remove custom culture assignments. Restores heroes to their original culture
- **Manage Custom Occupations (MCM)** — New "Delete Occupations" button in MCM settings to view and remove custom occupation assignments. Restores heroes to their original occupation
- **JSON Export v3** — Export file now includes `cultures` and `occupations` sections alongside descriptions, names, titles, and banners. Backward-compatible with v1/v2 imports
- **Culture/Occupation API** — New public API methods: `GetHeroCulture()`, `GetHeroOccupation()`, `GetOccupationDisplayName()`, `GetAllCustomCultures()`, `GetAllCustomOccupations()`
- **Occupation Name Localization** — All occupation names are translatable via localization keys. Turkish translations included
- **Fix: Duplicate custom filters** — Custom culture/occupation filter checkboxes no longer duplicate on encyclopedia list page refresh

### v1.1.2

- **Localization System** — Full multi-language support with 11 built-in languages (English, Turkish, German, French, Spanish, Chinese, Russian, Portuguese, Korean, Japanese, Polish). Language files auto-generated in Documents folder — users can add or customize translations
- **Language Selection** — New MCM setting in Advanced group to choose your language (requires restart)
- **Fix: Hotkeys firing when encyclopedia is closed** — Ctrl+E/R/Z no longer trigger on the campaign map after closing the encyclopedia
- **Fix: Hotkeys blocked on non-standard map screens** — Now works correctly with War Sail mode (NavalMapScreen) and other modded map screens
- **Fix: Encyclopedia close detection after edit/reset/undo** — Layer count tracking no longer corrupted by popup layers

### v1.1.1

- **Undo Last Edit (Ctrl+Z)** — Press `Ctrl+Z` on any encyclopedia page to instantly revert your most recent edit or reset (one level of undo)
- **Auto-Export on Save** — New MCM toggle in General group that automatically keeps the JSON export file up-to-date whenever you save or reset a description
- **Description Statistics** — New "Show Stats" button in MCM Info group displaying total descriptions, breakdown by type (Heroes/Clans/Kingdoms/Settlements), and total character count
- **Reset All Descriptions** — New button in MCM Sharing group to clear all custom descriptions at once (with confirmation dialog)
- **Visual Indicator** — New "Show Edited Indicator" toggle in MCM General group that prepends `[Edited]` to customized descriptions for easy identification
- **Export by Type** — New buttons in MCM Sharing group to export only Heroes, Clans, Kingdoms, or Settlements separately
- **Character Limit** — Max Description Length now defaults to 5000 (was unlimited); saves exceeding the limit are rejected with a warning; edit popup title shows current/max character count
- **Dynamic hint text** — Now shows `Ctrl+Z to Undo` when an undo action is available

### v1.1.0

- **Reset to Default (Ctrl+R)** — Press `Ctrl+R` on any encyclopedia page with a custom description to restore the original game text, with a yes/no confirmation dialog
- **Export / Import via MCM** — New "Sharing" settings group with Export and Import buttons to share descriptions as JSON files between campaigns or players
- **Improved JSON format** — Export now includes `version`, `exportedAt`, and `descriptionCount` metadata
- **Cross-mod Import API** — Added `EditableEncyclopediaAPI.ImportFromSharedFile()` for other mods
- **Dynamic hint text** — Shows `[Ctrl+E to Edit | Ctrl+R to Reset]` when a custom description exists, `[Ctrl+E to Edit Description]` otherwise
- **Bug fix: key poller dying after 2 seconds** — The encyclopedia hotkeys now remain active for the entire session instead of stopping after the first page load
- **Bug fix: page not updating visually after edit/reset** — The encyclopedia page text now updates immediately after saving or resetting

### v1.0.0

- Initial release
- Support for Hero, Clan, Kingdom, and Settlement pages
- Persistent storage in campaign save files
- Comprehensive MCM v5 settings
- Debug mode for troubleshooting
