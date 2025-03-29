using ShadowGroveGames.GameRichPresenceIntegration.Scripts;
using UnityEditor;
using UnityEngine;

namespace ShadowGroveGames.GameRichPresenceIntegration.Editor
{
    [CustomEditor(typeof(GameRichPresenceManagerScript))]
    public class GameRichPresenceManagerScriptEditor : UnityEditor.Editor
    {
        GUIStyle _labelWordWrapStyle;
        GUIStyle _smallLabelWordWrapStyle;

        public override void OnInspectorGUI()
        {
            PrepareGUIStyles();
            DrawGameRichPresenceHeader();
            DrawNotice();
            base.OnInspectorGUI();
            DrawNote();
        }

        void PrepareGUIStyles()
        {
            _labelWordWrapStyle = new GUIStyle(EditorStyles.boldLabel);
            _labelWordWrapStyle.wordWrap = true;
            _smallLabelWordWrapStyle = new GUIStyle(EditorStyles.label);
            _smallLabelWordWrapStyle.wordWrap = true;
        }

        void DrawGameRichPresenceHeader()
        {
            Texture2D gameRichPresenceHeader = (Texture2D)Resources.Load("rich-presence-integration-banner", typeof(Texture2D));

            GUI.DrawTexture(new Rect((Screen.width / 2) - (gameRichPresenceHeader.width / 2), 10, gameRichPresenceHeader.width, gameRichPresenceHeader.height), gameRichPresenceHeader, ScaleMode.ScaleToFit, true, gameRichPresenceHeader.width / gameRichPresenceHeader.height);
            EditorGUILayout.Space(gameRichPresenceHeader.height + 20);
        }

        void DrawNotice()
        {
            EditorGUILayout.LabelField("To use the full potential of Game Rich Precense you have to create an application in the Discord Developer Portal.", _labelWordWrapStyle);


#if (UNITY_2021_1_OR_NEWER)
            EditorGUILayout.BeginHorizontal();
            if (EditorGUILayout.LinkButton("Open Discord Developer Portal"))
                Application.OpenURL("https://discord.com/developers/applications");
            EditorGUILayout.EndHorizontal();
#else
            EditorGUILayout.LabelField("https://discord.com/developers/application", _labelWordWrapStyle);
#endif

            EditorGUILayout.Space(20);
        }
        void DrawNote()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Note: You can't click on your own Discord buttons, this is a limitation set by discord.", _smallLabelWordWrapStyle);
        }
    }

}
