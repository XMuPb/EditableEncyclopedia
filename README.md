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
| **Version** | Current mod version (v2.3.0) |
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
- Ensure you're using the latest version (v2.3.0+) which includes all NavalDLC fixes

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

## Changelog

For the full version history, see [CHANGELOG.md](CHANGELOG.md).

