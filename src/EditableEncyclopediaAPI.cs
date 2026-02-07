using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;

namespace EditableEncyclopedia
{
    /// <summary>
    /// Public API for other mods to read player-written encyclopedia descriptions.
    /// Always check <see cref="IsAvailable"/> before calling any method.
    /// </summary>
    public static class EditableEncyclopediaAPI
    {
        public static bool IsAvailable => DescriptionStorageBehavior.Instance != null;

        // --- Generic access ---

        public static string GetDescription(string objectId)
        {
            return DescriptionStorageBehavior.Instance?.GetDescription(objectId);
        }

        public static bool HasDescription(string objectId)
        {
            return DescriptionStorageBehavior.Instance?.HasDescription(objectId) ?? false;
        }

        // --- Type-specific convenience methods ---

        public static string GetHeroDescription(Hero hero)
        {
            return hero != null ? GetDescription(hero.StringId) : null;
        }

        public static string GetClanDescription(Clan clan)
        {
            return clan != null ? GetDescription(clan.StringId) : null;
        }

        public static string GetKingdomDescription(Kingdom kingdom)
        {
            return kingdom != null ? GetDescription(kingdom.StringId) : null;
        }

        public static string GetSettlementDescription(Settlement settlement)
        {
            return settlement != null ? GetDescription(settlement.StringId) : null;
        }

        // --- Bulk access ---

        public static Dictionary<string, string> GetAllDescriptions()
        {
            return DescriptionStorageBehavior.Instance?.GetAllDescriptions()
                ?? new Dictionary<string, string>();
        }

        public static int GetDescriptionCount()
        {
            return DescriptionStorageBehavior.Instance?.GetDescriptionCount() ?? 0;
        }

        public static List<KeyValuePair<Hero, string>> GetAllHeroDescriptions()
        {
            var result = new List<KeyValuePair<Hero, string>>();
            var all = GetAllDescriptions();
            foreach (var hero in Hero.AllAliveHeroes.Concat(Hero.DeadOrDisabledHeroes))
            {
                if (all.TryGetValue(hero.StringId, out string desc))
                    result.Add(new KeyValuePair<Hero, string>(hero, desc));
            }
            return result;
        }
    }
}
