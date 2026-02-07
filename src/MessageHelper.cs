using TaleWorlds.Library;

namespace EditableEncyclopedia
{
    internal static class MessageHelper
    {
        public static void ShowSuccess(string message)
        {
            InformationManager.DisplayMessage(
                new InformationMessage(message, Colors.Green));
        }

        public static void ShowError(string message)
        {
            InformationManager.DisplayMessage(
                new InformationMessage(message, Colors.Red));
        }

        public static void ShowDebug(string message)
        {
            var settings = Settings.EditableEncyclopediaSettings.Instance;
            if (settings != null && settings.DebugMode)
            {
                InformationManager.DisplayMessage(
                    new InformationMessage($"[EE Debug] {message}", Colors.Yellow));
            }
        }

        public static void ShowInfo(string message)
        {
            InformationManager.DisplayMessage(
                new InformationMessage(message, Colors.White));
        }
    }
}
