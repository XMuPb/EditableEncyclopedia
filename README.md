<div align="center">

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
- **Persistent Storage** — All custom descriptions are saved in your campaign save file
- **Multiple Page Types Supported:**
  - Heroes (characters)
  - Clans
  - Kingdoms / Factions
  - Settlements (towns, castles, villages)
- **Export / Import** — Export all your custom descriptions to a JSON file and import them into other campaigns
- **Reset to Default** — Easily restore the original game description for any page
- **Character Limit** — Optionally enforce a maximum character limit on descriptions
- **Flexible MCM Settings** — Extensive configuration options via the MCM v5 settings menu
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
   - If you've already customized a description, the **Cancel** button changes to **Reset** to restore the original text

### Export & Import

#### Export

1. Open MCM settings (**Options → Mod Options**)
2. Navigate to **"Editable Encyclopedia" → "General"**
3. Click the **Export to JSON** button
4. Your descriptions are saved to:
   ```
   Documents\Mount and Blade II Bannerlord\Configs\ModSettings\Global\EditableEncyclopedia\descriptions_export.json
   ```

#### Import

1. Place a valid `descriptions_export.json` file in the location above
2. Open MCM settings
3. Navigate to **"Editable Encyclopedia" → "General"**
4. Click the **Import from JSON** button
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
| **Open Config Folder** | Opens the folder containing MCM settings and export files |

### General Group

| Setting | Default | Description |
|---|---|---|
| **Show Edit Hint** | `true` | Append `[Ctrl+E to Edit Description]` to encyclopedia pages |
| **Show Confirmation Messages** | `true` | Display green success messages when saving descriptions |
| **Export Descriptions** | — | Button to export all custom descriptions to JSON |
| **Import Descriptions** | — | Button to import descriptions from JSON |

### Supported Pages Group

Enable/disable editing for specific encyclopedia page types:

| Setting | Default | Notes |
|---|---|---|
| **Enable Hero Editing** | `true` | |
| **Enable Clan Editing** | `true` | |
| **Enable Kingdom Editing** | `true` | |
| **Enable Settlement Editing** | `true` | |
| **Enable Unit Editing** | `true` | ⚠️ Not currently functional in Bannerlord API |
| **Enable Concept Editing** | `true` | ⚠️ Not currently functional in Bannerlord API |

### Advanced Group

| Setting | Default | Description |
|---|---|---|
| **Initial Key Poll Delay** | `500ms` | Delay before `Ctrl+E` starts working on a page. Prevents accidental triggers when rapidly navigating. |
| **Key Poll Interval** | `50ms` | How often the mod checks for `Ctrl+E`. Lower = more responsive, higher = less CPU usage. |
| **Max Description Length** | `0` (unlimited) | Set to a positive number to enforce a character limit. Descriptions exceeding the limit are automatically truncated on save. |

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

For a complete integration guide, see [INTEGRATION.md](INTEGRATION.md).

### Save Data

- Custom descriptions are stored in your campaign save file via `CampaignBehaviorBase`
- Each description is keyed by the object's `StringId`
- Descriptions survive save/load cycles automatically

### Export Format

The JSON export uses a simple format:

```json
{
  "descriptions": {
    "hero_lord_123": "Custom description for Lord 123...",
    "settlement_town_456": "Custom description for Town 456..."
  }
}
```

### How It Works

1. Uses **Harmony** to patch encyclopedia page `Refresh()` methods
2. Polls for `Ctrl+E` keypresses via a background timer
3. Displays a native Bannerlord text inquiry popup for editing
4. Automatically stops polling when the encyclopedia is closed

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

### v1.0.0

- 🎉 Initial release
- Support for Hero, Clan, Kingdom, and Settlement pages
- Export/Import functionality
- Reset to default feature
- Character limit option
- Comprehensive MCM v5 settings
- Debug mode for troubleshooting
