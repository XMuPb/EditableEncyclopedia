<div align="center">
<img src="https://i.imgur.com/M7iApFw.png" alt="Editable Encyclopedia Overview">
<!-- Replace with an actual banner image when available -->
<!-- <img src="assets/banner.png" alt="Editable Encyclopedia Banner" width="800"> -->

# 📜 Editable Encyclopedia

**Custom lore & descriptions for Mount & Blade II: Bannerlord**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Game](https://img.shields.io/badge/Mount%20%26%20Blade%20II-Bannerlord-blue)](https://www.taleworlds.com/en/Games/Bannerlord)
[![Discord](https://img.shields.io/discord/1234567890?color=7289da&label=Discord&logo=discord&logoColor=white)](https://discord.com/users/404393620897136640)

</div>

A **Mount & Blade II: Bannerlord** mod that allows you to edit encyclopedia page descriptions for Heroes, Clans, Kingdoms, and Settlements. Create your own lore, write custom character backgrounds, or document your campaign story directly in the encyclopedia!

---

## 📑 Table of Contents

- [Features](#-features)
- [Requirements](#-requirements)
- [Installation](#-installation)
- [Usage](#-usage)
- [Configuration (MCM Settings)](#%EF%B8%8F-configuration-mcm-settings)
- [Technical Details (API)](#-technical-details-for-developers)
- [Troubleshooting](#-troubleshooting)
- [Discord & Support](#-discord--support)
- [License](#-license)
- [Credits](#-credits)
- [Changelog](#-changelog)

---

## ✨ Features

- **Edit Encyclopedia Descriptions** — Press `Ctrl+E` on any encyclopedia page to open a text editor and write your own custom description
- **Edit Hero Names/Titles (Ctrl+N)** — Change any hero's display name and title directly from the encyclopedia
- **Edit Banners/Flags (Ctrl+B)** — Paste a banner code to change the banner for Heroes, Clans, Kingdoms, or Settlements
- **Edit Hero Culture (Ctrl+U)** — Change any hero's culture using a clickable carousel — no typing needed
- **Edit Hero Occupation (Ctrl+O)** — Change any hero's occupation with friendly display names (e.g., "Gang Leader" instead of "GangLeader")
- **Reset to Default** — Press `Ctrl+R` to restore the original game description for any page (with confirmation dialog)
- **Undo Last Edit** — Press `Ctrl+Z` to instantly revert your most recent edit or reset
- **Persistent Storage** — All custom data is saved in your campaign save file
- **Multiple Page Types Supported:**
  - Heroes (characters) — descriptions, names, titles, banners, cultures, occupations
  - Clans — descriptions, banners
  - Kingdoms / Factions — descriptions, banners
  - Settlements (towns, castles, villages) — descriptions, banners
- **Manage Custom Cultures/Occupations** — View and delete custom assignments from MCM settings
- **Dynamic Occupation Discovery** — Automatically discovers all occupation types at runtime, including those added by other mods
- **Prevent Kingdom Color Overwrites** — Re-applies your custom banners when the game forces them back to default colors
- **Export / Import** — Export all custom data (descriptions, names, titles, banners, cultures, occupations) to a JSON file and import into other campaigns via MCM
- **Export by Type** — Export only Heroes, Clans, Kingdoms, Settlements, or Banners separately
- **Auto-Export on Save** — Optionally keep the JSON export file always up-to-date for other mods
- **Auto-Import on Load** — Optionally import data from the JSON file when loading a save
- **Reset All Descriptions** — Clear all custom descriptions at once from MCM (with confirmation)
- **Description Statistics** — View a breakdown of all custom edits: descriptions, names, titles, banners, cultures, occupations
- **Edit Timestamp** — Optionally show the in-game date when a description was last edited
- **Visual Indicator** — Optionally show `[Edited]` prefix on customized pages for easy identification
- **Character Limit** — Optionally enforce a maximum character limit on descriptions (default 5000)
- **Localization** — Full multi-language support with 11 built-in languages
- **Flexible MCM Settings** — Extensive configuration options via the MCM v5 settings menu
- **Cross-Mod API** — Other mods can read cultures, occupations, descriptions, banners, and subscribe to changes
- **Debug Mode** — Enable verbose logging for troubleshooting

---

## 📋 Requirements

| Dependency | Required? | Notes |
|---|---|---|
| **Mount & Blade II: Bannerlord** | ✅ Yes | Tested on v1.3.13 and Newer Versions |
| **Harmony** | ✅ Yes | Usually bundled with Bannerlord |
| **ButterLib** | ✅ Yes | Required dependency |
| **[MCM v5](https://www.nexusmods.com/mountandblade2bannerlord/mods/612)** (Mod Configuration Menu) | ⚡ Optional | Recommended — the mod works without MCM but you won't have access to the settings panel |

---

## 📥 Installation

### Manual Installation

1. Download the [latest release](https://github.com/XMuPb/EditableEncyclopedia/releases)
2. Extract the `EditableEncyclopedia` folder
3. Copy it to your Bannerlord modules folder:
   ```
   \Steam\steamapps\common\Mount & Blade II Bannerlord\Modules\
   ```
4. Launch the Bannerlord launcher
5. Enable **"Editable Encyclopedia"** in the mods list
6. Make sure it loads **after** Harmony, ButterLib, and MCM (if installed)
7. Start or load a **Campaign / Sandbox**

---

## 🎮 Usage

### Editing Descriptions

1. Open the Encyclopedia in-game (default key: `N`)
2. Navigate to any **Hero**, **Clan**, **Kingdom**, or **Settlement** page
3. Press `Ctrl+E` to open the edit dialog
4. Type your custom description
5. Click **Save** to apply, or **Cancel** to discard changes

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

1. Open MCM settings (**Options → Mod Options**)
2. Navigate to **"Editable Encyclopedia" → "Sharing"**
3. Click the **Export** button
4. Your descriptions are saved to:
   ```
   Documents\Mount and Blade II Bannerlord\Configs\ModSettings\Global\EditableEncyclopedia\descriptions_export.json
   ```

#### Import

1. Place a valid `descriptions_export.json` file in the location above
2. Open MCM settings
3. Navigate to **"Editable Encyclopedia" → "Sharing"**
4. Click the **Import** button
5. All imported descriptions are merged with your current descriptions (existing entries are overwritten)

---

## ⚙️ Configuration (MCM Settings)

If you have MCM v5 installed, you can configure the mod via **Options → Mod Options → Editable Encyclopedia**.

### Info Group

| Setting | Description |
|---|---|
| **Author** | Displays the mod author (XMuPb) |
| **Version** | Current mod version |
| **Join Discord** | Opens the Discord invite link in your browser |
| **Description Statistics** | Shows a popup with total descriptions, breakdown by type, and total character count |
| **Open Config Folder** | Opens the folder containing MCM settings and export files |

### General Group

| Setting | Default | Description |
|---|---|---|
| **Show Edit Hint** | `true` | Append `[Ctrl+E to Edit Description]` to encyclopedia pages |
| **Show Name Edit Hint** | `true` | Show `[Ctrl+N to Edit Name]` hint on hero pages |
| **Show Banner Edit Hint** | `true` | Show `[Ctrl+B to Edit Banner]` hint on encyclopedia pages |
| **Show Culture Edit Hint** | `true` | Show `[Ctrl+U to Edit Culture]` hint on hero pages |
| **Show Occupation Edit Hint** | `true` | Show `[Ctrl+O to Edit Occupation]` hint on hero pages |
| **Show Confirmation Messages** | `true` | Display green success messages when saving descriptions |
| **Show Edited Indicator** | `false` | Prepend `[Edited]` to customized descriptions for easy identification |
| **Show Edit Timestamp** | `true` | Display the in-game date when a description was last edited (e.g., "Edited: Day 15 of Spring, 1084") |
| **Auto-Export on Save** | `false` | Automatically export all data to the shared JSON file every time a save occurs |
| **Auto-Import on Load** | `false` | Automatically import all data from the shared JSON file when a save is loaded |

### Supported Pages Group

Enable/disable editing for specific encyclopedia page types:

| Setting | Default | Description |
|---|---|---|
| **Enable Hero Editing** | `true` | Allow editing descriptions on Hero encyclopedia pages |
| **Enable Hero Name/Title Editing** | `true` | Allow editing hero names and titles using `Ctrl+N` |
| **Enable Clan Editing** | `true` | Allow editing descriptions on Clan encyclopedia pages |
| **Enable Kingdom Editing** | `true` | Allow editing descriptions on Kingdom/Faction pages |
| **Enable Settlement Editing** | `true` | Allow editing descriptions on Settlement pages |
| **Enable Banner/Flag Editing** | `true` | Allow editing banners/flags using `Ctrl+B`. Paste a banner code to change the banner for Heroes, Clans, Kingdoms, or Settlements |
| **Enable Culture Editing** | `true` | Allow changing a hero's culture using `Ctrl+U` |
| **Enable Occupation Editing** | `true` | Allow changing a hero's occupation using `Ctrl+O` |
| **Prevent Kingdom Color Overwrites** | `true` | Periodically re-applies your custom banners when the game forces them back to default culture colors (common in 1.3.13) |
| **Create Custom Banner** | Button | Opens bannerlord.party/banner in your browser to design a banner and copy its code |
| **Manage Custom Cultures** | Button | View and delete custom culture assignments. Restores heroes to original culture |
| **Manage Custom Occupations** | Button | View and delete custom occupation assignments. Restores heroes to original occupation |

### Sharing Group

| Setting | Description |
|---|---|
| **Export All to JSON** | Exports all custom data (descriptions, names, titles, banners) to a shareable JSON file |
| **Import All from JSON** | Imports all data from a JSON file and merges into the current campaign |
| **Export Heroes Only** | Exports only Hero descriptions |
| **Export Clans Only** | Exports only Clan descriptions |
| **Export Kingdoms Only** | Exports only Kingdom descriptions |
| **Export Settlements Only** | Exports only Settlement descriptions |
| **Export Banners Only** | Exports only custom banner codes |
| **Import Banners Only** | Imports only custom banner codes from the JSON file |
| **Reset All Descriptions** | Removes ALL custom descriptions from the current campaign (with confirmation) |

### Advanced Group

| Setting | Default | Description |
|---|---|---|
| **Initial Key Poll Delay** | `500ms` | Delay before `Ctrl+E` starts working on a page. Prevents accidental triggers when rapidly navigating. |
| **Key Poll Interval** | `50ms` | How often the mod checks for `Ctrl+E`. Lower = more responsive, higher = less CPU usage. |
| **Max Description Length** | `5000` | Maximum characters allowed per description. Set to 0 for unlimited. Saves exceeding the limit are rejected with a warning. |

### Debug Group

| Setting | Default | Description |
|---|---|---|
| **Debug Mode** | `false` | Enable verbose logging. Displays yellow in-game messages for all mod events. Useful for troubleshooting. |

---

## 🔧 Technical Details (For Developers)

### API Usage

Other mods can integrate with Editable Encyclopedia using the public API:

```csharp
// Check if Editable Encyclopedia is loaded
if (EditableEncyclopediaAPI.IsAvailable)
{
    // Get a specific hero's custom description
    string notes = EditableEncyclopediaAPI.GetHeroDescription(hero);

    // Get clan/kingdom/settlement notes
    string clanNotes = EditableEncyclopediaAPI.GetClanDescription(clan);
    string kingdomNotes = EditableEncyclopediaAPI.GetKingdomDescription(kingdom);
    string settlementNotes = EditableEncyclopediaAPI.GetSettlementDescription(settlement);

    // Get ALL descriptions at once
    Dictionary<string, string> allNotes = EditableEncyclopediaAPI.GetAllDescriptions();

    // Get all heroes that have custom descriptions
    var heroNotes = EditableEncyclopediaAPI.GetAllHeroDescriptions();
}
```

### Save Data

- Custom descriptions are stored in your campaign save file via `CampaignBehaviorBase`
- Each description is keyed by the object's `StringId`
- Descriptions survive save/load cycles automatically

### Export Format

The JSON export includes metadata and all custom descriptions:

```json
{
  "version": 3,
  "exportedAt": "2026-02-22T12:00:00.0000000Z",
  "descriptionCount": 2,
  "descriptions": {
    "lord_1_1": "Derthert is the aging king of Vlandia...",
    "settlement_town_V1": "Pravend is a coastal town..."
  },
  "nameCount": 0,
  "names": {},
  "titleCount": 0,
  "titles": {},
  "bannerCount": 0,
  "banners": {},
  "cultureCount": 1,
  "cultures": {
    "main_hero": "nord|Viking Warrior"
  },
  "occupationCount": 1,
  "occupations": {
    "main_hero": "GangLeader"
  }
}
```

### How It Works

1. Uses **Harmony** to patch encyclopedia page `Refresh()` methods
2. Polls for `Ctrl+E` / `Ctrl+R` / `Ctrl+Z` / `Ctrl+N` / `Ctrl+U` / `Ctrl+O` keypresses via a background timer using Win32 `GetAsyncKeyState`
3. Displays native Bannerlord dialogs for editing (text inquiry) and resetting (yes/no confirmation)
4. Poller runs for the duration of the campaign and only acts when an encyclopedia object is tracked

---

## ❓ Troubleshooting

<details>
<summary><strong>The Ctrl+E prompt doesn't appear</strong></summary>

- Make sure **"Show Edit Hint"** is enabled in MCM settings
- Check that the page type is enabled (e.g., **"Enable Hero Editing"**)
- Try increasing the **"Initial Key Poll Delay"** if it happens immediately after opening a page

</details>

<details>
<summary><strong>Ctrl+E doesn't work</strong></summary>

- Ensure the encyclopedia page has fully loaded
- Try enabling **Debug Mode** to see detailed logging
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
<summary><strong>I see red error messages</strong></summary>

- Enable **Debug Mode** for more details
- Check the game's log files in `Documents\Mount and Blade II Bannerlord\`
- Report issues on the [Discord server](https://discord.com/users/404393620897136640)

</details>

---

## 💬 Discord & Support

Join the community Discord for support, suggestions, and discussion:

[![Discord](https://img.shields.io/badge/Join%20Discord-7289DA?style=for-the-badge&logo=discord&logoColor=white)](https://discord.com/users/404393620897136640)

---

## 📄 License

This mod is licensed under the [MIT License](https://opensource.org/licenses/MIT).

Copyright © 2024 XMuPb

---

## 🙏 Credits

| | |
|---|---|
| **Author** | [XMuPb](https://github.com/XMuPb) |
| **Harmony** | [Pardeike](https://github.com/pardeike/Harmony) |
| **MCM v5** | Mod Configuration Menu team |
| **ButterLib** | ButterLib Team |

---

## 📝 Changelog

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
