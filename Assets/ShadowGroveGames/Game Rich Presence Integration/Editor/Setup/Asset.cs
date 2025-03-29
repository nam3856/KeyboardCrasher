#if UNITY_EDITOR
namespace ShadowGroveGames.GameRichPresenceIntegration.Editor.Setup
{
    internal static class Asset
    {
        internal const string KEY = "GameRichPresenceIntegration";
        internal const string NAME = "Game Rich Presence";
        internal const string LOGO = "rich-presence-integration-banner";
        internal const string REVIEW_URL = "https://assetstore.unity.com/packages/tools/integration/game-rich-presence-239804?utm_source=editor#reviews";
        internal const string README_GUID = "26c60cd764db22742a75fbfc16e7a5fa";

        internal readonly static string[] DONT_SHOW_IF_ASSABMLY_LOADED = new string[]
        {
            "org.Shadow-Grove.CompleteToolboxForDiscord.Editor",
        };

        // Review
        internal const int REVIEW_MIN_OPENINGS = 2;
        internal const int REVIEW_MIN_DAYS = 10;

        // Editor Prefs
        internal const string EDITOR_PREFS_KEY_GETTING_STARTED = KEY + "-GettingStarted";
        internal const string EDITOR_PREFS_KEY_REVIEW_DISABLE_REMINDER = KEY + "-ReviewReminder";
        internal const string EDITOR_PREFS_KEY_REVIEW_EDITOR_OPEN_COUNT = KEY + "-ReviewEditorOpenCount";
        internal const string EDITOR_PREFS_KEY_REVIEW_INIT_DATE = KEY + "-ReviewInitDate";
    }
}
#endif