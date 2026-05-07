using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Avastrad.UI.UiSystem.Editor
{
    [CustomPropertyDrawer(typeof(ScreenIdReference))]
    internal class ScreenIdReferenceDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lines = 3;
            var configProperty = property.FindPropertyRelative("config");
            var idProperty = property.FindPropertyRelative("id");
            if (configProperty.objectReferenceValue is ScreenIdConfig config)
                if (!IdIsValid(idProperty.stringValue, config))
                    lines = 4;
            
            return lines * EditorGUIUtility.singleLineHeight
                   + (lines - 1) * EditorGUIUtility.standardVerticalSpacing
                   + 8; // padding
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw frame/background
            GUI.Box(position, GUIContent.none, EditorStyles.helpBox);

            // Optional padding
            var padded = new Rect(
                position.x + 4,
                position.y + 4,
                position.width - 8,
                position.height - 8
            );

            EditorGUI.BeginProperty(position, label, property);

            var configProperty = property.FindPropertyRelative("config");
            var idProperty = property.FindPropertyRelative("id");
            
            var lineHeight = EditorGUIUtility.singleLineHeight;
            var spacing = EditorGUIUtility.standardVerticalSpacing;
            var y = padded.y;

            var labelRect = new Rect(padded.x, y, padded.width, lineHeight);
            EditorGUI.LabelField(labelRect, label, EditorStyles.boldLabel);
            y += lineHeight + spacing;

            var configRect = new Rect(padded.x, y, padded.width, lineHeight);
            EditorGUI.PropertyField(configRect, configProperty, new GUIContent("Screen ID Config"));
            y += lineHeight + spacing;

            if (configProperty.objectReferenceValue is ScreenIdConfig config)
            {
                var ids = config.Ids;
                if (ids == null || ids.Count == 0)
                {
                    var helpRect = new Rect(padded.x, y, padded.width, lineHeight);
                    EditorGUI.HelpBox(helpRect, "Config is empty", MessageType.Warning);
                }
                else
                {
                    var sortedIds = ids.OrderBy(id => id).ToArray();
                    var currentValue = idProperty.stringValue;
                    var selectedIndex = System.Array.IndexOf(sortedIds, currentValue);
                    
                    if (selectedIndex < 0)
                    {
                        var helpRect = new Rect(padded.x, y, padded.width, lineHeight);
                        EditorGUI.HelpBox(helpRect, "Selected ID not found in the config", MessageType.Error);
                        y += lineHeight + spacing;

                        sortedIds = sortedIds.Concat(new[] { currentValue }).ToArray();
                        var popupRect = new Rect(padded.x, y, padded.width, lineHeight);
                        selectedIndex = EditorGUI.Popup(popupRect, "Screen ID", sortedIds.Length-1, sortedIds);
                    }
                    else
                    {
                        var popupRect = new Rect(padded.x, y, padded.width, lineHeight);
                        selectedIndex = EditorGUI.Popup(popupRect, "Screen ID", selectedIndex, sortedIds);
                    }

                    idProperty.stringValue = sortedIds[selectedIndex];
                }
            }
            else
            {
                var helpRect = new Rect(padded.x, y, padded.width, lineHeight);
                EditorGUI.HelpBox(helpRect, "Assign a Screen ID Config to enable ID selection", MessageType.Error);
            }

            EditorGUI.EndProperty();
        }

        private static bool IdIsValid(string id, ScreenIdConfig config) 
            => config != null && config.ContainsId(id);
    }
}