# 🔌 Editable Encyclopedia — Integration Guide NEW

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
- [Best Practices](#best-practices)
- [Troubleshooting](#troubleshooting)

---

## Overview

Editable Encyclopedia allows players to write custom descriptions for Heroes, Clans, Kingdoms, and Settlements. Other mods can read these descriptions to enrich their own functionality — for example, an AI dialogue mod could use player-written character backstories as context.

There are **two ways** to integrate:

| Method | Pros | Cons |
|---|---|---|
| **DLL Reference** | Real-time access, type-safe, events | Requires EditableEncyclopedia as a dependency |
| **Shared JSON File** | No dependency, works across mod boundaries | File-based, not real-time, requires polling |

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
        if (!EditableEncyclopediaAPI.IsAvailable)
            return;

        string heroNotes = EditableEncyclopediaAPI.GetHeroDescription(hero);

        if (heroNotes != null)
        {
            // Player has written a custom description for this hero
        }
    }

    public void GetAllObjectNotes()
    {
        if (!EditableEncyclopediaAPI.IsAvailable)
            return;

        string clanNotes = EditableEncyclopediaAPI.GetClanDescription(clan);
        string kingdomNotes = EditableEncyclopediaAPI.GetKingdomDescription(kingdom);
        string settlementNotes = EditableEncyclopediaAPI.GetSettlementDescription(settlement);
        string notes = EditableEncyclopediaAPI.GetDescription("lord_1_1");
        bool hasNotes = EditableEncyclopediaAPI.HasDescription("lord_1_1");
    }
}
```

### Bulk Queries

```csharp
Dictionary<string, string> allDescriptions = EditableEncyclopediaAPI.GetAllDescriptions();
var heroNotes = EditableEncyclopediaAPI.GetAllHeroDescriptions();
int count = EditableEncyclopediaAPI.GetDescriptionCount();
```

---

## Method 2: Shared JSON File (No DLL Required)

The JSON file is located at:

```
Documents\Mount and Blade II Bannerlord\Configs\ModSettings\Global\EditableEncyclopedia\descriptions_export.json
```

### JSON Format

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

### Reading from Your Mod

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

public class YourModSharedFileIntegration
{
    private static string GetExportFilePath()
    {
        string documentsPath = Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments);
        return Path.Combine(
            documentsPath,
            "Mount and Blade II Bannerlord",
            "Configs", "ModSettings", "Global",
            "EditableEncyclopedia",
            "descriptions_export.json");
    }

    public Dictionary<string, string> LoadDescriptions()
    {
        var descriptions = new Dictionary<string, string>();
        string filePath = GetExportFilePath();

        if (!File.Exists(filePath))
            return descriptions;

        string json = File.ReadAllText(filePath);
        var root = JObject.Parse(json);
        var obj = root["descriptions"] as JObject;

        if (obj != null)
            foreach (var prop in obj.Properties())
                descriptions[prop.Name] = prop.Value.ToString();

        return descriptions;
    }
}
```

---

## API Reference

### Static Properties

```csharp
bool EditableEncyclopediaAPI.IsAvailable
```

### Generic Methods

```csharp
string EditableEncyclopediaAPI.GetDescription(string objectId)
bool EditableEncyclopediaAPI.HasDescription(string objectId)
```

### Type-Specific Methods

```csharp
string EditableEncyclopediaAPI.GetHeroDescription(Hero hero)
string EditableEncyclopediaAPI.GetClanDescription(Clan clan)
string EditableEncyclopediaAPI.GetKingdomDescription(Kingdom kingdom)
string EditableEncyclopediaAPI.GetSettlementDescription(Settlement settlement)
```

### Bulk Access Methods

```csharp
Dictionary<string, string> EditableEncyclopediaAPI.GetAllDescriptions()
int EditableEncyclopediaAPI.GetDescriptionCount()
List<KeyValuePair<Hero, string>> EditableEncyclopediaAPI.GetAllHeroDescriptions()
```

---

## Example: AI Dialogue Mod Integration

```csharp
using EditableEncyclopedia;
using TaleWorlds.CampaignSystem;

public class AIDialogueMod
{
    public string BuildCharacterContext(Hero hero)
    {
        string context = $"Character: {hero.Name}\nAge: {hero.Age}\n";

        if (EditableEncyclopediaAPI.IsAvailable)
        {
            string customNotes = EditableEncyclopediaAPI.GetHeroDescription(hero);
            if (!string.IsNullOrEmpty(customNotes))
            {
                context += "Player's notes:\n" + customNotes + "\n";
            }
        }

        return context;
    }
}
```

---

## Best Practices

```csharp
// Always check availability
if (EditableEncyclopediaAPI.IsAvailable)
{
    string notes = EditableEncyclopediaAPI.GetHeroDescription(hero);
}

// Always handle null returns
string notes = EditableEncyclopediaAPI.GetHeroDescription(hero);
if (notes != null) { /* use notes */ }

// Cache bulk queries
var all = EditableEncyclopediaAPI.GetAllDescriptions();
foreach (var hero in Hero.All)
    if (all.TryGetValue(hero.StringId, out string n)) { /* process */ }
```

### Declare Dependency Correctly

```xml
<DependedModules>
    <DependedModule Id="EditableEncyclopedia" />
</DependedModules>
```

---

## Troubleshooting

| Problem | Solution |
|---|---|
| `IsAvailable` returns false | Verify mod is enabled, check load order, only call during campaign |
| `GetDescription` returns null | Expected — means no custom description exists yet |
| Cannot load DLL | Check HintPath in .csproj, verify mod is installed |
| JSON file doesn't exist | Player needs to use MCM Export button first |

---

## Questions or Issues?

1. **Join the Discord:** [https://discord.gg/Zhnx9SuE6q](https://discord.gg/Zhnx9SuE6q)
2. **Open an issue on GitHub:** [https://github.com/XMuPb/EditableEncyclopedia/issues](https://github.com/XMuPb/EditableEncyclopedia/issues)

---

**Happy modding! 🎮⚔️**
