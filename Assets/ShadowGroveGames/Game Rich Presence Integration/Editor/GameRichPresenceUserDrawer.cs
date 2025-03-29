using ShadowGroveGames.GameRichPresenceIntegration.Scripts.Presence;
using UnityEditor;
using UnityEngine;

namespace ShadowGroveGames.GameRichPresenceIntegration.Editor
{
    [CustomPropertyDrawer(typeof(GameRichPresenceUser))]
    public class GameRichPresenceUserDrawer : PropertyDrawer
    {
        GUIStyle _labelAlignStyle;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            PrepareGUIStyles();

            SerializedProperty avatar = prop.FindPropertyRelative("_avatar");
            SerializedProperty name = prop.FindPropertyRelative("_username");
            SerializedProperty discrim = prop.FindPropertyRelative("_discriminator");

            SerializedProperty cacheSize = prop.FindPropertyRelative("_cacheSize");
            SerializedProperty cacheFormat = prop.FindPropertyRelative("_cacheFormat");
            SerializedProperty avatarHash = prop.FindPropertyRelative("_avatarHash");

            string displayName = name.stringValue + "#" + discrim.intValue.ToString("D4");
            string imageSize = cacheSize.intValue + " x " + cacheSize.intValue + ", " + cacheFormat.enumNames[cacheFormat.enumValueIndex];

            EditorGUILayout.LabelField(label);

            Rect imageRectangle = new Rect(16, 16, 108, 108);
            imageRectangle.position += EditorGUILayout.GetControlRect().position;

            DrawAvatar(imageRectangle, avatar);

            EditorGUILayout.LabelField(displayName, _labelAlignStyle);
            EditorGUILayout.LabelField(imageSize, _labelAlignStyle);
            EditorGUILayout.LabelField(avatarHash.stringValue, _labelAlignStyle);
            EditorGUILayout.Space(60);
        }

        /// <summary>Draws the avatar box </summary>
        void DrawAvatar(Rect position, SerializedProperty avatarProperty)
        {
            //Draw the backing colour
            EditorGUI.HelpBox(position, "", MessageType.None);

            //Draw the avatar if we have one
            if (avatarProperty != null && avatarProperty.objectReferenceValue != null)
                EditorGUI.DrawTextureTransparent(position, avatarProperty.objectReferenceValue as Texture2D, ScaleMode.ScaleToFit);
        }

        void PrepareGUIStyles()
        {
            _labelAlignStyle = new GUIStyle(EditorStyles.boldLabel);
            _labelAlignStyle.alignment = TextAnchor.MiddleLeft;
            _labelAlignStyle.margin = new RectOffset(170, 0, 0, 0);
            _labelAlignStyle.wordWrap = true;
        }
    }
}
