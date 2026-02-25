# Editable Encyclopedia — Integration Guide

> A guide for Bannerlord mod developers who want to read or react to player-written encyclopedia descriptions.

---

## Table of Contents
- [Overview](#overview)
- [Integration Methods](#integration-methods)
- [Method 1: Direct DLL Reference (Recommended)](#method-1-direct-dll-reference-recommended)
- [Method 2: Shared JSON File (No DLL Required)](#method-2-shared-json-file-no-dll-required)
- [API Reference](#api-reference)
- [Event-Driven Integration](#event-driven-integration)
- [Example: AI Dialogue Mod Integration](#example-ai-dialogue-mod-integration)
- [Example: Reading from Shared JSON](#example-reading-from-shared-json)
- [Best Practices](#best-practices)
- [Troubleshooting](#troubleshooting)

---

## Overview

Editable Encyclopedia allows players to write custom descriptions for Heroes, Clans, Kingdoms, and Settlements. Other mods can read these descriptions to enrich their own functionality — for example, an AI dialogue mod could use player-written character backstories as context.

There are **two ways** to integrate:

| Method | Pros | Cons |
|---|---|---|
| **DLL Reference** | Real-time access, type-safe, events | Requires EditableEncyclopedia as a dependency |
| **Shared JSON File** | No dependency, works across mod boundaries | File-based, not real-time, requires manual export |

---

## Integration Methods

### When to Use Each Method

**Use DLL Reference if:**
- You want real-time access to descriptions
- Your mod needs to react immediately when descriptions change
- You're comfortable adding a mod dependency
- You need type-safe API access

**Use Shared JSON if:**
- You can't or don't want to add a dependency
- Periodic updates are sufficient (e.g., once per game session)
- You need descriptions in a non-C# environment
- Your mod loads independently of EditableEncyclopedia

---

## Method 1: Direct DLL Reference (Recommended)

### Setup

1. **Add DLL Reference**

   Add a reference to `EditableEncyclopedia.dll` in your `.csproj`:

   ```xml
   <ItemGroup>
       <Reference Include="EditableEncyclopedia">
           <HintPath>$(GameFolder)\Modules\EditableEncyclopedia\bin\Win64_Shipping_Client\EditableEncyclopedia.dll</HintPath>
           <Private>False</Private>
       </Reference>
   </ItemGroup>
   ```

2. **Add Dependency in SubModule.xml**

   Ensure EditableEncyclopedia loads before your mod:

   ```xml
   <DependedModules>
       <DependedModule Id="EditableEncyclopedia" />
   </DependedModules>
   ```

3. **Always Check Availability**

   ```csharp
   using EditableEncyclopedia;

   if (EditableEncyclopediaAPI.IsAvailable)
   {
       // Safe to call API methods
   }
   ```

### Basic Usage

```csharp
using EditableEncyclopedia;

public class YourMod
{
    public void GetCharacterNotes(Hero hero)
    {
        // Always check availability first
        if (!EditableEncyclopediaAPI.IsAvailable)
        {
            // EditableEncyclopedia is not loaded or no campaign is active
            return;
        }

        // Get a specific hero's custom description
        string heroNotes = EditableEncyclopediaAPI.GetHeroDescription(hero);

        if (heroNotes != null)
        {
            // Player has written a custom description for this hero
            // Use it as context, display it, etc.
            InformationManager.DisplayMessage(
                new InformationMessage($"Found notes about {hero.Name}: {heroNotes}"));
        }
        else
        {
            // No custom description exists for this hero
            InformationManager.DisplayMessage(
                new InformationMessage($"{hero.Name} has no custom notes."));
        }
    }

    public void GetAllObjectNotes()
    {
        if (!EditableEncyclopediaAPI.IsAvailable)
            return;

        // Check other object types
        Clan clan = Clan.PlayerClan;
        string clanNotes = EditableEncyclopediaAPI.GetClanDescription(clan);

        Kingdom kingdom = Kingdom.All.FirstOrDefault();
        string kingdomNotes = EditableEncyclopediaAPI.GetKingdomDescription(kingdom);

        Settlement settlement = Settlement.All.FirstOrDefault();
        string settlementNotes = EditableEncyclopediaAPI.GetSettlementDescription(settlement);

        // Generic access by StringId
        string notes = EditableEncyclopediaAPI.GetDescription("lord_1_1");

        // Check if a description exists without retrieving it
        bool hasNotes = EditableEncyclopediaAPI.HasDescription("lord_1_1");
    }
}
```

### Bulk Queries

```csharp
using EditableEncyclopedia;
using System.Collections.Generic;

public class YourMod
{
    public void ProcessAllCustomDescriptions()
    {
        if (!EditableEncyclopediaAPI.IsAvailable)
            return;

        // Get ALL custom descriptions at once
        // Dictionary<string, string> where key = StringId, value = description
        Dictionary<string, string> allDescriptions =
            EditableEncyclopediaAPI.GetAllDescriptions();

        InformationManager.DisplayMessage(
            new InformationMessage(
                $"Player has written {allDescriptions.Count} custom descriptions"));

        foreach (var kvp in allDescriptions)
        {
            string objectId = kvp.Key;
            string description = kvp.Value;
            // Process each description...
        }
    }

    public void ProcessHeroNotesOnly()
    {
        if (!EditableEncyclopediaAPI.IsAvailable)
            return;

        // Get all Heroes that have custom descriptions
        // Returns Dictionary<string, string> where key = hero StringId
        var heroNotes = EditableEncyclopediaAPI.GetAllHeroDescriptions();

        foreach (var kvp in heroNotes)
        {
            string heroId = kvp.Key;
            string description = kvp.Value;

            // Example: Use as AI dialogue context
            // SendToAI($"Character background for {heroId}: {description}");
        }
    }

    public void GetDescriptionCount()
    {
        if (!EditableEncyclopediaAPI.IsAvailable)
            return;

        // Get total count of custom descriptions
        int count = EditableEncyclopediaAPI.GetDescriptionCount();

        InformationManager.DisplayMessage(
            new InformationMessage($"Total custom descriptions: {count}"));
    }
}
```

---

## Method 2: Shared JSON File (No DLL Required)

Editable Encyclopedia can export descriptions to a shared JSON file that any mod can read. Players export via **MCM -> Editable Encyclopedia -> Sharing -> Export**.

### Setup

The JSON file is created when the player uses the **Export** button in MCM settings, and is located at:

```
Documents\Mount and Blade II Bannerlord\Configs\ModSettings\Global\EditableEncyclopedia\descriptions_export.json
```

### JSON Format (v3)

```json
{
  "version": 3,
  "exportedAt": "2026-02-22T12:30:00.0000000Z",
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
    "lord_6_12": "nord|Viking Warrior"
  },
  "occupationCount": 1,
  "occupations": {
    "lord_6_12": "GangLeader"
  }
}
```

| Field | Type | Description |
|---|---|---|
| `version` | `int` | Format version (currently `3`) |
| `exportedAt` | `string` | ISO 8601 UTC timestamp of when the export was created |
| `descriptions` | `object` | Key-value pairs: StringId → description text |
| `names` | `object` | Key-value pairs: StringId → custom name |
| `titles` | `object` | Key-value pairs: StringId → custom title |
| `banners` | `object` | Key-value pairs: StringId → banner code |
| `cultures` | `object` | Key-value pairs: heroId → `"cultureId\|displayName"` (v3+) |
| `occupations` | `object` | Key-value pairs: heroId → occupation enum name (v3+) |

> **Backward compatibility:** The importer reads v1, v2, and v3 files. Missing sections are simply skipped.

### Reading from Your Mod

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using TaleWorlds.Library;

public class YourModSharedFileIntegration
{
    private static string GetExportFilePath()
    {
        string documentsPath = Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments);
        return Path.Combine(
            documentsPath,
            "Mount and Blade II Bannerlord",
            "Configs",
            "ModSettings",
            "Global",
            "EditableEncyclopedia",
            "descriptions_export.json");
    }

    public Dictionary<string, string> LoadDescriptions()
    {
        var descriptions = new Dictionary<string, string>();

        try
        {
            string filePath = GetExportFilePath();

            if (!File.Exists(filePath))
            {
                // File doesn't exist yet - player hasn't exported
                return descriptions;
            }

            string json = File.ReadAllText(filePath);
            var root = JObject.Parse(json);
            var descriptionsObj = root["descriptions"] as JObject;

            if (descriptionsObj != null)
            {
                foreach (var prop in descriptionsObj.Properties())
                {
                    descriptions[prop.Name] = prop.Value.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            // Handle errors (file not found, invalid JSON, etc.)
            InformationManager.DisplayMessage(
                new InformationMessage(
                    $"Failed to load EditableEncyclopedia export: {ex.Message}"));
        }

        return descriptions;
    }

    public string GetHeroDescription(string heroStringId)
    {
        var descriptions = LoadDescriptions();

        if (descriptions.TryGetValue(heroStringId, out string description))
        {
            return description;
        }

        return null;
    }
}
```

### Limitations of JSON Method

- **Not Real-Time:** Requires the player to manually export via MCM (Sharing group)
- **Polling Required:** Your mod must periodically re-read the file to detect changes
- **No Events:** You won't be notified when descriptions change
- **File I/O Overhead:** Reading from disk is slower than in-memory access

---

## API Reference

### Static Properties

```csharp
/// <summary>
/// Returns true if Editable Encyclopedia is loaded and a campaign is active.
/// Always check this before calling other API methods from external mods.
/// </summary>
bool EditableEncyclopediaAPI.IsAvailable
```

### Generic Methods

```csharp
/// <summary>
/// Gets the custom description for any object by its StringId.
/// Returns null if no custom description exists.
/// </summary>
string EditableEncyclopediaAPI.GetDescription(string objectId)

/// <summary>
/// Returns true if a custom description exists for the given StringId.
/// </summary>
bool EditableEncyclopediaAPI.HasDescription(string objectId)
```

### Type-Specific Methods

```csharp
/// <summary>
/// Gets the custom description for a Hero.
/// Returns null if no custom description exists.
/// </summary>
string EditableEncyclopediaAPI.GetHeroDescription(Hero hero)

/// <summary>
/// Gets the custom description for a Clan.
/// Returns null if no custom description exists.
/// </summary>
string EditableEncyclopediaAPI.GetClanDescription(Clan clan)

/// <summary>
/// Gets the custom description for a Kingdom.
/// Returns null if no custom description exists.
/// </summary>
string EditableEncyclopediaAPI.GetKingdomDescription(Kingdom kingdom)

/// <summary>
/// Gets the custom description for a Settlement.
/// Returns null if no custom description exists.
/// </summary>
string EditableEncyclopediaAPI.GetSettlementDescription(Settlement settlement)
```

### Bulk Access Methods

```csharp
/// <summary>
/// Returns a read-only copy of ALL custom descriptions.
/// Key = StringId, Value = custom description text.
/// Returns an empty dictionary if unavailable.
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetAllDescriptions()

/// <summary>
/// Returns the total number of custom descriptions the player has written.
/// </summary>
int EditableEncyclopediaAPI.GetDescriptionCount()

/// <summary>
/// Returns all hero descriptions as a dictionary (StringId -> description).
/// Filters by heroes that exist in the current campaign.
/// Returns an empty dictionary if unavailable.
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetAllHeroDescriptions()

/// <summary>
/// Returns all clan descriptions as a dictionary (StringId -> description).
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetAllClanDescriptions()

/// <summary>
/// Returns all kingdom descriptions as a dictionary (StringId -> description).
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetAllKingdomDescriptions()

/// <summary>
/// Returns all settlement descriptions as a dictionary (StringId -> description).
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetAllSettlementDescriptions()
```

### Export/Import Methods

```csharp
/// <summary>
/// Exports all data (descriptions, names, titles, banners, cultures, occupations)
/// to the shared JSON file on disk (v3 format).
/// Returns true on success, false on failure.
/// </summary>
bool EditableEncyclopediaAPI.ExportToSharedFile()

/// <summary>
/// Imports all data from the shared JSON file and merges into the current campaign.
/// Supports v1, v2, and v3 formats. Returns total entries imported, or -1 on failure.
/// </summary>
int EditableEncyclopediaAPI.ImportFromSharedFile()

/// <summary>
/// Returns the full path to the shared descriptions JSON file.
/// </summary>
string EditableEncyclopediaAPI.GetSharedFilePath()
```

### Culture/Occupation Methods (v1.1.4+)

```csharp
/// <summary>
/// Returns the custom culture display name for a hero, or null if none set.
/// </summary>
string EditableEncyclopediaAPI.GetHeroCulture(string heroId)

/// <summary>
/// Returns the custom occupation value for a hero, or -1 if none set.
/// </summary>
int EditableEncyclopediaAPI.GetHeroOccupation(string heroId)

/// <summary>
/// Returns a friendly display name for an occupation value (e.g., 21 → "Gang Leader").
/// </summary>
string EditableEncyclopediaAPI.GetOccupationDisplayName(int occupationValue)

/// <summary>
/// Returns the count of heroes with custom culture/occupation assignments.
/// </summary>
int EditableEncyclopediaAPI.GetCustomCultureCount()
int EditableEncyclopediaAPI.GetCustomOccupationCount()

/// <summary>
/// Returns all custom cultures/occupations as dictionaries for export.
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetAllCustomCultures()
Dictionary<string, string> EditableEncyclopediaAPI.GetAllCustomOccupations()
```

---

## Event-Driven Integration

Editable Encyclopedia exposes an `OnDescriptionChanged` event that fires whenever a description is saved or removed:

```csharp
using EditableEncyclopedia;

public class YourModBehavior : CampaignBehaviorBase
{
    public override void RegisterEvents()
    {
        // Subscribe to description changes
        EditableEncyclopediaAPI.OnDescriptionChanged += OnDescriptionChanged;
    }

    private void OnDescriptionChanged(string objectId, string newDescription)
    {
        if (newDescription == null)
        {
            // Description was removed (reset to default)
            // React accordingly...
        }
        else
        {
            // Description was added or updated
            // React accordingly...
        }
    }

    public override void SyncData(IDataStore dataStore)
    {
        // Persist your data if needed
    }
}
```

**Event signature:**
```csharp
/// Args: (string objectId, string newDescription)
/// newDescription is null if the description was removed/reset.
event Action<string, string> EditableEncyclopediaAPI.OnDescriptionChanged
```

---

## Example: AI Dialogue Mod Integration

A practical example showing how an AI-powered dialogue mod might use player-written character notes:

```csharp
using EditableEncyclopedia;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

public class AIDialogueMod
{
    public string GenerateDialogue(Hero hero, string conversationContext)
    {
        // Build context for AI
        string context = $"Character: {hero.Name}\n";
        context += $"Age: {hero.Age}\n";
        context += $"Occupation: {hero.Occupation}\n\n";

        // Add player-written notes if available
        if (EditableEncyclopediaAPI.IsAvailable)
        {
            string customNotes = EditableEncyclopediaAPI.GetHeroDescription(hero);

            if (!string.IsNullOrEmpty(customNotes))
            {
                context += "Player's notes about this character:\n";
                context += customNotes + "\n\n";
            }
        }

        context += $"Conversation context: {conversationContext}\n";

        // Send to your AI API
        string response = SendToAI(context);

        return response;
    }

    private string SendToAI(string context)
    {
        // Your AI integration logic here
        return "AI-generated response based on context";
    }

    // Example: Check all companions at start of campaign
    public void InitializeCompanionContext()
    {
        if (!EditableEncyclopediaAPI.IsAvailable)
            return;

        // Get all heroes with custom descriptions
        var heroNotes = EditableEncyclopediaAPI.GetAllHeroDescriptions();

        foreach (var kvp in heroNotes)
        {
            string heroId = kvp.Key;
            string notes = kvp.Value;

            // Look up the hero object if needed
            Hero hero = Hero.AllAliveHeroes.FirstOrDefault(h => h.StringId == heroId);
            if (hero != null && hero.IsPlayerCompanion)
            {
                // Pre-load companion context for AI
                InformationManager.DisplayMessage(
                    new InformationMessage(
                        $"Loaded custom context for companion {hero.Name}",
                        Colors.Cyan));
            }
        }
    }
}
```

---

## Example: Reading from Shared JSON

A complete example of reading the JSON export file:

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

public class DescriptionExportReader
{
    private Dictionary<string, string> _descriptions;
    private DateTime _lastLoadTime;

    public DescriptionExportReader()
    {
        _descriptions = new Dictionary<string, string>();
        _lastLoadTime = DateTime.MinValue;
    }

    public void LoadIfNeeded()
    {
        // Only reload every 5 minutes to avoid excessive file I/O
        if ((DateTime.Now - _lastLoadTime).TotalMinutes < 5)
            return;

        string filePath = GetExportFilePath();

        if (!File.Exists(filePath))
        {
            InformationManager.DisplayMessage(
                new InformationMessage(
                    "EditableEncyclopedia export not found. " +
                    "Player needs to use Export button in MCM (Sharing group)."));
            return;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            var root = JObject.Parse(json);
            var descriptionsObj = root["descriptions"] as JObject;

            _descriptions.Clear();

            if (descriptionsObj != null)
            {
                foreach (var prop in descriptionsObj.Properties())
                {
                    _descriptions[prop.Name] = prop.Value.ToString();
                }
            }

            _lastLoadTime = DateTime.Now;

            InformationManager.DisplayMessage(
                new InformationMessage(
                    $"Loaded {_descriptions.Count} descriptions from export",
                    Colors.Green));
        }
        catch (Exception ex)
        {
            InformationManager.DisplayMessage(
                new InformationMessage(
                    $"Failed to load descriptions: {ex.Message}",
                    Colors.Red));
        }
    }

    public string GetDescription(string objectId)
    {
        LoadIfNeeded();

        if (_descriptions.TryGetValue(objectId, out string description))
        {
            return description;
        }

        return null;
    }

    public string GetHeroDescription(Hero hero)
    {
        if (hero == null)
            return null;

        return GetDescription(hero.StringId);
    }

    private static string GetExportFilePath()
    {
        string documentsPath = Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments);
        return Path.Combine(
            documentsPath,
            "Mount and Blade II Bannerlord",
            "Configs",
            "ModSettings",
            "Global",
            "EditableEncyclopedia",
            "descriptions_export.json");
    }
}
```

---

## Best Practices

### Always Check Availability

```csharp
// GOOD
if (EditableEncyclopediaAPI.IsAvailable)
{
    string notes = EditableEncyclopediaAPI.GetHeroDescription(hero);
}

// BAD - Will throw NullReferenceException if mod not loaded
string notes = EditableEncyclopediaAPI.GetHeroDescription(hero);
```

### Handle Null Returns Gracefully

```csharp
// GOOD
string notes = EditableEncyclopediaAPI.GetHeroDescription(hero);
if (notes != null)
{
    // Use the notes
}

// BAD - Will throw NullReferenceException if no description exists
string notes = EditableEncyclopediaAPI.GetHeroDescription(hero);
int length = notes.Length; // Crash!
```

### Cache Bulk Queries

```csharp
// GOOD - Query once, use many times
var allDescriptions = EditableEncyclopediaAPI.GetAllDescriptions();
foreach (var hero in Hero.All)
{
    if (allDescriptions.TryGetValue(hero.StringId, out string notes))
    {
        // Process notes
    }
}

// BAD - Queries for every hero individually
foreach (var hero in Hero.All)
{
    string notes = EditableEncyclopediaAPI.GetHeroDescription(hero);
}
```

### Use the OnDescriptionChanged Event

```csharp
// GOOD - React in real-time to description changes
EditableEncyclopediaAPI.OnDescriptionChanged += (objectId, desc) =>
{
    // Update your cache or react immediately
};

// LESS IDEAL - Polling at intervals
CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, () =>
{
    var descriptions = EditableEncyclopediaAPI.GetAllDescriptions();
    // Compare with cache...
});
```

### Declare Dependency Correctly

Always add EditableEncyclopedia to your `SubModule.xml` dependencies:

```xml
<DependedModules>
    <DependedModule Id="EditableEncyclopedia" />
</DependedModules>
```

This ensures:
- Correct load order
- Mod won't load if EditableEncyclopedia is disabled
- Clear dependency relationship for users

---

## Troubleshooting

### "IsAvailable returns false"

**Possible causes:**
1. EditableEncyclopedia mod is not installed or enabled
2. No campaign is currently active (main menu)
3. Your mod loads before EditableEncyclopedia (check load order)

**Solution:**
- Verify EditableEncyclopedia is enabled in launcher
- Ensure you added it to `SubModule.xml` dependencies
- Only call API during campaign (not in main menu)

### "GetDescription returns null for known objects"

**Possible causes:**
1. Player hasn't written a custom description for that object yet
2. The description was deleted/reset

**Solution:**
- This is expected behavior - null means no custom description exists
- Always check for null before using the returned string

### "Cannot load EditableEncyclopedia.dll"

**Possible causes:**
1. DLL path in `.csproj` is incorrect
2. EditableEncyclopedia is not installed in the game's Modules folder
3. Wrong game version (DLL compiled for different Bannerlord version)

**Solution:**
- Verify the `HintPath` in your `.csproj` points to the correct location
- Check that EditableEncyclopedia is installed
- Ensure DLL version matches your Bannerlord version

### "JSON export file doesn't exist"

**Possible causes:**
1. Player hasn't used the Export button in MCM yet
2. Wrong file path

**Solution:**
- Inform the player to use **MCM -> Editable Encyclopedia -> Sharing -> Export** button
- Verify the file path is correct for the current OS/user

### "Descriptions are out of date"

**Possible causes:**
1. Using JSON method and player hasn't re-exported recently
2. Caching descriptions for too long

**Solution:**
- For JSON method: Instruct player to re-export after making changes
- For DLL method: Use the `OnDescriptionChanged` event for real-time updates, or query in real-time
- Consider implementing periodic refresh logic

---

## Questions or Issues?

If you have questions about integrating with Editable Encyclopedia, or encounter issues:

1. **Check this guide first** - most common scenarios are covered above
2. **Join the Discord:** [https://discord.com/users/404393620897136640](https://discord.com/users/404393620897136640)
3. **Open an issue on GitHub:** [https://github.com/XMuPb/EditableEncyclopedia/issues](https://github.com/XMuPb/EditableEncyclopedia/issues)

---

## License

This integration guide is part of the Editable Encyclopedia project, licensed under the [MIT License](https://opensource.org/licenses/MIT).
