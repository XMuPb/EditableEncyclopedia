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

Editable Encyclopedia allows players to write custom descriptions, lore fields (backstory, personality, goals, relationships, rumors, chronicle), and tags for **5 entity types**: Heroes, Clans, Kingdoms, Settlements, and **Concepts**. Other mods can read this data to enrich their own functionality — for example, an AI dialogue mod could use player-written character backstories as context.

> **v2.0.0 code quality overhaul:** All internal code now uses diagnostic logging via `MCMSettings.DebugLog()` and reflection-safe access patterns, making integration debugging significantly easier. See [v2.0.0 API Improvements](#v200-api-improvements) for details.

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

### JSON Format (v11)

```json
{
  "version": 11,
  "exportedAt": "2026-03-24T12:00:00.0000000Z",
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
  },
  "cultureDefCount": 0,
  "cultureDefs": {},
  "heroInfoFieldCount": 2,
  "heroInfoFields": {
    "backstory_lord_1_1": "Born in the highlands of Vlandia...",
    "chronicle_main_hero": "Day 15 of Spring, 1084\nDefeated the bandits..."
  },
  "tagCount": 1,
  "tags": {
    "lord_1_1": "ally, king"
  },
  "timestampCount": 0,
  "timestamps": {},
  "journalCount": 1,
  "journal": {
    "lord_1_1": "Day 15 of Spring, 1084|[War] Defeated Ragnar near Varcheg\nDay 16 of Spring, 1084|[Politics] Became ruler"
  },
  "relationNoteCount": 1,
  "relationNotes": {
    "main_hero_lord_1_1": "Trusted ally and king"
  },
  "tagNoteCount": 1,
  "tagNotes": {
    "lord_1_1|ally": "Helped me in the siege of Pravend"
  },
  "relationHistoryCount": 0,
  "relationHistory": {},
  "tagCategoryCount": 0,
  "tagCategories": {},
  "tagPresetCount": 0,
  "tagPresets": {},
  "perHeroAutoTagThresholdCount": 0,
  "perHeroAutoTagThresholds": {},
  "relationNoteTagCount": 0,
  "relationNoteTags": {},
  "relationNoteTagLockCount": 0,
  "relationNoteTagLocks": {}
}
```

| Field | Type | Description |
|---|---|---|
| `version` | `int` | Format version (currently `11`) |
| `exportedAt` | `string` | ISO 8601 UTC timestamp of when the export was created |
| `descriptions` | `object` | Key-value pairs: StringId → description text |
| `names` | `object` | Key-value pairs: StringId → custom name |
| `titles` | `object` | Key-value pairs: StringId → custom title |
| `banners` | `object` | Key-value pairs: StringId → banner code |
| `cultures` | `object` | Key-value pairs: heroId → `"cultureId\|displayName"` (v3+) |
| `occupations` | `object` | Key-value pairs: heroId → occupation enum name (v3+) |
| `cultureDefs` | `object` | Key-value pairs: culture StringId → troop tree definition (v4+) |
| `heroInfoFields` | `object` | Key-value pairs: `fieldKey_heroId` → narrative text (v4+) |
| `tags` | `object` | Key-value pairs: StringId → comma-separated tag list (v5+) |
| `timestamps` | `object` | Key-value pairs: StringId → edit date string (v6+) |
| `journal` | `object` | Key-value pairs: StringId → newline-separated `"date\|text"` entries (v7+) |
| `relationNotes` | `object` | Key-value pairs: `"heroId_targetHeroId"` → note text (v8+) |
| `tagNotes` | `object` | Key-value pairs: `"objectId\|tagName"` → annotation text (v9+) |
| `relationHistory` | `object` | Key-value pairs: `"heroId_targetHeroId"` → relation change history (v10+) |
| `tagCategories` | `object` | Key-value pairs: category name → comma-separated tag list (v10+) |
| `tagPresets` | `object` | Key-value pairs: preset name → comma-separated tag list (v10+) |
| `perHeroAutoTagThresholds` | `object` | Key-value pairs: heroId → `"enemyThreshold\|friendThreshold"` (v10+) |
| `relationNoteTags` | `object` | Key-value pairs: `"viewingHeroId_targetHeroId"` → tags on relation notes (v11+) |
| `relationNoteTagLocks` | `object` | Key-value pairs: `"viewingHeroId_targetHeroId"` → lock state for relation note tags (v11+) |

> **Backward compatibility:** The importer reads v1 through v11 files. Missing sections are simply skipped.

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
/// Exports all data to the shared JSON file on disk (v11 format, 19 data sections):
/// descriptions, names, titles, banners, cultures, occupations, cultureDefs,
/// heroInfoFields, tags, timestamps, journal, relationNotes, tagNotes,
/// relationHistory, tagCategories, tagPresets, perHeroAutoTagThresholds,
/// relationNoteTags, relationNoteTagLocks.
/// Returns true on success, false on failure.
/// </summary>
bool EditableEncyclopediaAPI.ExportToSharedFile()

/// <summary>
/// Imports all data from the shared JSON file and merges into the current campaign.
/// Supports v1 through v11 formats. Missing sections are skipped.
/// Returns total entries imported, or -1 on failure.
/// </summary>
int EditableEncyclopediaAPI.ImportFromSharedFile()

/// <summary>
/// Imports from shared JSON file and returns per-section counts.
/// ImportResult has: Descriptions, Names, Titles, Banners, CultureDefs,
/// Cultures, Occupations, HeroInfoFields, Tags, Journal, RelationNotes,
/// TagNotes, and Total properties.
/// </summary>
ImportResult EditableEncyclopediaAPI.ImportFromSharedFileDetailed()

/// <summary>
/// Returns the full path to the shared descriptions JSON file.
/// </summary>
string EditableEncyclopediaAPI.GetSharedFilePath()

/// Export by type — exports only specific entity categories:
bool EditableEncyclopediaAPI.ExportHeroDescriptions()
bool EditableEncyclopediaAPI.ExportClanDescriptions()
bool EditableEncyclopediaAPI.ExportKingdomDescriptions()
bool EditableEncyclopediaAPI.ExportSettlementDescriptions()
bool EditableEncyclopediaAPI.ExportBannerCodes()

/// Import banners only from the shared JSON file:
int EditableEncyclopediaAPI.ImportBannersFromSharedFile()
```

**Direct file access** (for mods that don't reference the DLL):

```csharp
// Read the JSON file directly using SharedFileExporter:
SharedFileExporter.ExportData data = SharedFileExporter.ImportAll();
// data.Descriptions, data.Names, data.Titles, data.Banners,
// data.Cultures, data.Occupations, data.CultureDefs, data.HeroInfoFields,
// data.Tags, data.Timestamps, data.Journal, data.RelationNotes,
// data.TagNotes, data.RelationHistory, data.TagCategories,
// data.TagPresets, data.PerHeroAutoTagThresholds
// — each is Dictionary<string, string> or null if section missing

// Get file path:
string path = SharedFileExporter.GetExportFilePath();
// → Documents\Mount and Blade II Bannerlord\Configs\ModSettings\
//   Global\EditableEncyclopedia\descriptions_export.json
```

### Custom Names, Titles & Banners (v1.1.0+)

These are accessed via `EncyclopediaEditBehavior.Instance` (the campaign behavior singleton), not the static API class.

**v2.0.0 name validation:** Names set via the mod's Ctrl+N popup are now sanitized before being stored:
- Control characters (newlines, tabs, etc.) are stripped
- Max length is enforced (configurable via MCM, default 100 characters)
- The user is warned on-screen if characters were removed

**v2.0.0 Concept page support:** Concept encyclopedia pages now support name editing via Ctrl+N, in addition to Heroes, Clans, Kingdoms, and Settlements.

```csharp
var behavior = EncyclopediaEditBehavior.Instance;

// ── Custom Names ────────────────────────────────────────────
string customName = behavior.GetCustomName(objectId);     // Returns custom name or null
bool hasName = behavior.HasCustomName(objectId);           // True if custom name exists
var allNames = behavior.GetAllCustomNames();               // All custom names (for export)

// ── Custom Banners ──────────────────────────────────────────
string bannerCode = behavior.GetCustomBannerCode(objectId); // Serialized banner string or null
bool hasBanner = behavior.HasCustomBanner(objectId);         // True if custom banner exists
var allBanners = behavior.GetAllCustomBanners();             // All custom banners (for export)

// ── Custom Timestamps ───────────────────────────────────────
string editDate = behavior.GetTimestamp(objectId);            // "Day 15 of Spring, 1084" or null
```

### Info Stats Methods (v2.0.0+)

Info stats are computed from live game state. Each method returns a `Dictionary<string, string>` of key-value pairs.

```csharp
/// <summary>
/// Returns computed stats for a Hero: Culture, Occupation, Kingdom, Location,
/// Status, Spouse, Troops, Morale, Companions, Towns, Castles, Garrisons,
/// Workshops, Influence, Kills, Battles, Tournaments, Hall Rank.
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetHeroInfoStats(Hero hero)
Dictionary<string, string> EditableEncyclopediaAPI.GetHeroInfoStats(string heroId)

/// <summary>
/// Returns computed stats for a Clan: Kingdom, Leader, Culture, Renown,
/// Influence, Troops, Parties, Lords, Companions, Towns, Castles, Villages.
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetClanInfoStats(Clan clan)

/// <summary>
/// Returns computed stats for a Kingdom: Ruler, Culture, Clans, Lords, Towns,
/// Castles, Villages, Total Troops, Total Garrisons, Active Wars, At War With.
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetKingdomInfoStats(Kingdom kingdom)

/// <summary>
/// Returns computed stats for a Settlement: Owner, Clan, Kingdom, Culture,
/// Prosperity, Loyalty, Security, Food, Garrison, Militia, Wall Level,
/// Workshops, Governor, Bound Villages, Notables. Village-specific: Hearths, Bound To.
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetSettlementInfoStats(Settlement settlement)
Dictionary<string, string> EditableEncyclopediaAPI.GetSettlementInfoStats(string settlementId)
```

| Page Type | Stats Returned |
|-----------|----------------|
| **Hero** | Culture, Occupation, Kingdom, Location, Status, Spouse, Troops, Morale, Companions, Towns, Castles, Garrisons, Workshops, Influence, Battles (W/L), Kills (Heroes/Troops), Tournaments, Hall Rank |
| **Clan** | Kingdom, Leader, Culture, Renown, Influence, Troops, Parties, Lords, Companions, Towns, Castles, Villages |
| **Kingdom** | Ruler, Culture, Clans, Lords, Towns, Castles, Villages, Total Troops, Total Garrisons, Active Wars, At War With |
| **Settlement** | Owner, Clan, Kingdom, Culture, Prosperity, Loyalty, Security, Food, Garrison, Militia, Wall Level, Workshops, Governor, Bound Villages, Notables |
| **Concept** | (Description and name editing supported; no computed stats) |

### Culture/Occupation Methods (v1.1.3+)

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

### Hero Lore Fields Methods (v2.0.0+)

The Hero Lore System provides 7 editable fields per hero, stored as key-value pairs in the `_heroInfoFields` dictionary:

| Field Key | Description | Display Location |
|-----------|-------------|-----------------|
| `description` | Main hero description | Description area (replaces native text) |
| `backstory` | Origin story, birthplace, key life events | Lore section |
| `personality` | Temperament, leadership style, fatal flaw | Lore section |
| `goals` | Short-term objectives, long-term ambitions | Lore section |
| `relationships` | Allies, rivals, family, enemies | Lore section |
| `rumors` | Gossip, scandals, secrets | Lore section |
| `chronicle` | Dated journal entries (auto-prepends game date) | Journal section |

**Lore Story Templates** — Built-in writing prompts for 5 character roles help players craft structured lore:

| Role | Backstory | Goals | Personality | Relationships | Rumors |
|------|-----------|-------|-------------|---------------|--------|
| **Lord** | Birthplace, noble house, lineage, first battle | Military conquest, political power, dynastic legacy | Temperament, honor code, fatal flaw | Liege, vassals, rivals, alliances | Court gossip, battlefield legends |
| **Merchant** | Trade origins, specialty goods, trade routes | Market expansion, wealth accumulation | Negotiation style, risk tolerance | Partners, clients, rivals, informants | Shady deals, hidden wealth |
| **Wanderer** | Why they left home, places traveled, skills | Personal quest, dream destination | Outlook on life, survival instinct | Companions, debts, old enemies | Mysterious past, secret abilities |
| **Gang Leader** | Street origins, rise to power, territory | Expansion, rival elimination, rackets | Demeanor, fear vs loyalty, code | Lieutenants, enforcers, corrupt officials | Bodies, betrayals, hidden stash |
| **Preacher** | Faith origins, teachings, followers | Spiritual mission, conversions | Piety, conviction, inner doubt | Flock, clergy, patrons, persecutors | Heresies, miracles, past sins |

Templates use placeholder tokens (`{settlement}`, `{culture}`, `{name}`) that can be filled in by the player.

```csharp
/// <summary>
/// Returns a hero's lore field value by field key and hero StringId.
/// Field keys: "backstory", "personality", "goals", "relationships", "rumors", "chronicle"
/// Returns null if no value set for that field.
/// </summary>
string EditableEncyclopediaAPI.GetHeroInfoField(string fieldKey, string heroId)

/// <summary>
/// Returns the total count of hero lore field entries across all heroes.
/// </summary>
int EditableEncyclopediaAPI.GetHeroInfoFieldCount()

/// <summary>
/// Returns all hero lore fields as a dictionary for export.
/// Key format: "fieldKey_heroId" (e.g., "backstory_lord_1_1")
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetAllHeroInfoFieldsForExport()

/// <summary>
/// Returns the comma-separated tag string for an entity, or null if no tags set.
/// </summary>
string EditableEncyclopediaAPI.GetTags(string objectId)

/// <summary>
/// Returns the count of entities with custom tags.
/// </summary>
int EditableEncyclopediaAPI.GetTagCount()

/// Returns all unique tag names used across all entities.
IEnumerable<string> EditableEncyclopediaAPI.GetAllUniqueTags()

/// Returns all entity StringIds that have a specific tag.
List<string> EditableEncyclopediaAPI.GetObjectsWithTag(string tag)

/// Returns entity StringIds that have ANY of the given tags.
List<string> EditableEncyclopediaAPI.GetObjectsWithAnyTag(IEnumerable<string> tags)

/// Returns entity StringIds that have ALL of the given tags.
List<string> EditableEncyclopediaAPI.GetObjectsWithAllTags(IEnumerable<string> tags)

/// Returns usage count for each tag across all entities.
Dictionary<string, int> EditableEncyclopediaAPI.GetTagUsageCounts()

/// Renames a tag globally across all entities.
void EditableEncyclopediaAPI.RenameTagGlobal(string oldTag, string newTag)

/// Removes a tag globally from all entities.
void EditableEncyclopediaAPI.RemoveTagGlobal(string tag)

/// Merges source tag into target tag across all entities.
void EditableEncyclopediaAPI.MergeTags(string sourceTag, string targetTag)
```

### Hero Lore Convenience Methods (v2.0.0+)

```csharp
/// <summary>
/// Returns ALL lore fields for a hero as a dictionary (fieldKey -> text).
/// Only includes fields that have been set. Keys: "backstory", "personality",
/// "goals", "relationships", "rumors", "chronicle".
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetAllHeroLoreFields(string heroId)

/// <summary>
/// Returns true if a hero has ANY lore field set.
/// </summary>
bool EditableEncyclopediaAPI.HasHeroLore(string heroId)
```

### Lore Story Templates API (v2.0.0+)

The mod includes built-in writing prompts for 5 character roles. These can be accessed programmatically:

```csharp
/// <summary>
/// Returns available character role names: "Lord", "Merchant", "Wanderer", "GangLeader", "Preacher".
/// </summary>
string[] EditableEncyclopediaAPI.GetAvailableRoles()

/// <summary>
/// Returns field keys that have templates: "backstory", "personality", "goals", "relationships", "rumors".
/// </summary>
string[] EditableEncyclopediaAPI.GetTemplateFieldKeys()

/// <summary>
/// Returns the resolved template for a hero — placeholders ({name}, {culture}, {settlement},
/// {clan}, {faction}, {occupation}, {date}) are filled with the hero's actual values.
/// Tries occupation-specific → culture-specific → default template.
/// Returns null if no template exists.
/// </summary>
string EditableEncyclopediaAPI.GetLoreTemplate(string fieldKey, string heroId)

/// <summary>
/// Returns the raw (unresolved) template for a role and field.
/// Example: GetRoleTemplate("Lord", "backstory") returns:
/// "Born in: {settlement}\nNoble House: {clan}\nCulture: {culture}\nLineage: \n..."
/// </summary>
string EditableEncyclopediaAPI.GetRoleTemplate(string role, string fieldKey)

/// <summary>
/// Returns ALL templates for a role as a dictionary (fieldKey -> template text).
/// Example: GetAllRoleTemplates("Merchant") returns 5 field templates.
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetAllRoleTemplates(string role)
```

**Template Lookup Priority:**
1. Occupation-specific: `template_backstory_Lord` (matches hero's occupation)
2. Culture-specific: `template_backstory_sturgia` (matches hero's culture)
3. Default: `template_backstory` (generic template)

**Available Roles and Fields:**

| Role | backstory | personality | goals | relationships | rumors |
|------|-----------|-------------|-------|---------------|--------|
| **Lord** | Birthplace, noble house, lineage, first battle | Temperament, leadership, honor code, fatal flaw | Military, political, dynastic, threats | Liege, vassals, court rivals, marriage alliances | Court gossip, battlefield legends, scandals |
| **Merchant** | Trade origins, specialty goods, routes, rivals | Negotiation style, risk tolerance, ethics | Trade expansion, wealth, new markets | Partners, clients, rivals, informants | Shady deals, hidden wealth, smuggling |
| **Wanderer** | Why they left, places traveled, skills | Outlook, survival instinct, trust issues | Current quest, dream, skills to master | Companions, debts, enemies, love interest | Mysterious past, strange abilities, bounty |
| **GangLeader** | Street origins, rise to power, territory | Demeanor, fear vs loyalty, personal code | Territory expansion, rival gangs, rackets | Lieutenants, enforcers, corrupt officials | Bodies buried, betrayals, hidden stash |
| **Preacher** | Faith origins, teachings, followers | Piety, conviction, tolerance, inner doubt | Spiritual mission, conversions, enemies | Flock, clergy, patrons, persecutors | Heresies, miracles, forbidden texts |

**v2.0.0 Change: Role Templates Auto-Display**

As of v2.0.0, when a hero has no custom lore saved for a field, the Lore section automatically displays the resolved role template as default content. This means `GetRoleTemplate()` output is now visible in the encyclopedia UI without the user clicking "+Add". The `ResolveFieldTemplate()` method (in `EncyclopediaEditPopup`) was changed from `private` to `internal` for this purpose.

### Chronicle Methods (v2.0.0+)

```csharp
/// <summary>
/// Returns the hero's Chronicle lore field (the auto-dated running journal).
/// Returns null if no chronicle exists.
/// </summary>
string EditableEncyclopediaAPI.GetHeroChronicle(string heroId)

/// <summary>
/// Returns ALL chronicle/journal entries across all entities as a flat list.
/// Each entry has EntityId, Date, and Text properties.
/// This is the same data shown in the Global Chronicle Panel.
/// </summary>
List<ChronicleEntry> EditableEncyclopediaAPI.GetAllChronicleEntries()

// ChronicleEntry class:
// - EntityId (string) — the StringId of the source entity
// - Date (string) — the in-game date string
// - Text (string) — the entry text (may include [War], [Politics] etc.)
```

### Journal & Chronicle Methods (v2.0.0+)

```csharp
/// <summary>
/// Returns the journal/chronicle entries for an entity as a raw string.
/// Entries are newline-separated "date|text" format.
/// Returns null if no journal entries exist.
/// </summary>
string EditableEncyclopediaAPI.GetJournalEntries(string objectId)

/// <summary>
/// Returns the total count of entities with journal entries.
/// </summary>
int EditableEncyclopediaAPI.GetJournalCount()

/// <summary>
/// Returns all journal entries as a dictionary for export.
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetAllJournalForExport()
```

### Relation Notes & History Methods (v2.0.0+)

```csharp
/// <summary>
/// Returns the relation note between two heroes.
/// Key format: "viewingHeroId_targetHeroId"
/// Returns null if no note exists.
/// </summary>
string EditableEncyclopediaAPI.GetRelationNote(string viewingHeroId, string targetHeroId)

/// <summary>
/// Returns all relation notes as a dictionary for export.
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetAllRelationNotesForExport()

/// <summary>
/// Returns all relation history entries as a dictionary.
/// Key format: "heroId_targetHeroId", Value: newline-separated "date|change|description"
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetRelationHistory()

/// <summary>
/// Returns relation history for a specific hero (all their relationships).
/// </summary>
Dictionary<string, string> EditableEncyclopediaAPI.GetRelationHistoryForHero(string heroId)
```

### Timeline Methods (v2.0.0+)

```csharp
/// <summary>
/// Returns timeline entries for a hero — personal events where they participated.
/// Includes battles, captures, sieges, raids, kills, family events, clan changes, tournaments.
/// Uses TimelineDataCollector to filter world events to hero-specific ones.
/// </summary>
List<TimelineEntry> EditableEncyclopediaAPI.GetHeroTimeline(string heroId)
```

### Import with Detailed Results (v2.0.0+)

```csharp
/// <summary>
/// Imports from shared JSON file and returns detailed results per data section.
/// </summary>
ImportResult EditableEncyclopediaAPI.ImportFromSharedFileDetailed()
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

### Debug logging (v2.0.0+)

All exceptions are now logged to `Logs/debug.log` with class name and context. Enable debug mode in MCM to also see messages on-screen. If your mod integrates with the API and encounters unexpected behavior, check the debug log first — it will contain detailed error messages including the originating class and method.

### Reflection failures (v2.0.0+)

If a game update breaks reflection, the debug log will show which property/field could not be found (e.g., `"Patches: Culture field not found via reflection"`). This typically means a Bannerlord update changed internal class layouts. Check for an updated version of Editable Encyclopedia, or report the issue on GitHub.

---

## v2.0.0 API Improvements

### Error Handling

All API methods now log errors via `MCMSettings.DebugLog()` instead of silently swallowing exceptions. If your mod calls the API and something goes wrong, check `Logs/debug.log` in the mod's config folder.

### SafeGet Pattern

Bulk info stat methods (`GetHeroInfoStats`, `GetClanInfoStats`, `GetKingdomInfoStats`, `GetSettlementInfoStats`) now use a `SafeGet()` helper that:
- Returns the stat value if available
- Returns a fallback (`"Unknown"` or `"0"`) on null/error
- Logs the failure for debugging

This means callers no longer need to worry about null values in stat dictionaries — every expected key will be present.

### EditPopupInjector Custom Parameters (v2.0.0)

`EditPopupInjector` now supports 3 optional parameters for mod integration, enabling other mods to customize the edit popup behavior when invoking it programmatically:

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `description` | `string` | (same as input) | Separate preview text from the input text — allows showing context (e.g., "Native Name: Skorin") while the input field remains empty |
| `confirmText` | `string` | `"Done"` | Custom confirm button text (e.g., "Next (Edit Title)" for hero name flow) |
| `tipText` | `string` | (from `edit_tips` localization) | Custom tip text overriding the default editing tips hint |

Additionally, `EncyclopediaEditPopup` exposes a `PendingCustomTipText` field for deferred tip injection when the popup widget tree is built asynchronously. `ShowNativeTextInquiry` was changed from `private` to `internal` for cross-class fallback access.

New localization keys for name/title editing:
- `edit_tips_name` — "Enter a new name below. Max {0} characters. Leave empty to reset to native name."
- `edit_tips_title` — "Enter a new title below. Max {0} characters. Leave empty to reset to native title."
- `timeline_no_events` — Empty timeline message for hero pages with no events

### Name Validation

Names set via the mod's Ctrl+N popup are now sanitized:
- Control characters (newlines, tabs, etc.) stripped
- Max length enforced (configurable via MCM, default 100)
- User warned if characters were removed

### New Entity Support

Concept encyclopedia pages now support name editing via Ctrl+N. This brings the total supported entity types to 5: Heroes, Clans, Kingdoms, Settlements, and Concepts.

---

## v2.0.0 Integration Notes (Layer Detection & Role Templates)

### API Changes
- `EncyclopediaEditPopup.ResolveFieldTemplate(string fieldKey, string heroId)` — Changed from `private` to `internal`. Returns the resolved template string with all placeholders filled in, or null if no template exists. Used internally by `LoreSectionInjector` to display role templates as default lore content.

### Behavioral Changes
- **Lore section now shows role templates by default** — When no custom lore is saved for a hero, the Lore section displays resolved occupation/culture/default templates. This means the "+Backstory", "+Personality" etc. buttons are replaced by actual template content for heroes with matching roles (Lord, Merchant, Wanderer, GangLeader, Preacher).
- **Encyclopedia layer detection is stricter** — All injectors now require `EncyclopediaDividerButtonWidget` in candidate layers. If your mod adds custom encyclopedia-like pages, ensure they include this widget type or the injectors won't target them.
- **Deferred retry on non-map screens** — Widget injection on Clan/Party/Inventory screens is deferred by ~200ms to wait for the encyclopedia overlay layer. If your mod hooks into the same Postfix, be aware that injection may happen asynchronously.

---

## Questions or Issues?

If you have questions about integrating with Editable Encyclopedia, or encounter issues:

1. **Check this guide first** - most common scenarios are covered above
2. **Join the Discord:** [https://discord.com/users/404393620897136640](https://discord.com/users/404393620897136640)
3. **Open an issue on GitHub:** [https://github.com/XMuPb/EditableEncyclopedia/issues](https://github.com/XMuPb/EditableEncyclopedia/issues)

---

## License

This integration guide is part of the Editable Encyclopedia project, licensed under the [MIT License](https://opensource.org/licenses/MIT).
