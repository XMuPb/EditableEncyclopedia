using System.Threading;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace EditableEncyclopedia.Patches
{
    /// <summary>
    /// Harmony patches for encyclopedia page Refresh() methods.
    /// Appends the edit hint and starts a key-poll timer for Ctrl+E on each page.
    /// </summary>
    internal static class EncyclopediaPagePatches
    {
        private static Timer _keyPollTimer;
        private static string _currentObjectId;
        private static string _currentPageType;

        // ── Hero page ───────────────────────────────────────────────

        [HarmonyPatch(typeof(EncyclopediaHeroPageVM), "Refresh")]
        internal static class HeroPagePatch
        {
            static void Postfix(EncyclopediaHeroPageVM __instance)
            {
                var settings = Settings.EditableEncyclopediaSettings.Instance;
                if (settings != null && !settings.EnableHeroEditing) return;

                var hero = __instance.Obj as Hero;
                if (hero == null) return;

                StartEditSession(hero.StringId, "Hero", __instance);
            }
        }

        // ── Clan page ───────────────────────────────────────────────

        [HarmonyPatch(typeof(EncyclopediaClanPageVM), "Refresh")]
        internal static class ClanPagePatch
        {
            static void Postfix(EncyclopediaClanPageVM __instance)
            {
                var settings = Settings.EditableEncyclopediaSettings.Instance;
                if (settings != null && !settings.EnableClanEditing) return;

                var clan = __instance.Obj as Clan;
                if (clan == null) return;

                StartEditSession(clan.StringId, "Clan", __instance);
            }
        }

        // ── Kingdom page ────────────────────────────────────────────

        [HarmonyPatch(typeof(EncyclopediaFactionPageVM), "Refresh")]
        internal static class KingdomPagePatch
        {
            static void Postfix(EncyclopediaFactionPageVM __instance)
            {
                var settings = Settings.EditableEncyclopediaSettings.Instance;
                if (settings != null && !settings.EnableKingdomEditing) return;

                var kingdom = __instance.Obj as Kingdom;
                if (kingdom == null) return;

                StartEditSession(kingdom.StringId, "Kingdom", __instance);
            }
        }

        // ── Settlement page ─────────────────────────────────────────

        [HarmonyPatch(typeof(EncyclopediaSettlementPageVM), "Refresh")]
        internal static class SettlementPagePatch
        {
            static void Postfix(EncyclopediaSettlementPageVM __instance)
            {
                var settings = Settings.EditableEncyclopediaSettings.Instance;
                if (settings != null && !settings.EnableSettlementEditing) return;

                var settlement = __instance.Obj as Settlement;
                if (settlement == null) return;

                StartEditSession(settlement.StringId, "Settlement", __instance);
            }
        }

        // ── Shared logic ────────────────────────────────────────────

        private static void StartEditSession(string objectId, string pageType, ViewModel vm)
        {
            StopPolling();

            _currentObjectId = objectId;
            _currentPageType = pageType;

            var settings = Settings.EditableEncyclopediaSettings.Instance;
            if (settings?.ShowEditHint == true)
            {
                MessageHelper.ShowInfo("[Ctrl+E to Edit Description]");
            }

            MessageHelper.ShowDebug($"Encyclopedia page opened: {pageType} ({objectId})");

            int delay = settings?.InitialKeyPollDelay ?? 500;
            int interval = settings?.KeyPollInterval ?? 50;

            _keyPollTimer = new Timer(
                _ => PollForEditKey(),
                null,
                delay,
                interval);
        }

        private static void PollForEditKey()
        {
            if (!Input.IsKeyDown(InputKey.LeftControl) && !Input.IsKeyDown(InputKey.RightControl))
                return;
            if (!Input.IsKeyPressed(InputKey.E))
                return;

            StopPolling();
            OpenEditDialog(_currentObjectId, _currentPageType);
        }

        private static void OpenEditDialog(string objectId, string pageType)
        {
            var storage = DescriptionStorageBehavior.Instance;
            if (storage == null) return;

            string existingText = storage.GetDescription(objectId) ?? "";

            var settings = Settings.EditableEncyclopediaSettings.Instance;
            int maxLength = settings?.MaxDescriptionLength ?? 0;
            if (maxLength <= 0) maxLength = 10000;

            InformationManager.ShowTextInquiry(
                new TextInquiryData(
                    $"Edit {pageType} Description",
                    string.Empty,
                    true,
                    true,
                    new TextObject("Save").ToString(),
                    existingText != "" ? new TextObject("Reset").ToString() : new TextObject("Cancel").ToString(),
                    onAffirmative: text => OnSave(objectId, text),
                    onNegative: _ => OnCancel(objectId, existingText),
                    false,
                    null,
                    "",
                    existingText),
                false,
                false);
        }

        private static void OnSave(string objectId, string text)
        {
            var storage = DescriptionStorageBehavior.Instance;
            if (storage == null) return;

            var settings = Settings.EditableEncyclopediaSettings.Instance;
            int maxLength = settings?.MaxDescriptionLength ?? 0;
            if (maxLength > 0 && text != null && text.Length > maxLength)
                text = text.Substring(0, maxLength);

            storage.SetDescription(objectId, text);

            if (settings?.ShowConfirmationMessages == true)
                MessageHelper.ShowSuccess("Description saved.");

            MessageHelper.ShowDebug($"Saved description for {objectId} ({text?.Length ?? 0} chars)");
        }

        private static void OnCancel(string objectId, string existingText)
        {
            if (!string.IsNullOrEmpty(existingText))
            {
                DescriptionStorageBehavior.Instance?.RemoveDescription(objectId);
                var settings = Settings.EditableEncyclopediaSettings.Instance;
                if (settings?.ShowConfirmationMessages == true)
                    MessageHelper.ShowSuccess("Description reset to default.");
            }
        }

        public static void StopPolling()
        {
            _keyPollTimer?.Dispose();
            _keyPollTimer = null;
        }
    }
}
