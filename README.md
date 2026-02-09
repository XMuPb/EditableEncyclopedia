<div align="center">
<img src="https://i.imgur.com/M7iApFw.png" alt="Editable Encyclopedia Overview">
<!-- Replace with an actual banner image when available -->
<!-- <img src="assets/banner.png" alt="Editable Encyclopedia Banner" width="800"> -->

# 📜 Editable Encyclopedia

**Custom lore & descriptions for Mount & Blade II: Bannerlord**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Game](https://img.shields.io/badge/Mount%20%26%20Blade%20II-Bannerlord-blue)](https://www.taleworlds.com/en/Games/Bannerlord)
[![Discord](https://img.shields.io/discord/1234567890?color=7289da&label=Discord&logo=discord&logoColor=white)](https://discord.gg/Zhnx9SuE6q)

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
- **Reset to Default** — Press `Ctrl+R` to restore the original game description for any page (with confirmation dialog)
- **Undo Last Edit** — Press `Ctrl+Z` to instantly revert your most recent edit or reset
- **Persistent Storage** — All custom descriptions are saved in your campaign save file
- **Multiple Page Types Supported:**
  - Heroes (characters)
  - Clans
  - Kingdoms / Factions
  - Settlements (towns, castles, villages)
- **Export / Import** — Export all your custom descriptions to a JSON file and import them into other campaigns via MCM
- **Export by Type** — Export only Heroes, Clans, Kingdoms, or Settlements separately
- **Auto-Export on Save** — Optionally keep the JSON export file always up-to-date for other mods
- **Reset All Descriptions** — Clear all custom descriptions at once from MCM (with confirmation)
- **Description Statistics** — View a breakdown of your custom descriptions by type and total character count
- **Visual Indicator** — Optionally show `[Edited]` prefix on customized pages for easy identification
- **Character Limit** — Optionally enforce a maximum character limit on descriptions (default 5000)
- **Flexible MCM Settings** — Extensive configuration options via the MCM v5 settings menu
- **Cross-Mod API** — Other mods can read and subscribe to description changes
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
| **Show Confirmation Messages** | `true` | Display green success messages when saving descriptions |
| **Show Edited Indicator** | `false` | Prepend `[Edited]` to customized descriptions for easy identification |
| **Auto-Export on Save** | `false` | Automatically export to JSON every time a description is saved or reset |

### Supported Pages Group

Enable/disable editing for specific encyclopedia page types:

| Setting | Default |
|---|---|
| **Enable Hero Editing** | `true` |
| **Enable Clan Editing** | `true` |
| **Enable Kingdom Editing** | `true` |
| **Enable Settlement Editing** | `true` |

### Sharing Group

| Setting | Description |
|---|---|
| **Export Descriptions to JSON** | Exports all custom descriptions to a shareable JSON file |
| **Import Descriptions from JSON** | Imports descriptions from a JSON file and merges them into the current campaign |
| **Export Heroes Only** | Exports only Hero descriptions |
| **Export Clans Only** | Exports only Clan descriptions |
| **Export Kingdoms Only** | Exports only Kingdom descriptions |
| **Export Settlements Only** | Exports only Settlement descriptions |
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
  "version": 1,
  "exportedAt": "2026-02-07T12:00:00.0000000Z",
  "descriptionCount": 4,
  "descriptions": {
    "lord_1_1": "Derthert is the aging king of Vlandia...",
    "lord_2_3": "Caladog is the High King of Battania...",
    "settlement_town_V1": "Pravend is a coastal town...",
    "clan_empire_1": "The noble house of Pethros..."
  }
}
```

### How It Works

1. Uses **Harmony** to patch encyclopedia page `Refresh()` methods
2. Polls for `Ctrl+E` / `Ctrl+R` / `Ctrl+Z` keypresses via a background timer using Win32 `GetAsyncKeyState`
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
- Report issues on the [Discord server](https://discord.gg/Zhnx9SuE6q)

</details>

---

## 💬 Discord & Support

Join the community Discord for support, suggestions, and discussion:

[![Discord](https://img.shields.io/badge/Join%20Discord-7289DA?style=for-the-badge&logo=discord&logoColor=white)](https://discord.gg/Zhnx9SuE6q)

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
