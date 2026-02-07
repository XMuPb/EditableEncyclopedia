using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace EditableEncyclopedia.Settings
{
    internal sealed class EditableEncyclopediaSettings : AttributeGlobalSettings<EditableEncyclopediaSettings>
    {
        public override string Id => "EditableEncyclopedia";
        public override string DisplayName => "Editable Encyclopedia";
        public override string FolderName => "EditableEncyclopedia";
        public override string FormatType => "json2";

        // ── Info Group ──────────────────────────────────────────────

        [SettingPropertyText("Author", Order = 0, RequireRestart = false, HintText = "Mod author")]
        [SettingPropertyGroup("Info", GroupOrder = 0)]
        public string Author { get; set; } = "XMuPb";

        [SettingPropertyText("Version", Order = 1, RequireRestart = false, HintText = "Current mod version")]
        [SettingPropertyGroup("Info")]
        public string Version { get; set; } = "v1.1.0";

        // ── General Group ───────────────────────────────────────────

        [SettingPropertyBool("Show Edit Hint", Order = 0, RequireRestart = false,
            HintText = "Append [Ctrl+E to Edit Description] to encyclopedia pages")]
        [SettingPropertyGroup("General", GroupOrder = 1)]
        public bool ShowEditHint { get; set; } = true;

        [SettingPropertyBool("Show Confirmation Messages", Order = 1, RequireRestart = false,
            HintText = "Display green success messages when saving descriptions")]
        [SettingPropertyGroup("General")]
        public bool ShowConfirmationMessages { get; set; } = true;

        [SettingPropertyButton("Export Descriptions to JSON", Order = 2, RequireRestart = false,
            Content = "Export",
            HintText = "Export all custom descriptions to a JSON file for sharing")]
        [SettingPropertyGroup("General")]
        public void OnExportClicked()
        {
            var result = DescriptionExportImportService.ExportDescriptions();
            if (result.IsSuccess)
                MessageHelper.ShowSuccess(result.Message);
            else
                MessageHelper.ShowError(result.Message);
        }

        [SettingPropertyButton("Import Descriptions from JSON", Order = 3, RequireRestart = false,
            Content = "Import",
            HintText = "Import descriptions from a JSON file — merges with existing descriptions")]
        [SettingPropertyGroup("General")]
        public void OnImportClicked()
        {
            var result = DescriptionExportImportService.ImportDescriptions();
            if (result.IsSuccess)
                MessageHelper.ShowSuccess(result.Message);
            else
                MessageHelper.ShowError(result.Message);
        }

        // ── Supported Pages Group ───────────────────────────────────

        [SettingPropertyBool("Enable Hero Editing", Order = 0, RequireRestart = false,
            HintText = "Allow editing encyclopedia pages for heroes/characters")]
        [SettingPropertyGroup("Supported Pages", GroupOrder = 2)]
        public bool EnableHeroEditing { get; set; } = true;

        [SettingPropertyBool("Enable Clan Editing", Order = 1, RequireRestart = false,
            HintText = "Allow editing encyclopedia pages for clans")]
        [SettingPropertyGroup("Supported Pages")]
        public bool EnableClanEditing { get; set; } = true;

        [SettingPropertyBool("Enable Kingdom Editing", Order = 2, RequireRestart = false,
            HintText = "Allow editing encyclopedia pages for kingdoms/factions")]
        [SettingPropertyGroup("Supported Pages")]
        public bool EnableKingdomEditing { get; set; } = true;

        [SettingPropertyBool("Enable Settlement Editing", Order = 3, RequireRestart = false,
            HintText = "Allow editing encyclopedia pages for settlements")]
        [SettingPropertyGroup("Supported Pages")]
        public bool EnableSettlementEditing { get; set; } = true;

        // ── Advanced Group ──────────────────────────────────────────

        [SettingPropertyInteger("Initial Key Poll Delay (ms)", 100, 2000, Order = 0, RequireRestart = false,
            HintText = "Delay before Ctrl+E starts working on a page")]
        [SettingPropertyGroup("Advanced", GroupOrder = 3)]
        public int InitialKeyPollDelay { get; set; } = 500;

        [SettingPropertyInteger("Key Poll Interval (ms)", 10, 500, Order = 1, RequireRestart = false,
            HintText = "How often the mod checks for Ctrl+E")]
        [SettingPropertyGroup("Advanced")]
        public int KeyPollInterval { get; set; } = 50;

        [SettingPropertyInteger("Max Description Length", 0, 10000, Order = 2, RequireRestart = false,
            HintText = "Maximum character limit (0 = unlimited)")]
        [SettingPropertyGroup("Advanced")]
        public int MaxDescriptionLength { get; set; } = 0;

        // ── Debug Group ─────────────────────────────────────────────

        [SettingPropertyBool("Debug Mode", Order = 0, RequireRestart = false,
            HintText = "Enable verbose logging for troubleshooting")]
        [SettingPropertyGroup("Debug", GroupOrder = 4)]
        public bool DebugMode { get; set; } = false;
    }
}
