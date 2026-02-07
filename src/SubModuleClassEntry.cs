using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace EditableEncyclopedia
{
    public class SubModuleClassEntry : MBSubModuleBase
    {
        private Harmony _harmony;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            _harmony = new Harmony("com.xmupb.editableencyclopedia");
            _harmony.PatchAll();
        }

        protected override void OnSubModuleUnloaded()
        {
            _harmony?.UnpatchAll("com.xmupb.editableencyclopedia");
            base.OnSubModuleUnloaded();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);

            if (gameStarter is CampaignGameStarter campaignStarter)
            {
                campaignStarter.AddBehavior(new DescriptionStorageBehavior());
                MessageHelper.ShowDebug("DescriptionStorageBehavior registered.");
            }
        }
    }
}
