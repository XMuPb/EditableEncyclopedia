# Editable Encyclopedia — Quick Start

Get started in 2 minutes. For full documentation, see [README.md](README.md).

---

## Installation

1. Install **MCM v5** (Mod Configuration Menu) — required dependency
2. Download **Editable Encyclopedia** and extract to your `Modules` folder
3. Enable the mod in the Bannerlord launcher (load after MCM)
4. Start or load a campaign

---

## Your First Edit

1. Open the **Encyclopedia** (press `N` on the campaign map)
2. Navigate to any hero page
3. Press **Ctrl+E** — the edit popup opens with portrait and character counter
4. Type your custom description and press **Done**
5. Your text is saved to the campaign file automatically

---

## All Keyboard Shortcuts

| Shortcut | What It Does | Pages |
|----------|-------------|-------|
| **Ctrl+E** | Edit description / lore fields | Heroes, Clans, Kingdoms, Settlements |
| **Ctrl+N** | Edit name / title | Heroes, Clans, Kingdoms, Settlements, Concepts |
| **Ctrl+B** | Edit banner (paste banner code) | Heroes, Clans, Kingdoms, Settlements |
| **Ctrl+U** | Edit culture | Heroes only |
| **Ctrl+O** | Edit occupation | Heroes only |
| **Ctrl+G** | Add/remove tags | All pages |
| **Ctrl+J** | Add journal note | All pages |
| **Ctrl+F** | Relation notes (hero-to-hero) | Heroes only |
| **Ctrl+R** | Reset to default | Any edited page |
| **Ctrl+Z** | Undo last edit | Any edited page |

---

## Hero Lore Fields

Press **Ctrl+E** on a hero page to see a field picker:

- **Description** — Main encyclopedia text
- **Backstory** — Character origin story
- **Personality** — Traits and temperament
- **Goals** — Ambitions and objectives
- **Relationships** — Key alliances and rivalries
- **Rumors** — Gossip and secrets
- **Chronicle** — Auto-dated running journal

Each field has **story templates** for Lords, Merchants, Wanderers, Gang Leaders, and Preachers.

---

## Key Features at a Glance

**Tags** — Press Ctrl+G to tag any entity (e.g., "ally", "enemy", "target"). Tags show on the page with colored badges. Auto-tags generate automatically based on game state.

**Journal** — Press Ctrl+J to add dated notes. The auto-journal system also logs 16+ event types automatically (battles, sieges, diplomacy, marriages, deaths) with colored category tags.

**Timeline** — A collapsible section on hero pages showing their personal biography — battles fought, captures, family events. Filters by category with pagination.

**Relation Notes** — Press Ctrl+F on a hero page to write notes about relationships with other heroes. Notes display in a dedicated section with clickable names.

**Global Chronicle** — Click the Chronicle button on the campaign map bar to see all world history events in a filterable, paginated overlay panel.

**Export/Import** — Export all your custom data to JSON (MCM > Export). Import into other campaigns. Auto-export on save and auto-import on load available.

---

## Configuration

Open **Mod Options > Editable Encyclopedia** in the game menu to access **71 settings** including:

- Enable/disable each feature individually
- Show/hide keyboard shortcut hints
- Set character limits for names (default 100) and descriptions (default 5000)
- Configure auto-tags thresholds
- Manage custom cultures, occupations, and tag presets
- Export and import data

---

## Troubleshooting

**Nothing happens when I press Ctrl+E:**
- Make sure the encyclopedia is open (press N first)
- Check MCM settings — the feature might be disabled
- Enable Debug Mode in MCM > Debug to see log messages

**My edits disappeared after reloading:**
- Edits are saved in the campaign save file — make sure you saved the game
- Check if Auto-Import is enabled (it overwrites from JSON on load)

**Debug log location:**
`Documents\Mount and Blade II Bannerlord\Configs\ModSettings\Global\EditableEncyclopedia\Logs\debug.log`

---

For full documentation, API reference, and integration guide, see [README.md](README.md).
