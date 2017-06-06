using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEditor;
using UnityEngine;

namespace ToolsEditor
{
    [CustomPropertyDrawer(typeof(Vector2i))]
    public class Vector2iDrawer : PropertyDrawer
    {
        private readonly float labelWidth = 20f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects

            float valueWidth = (position.width - labelWidth * 2) / 2;

            var y = new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height);
            var x = new Rect(position.x, position.y, position.width / 2, position.height);

            //        var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
            //        var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

            Rect labelPositionX = new Rect(position.x, position.y, labelWidth, position.height);
            Rect positionX = new Rect(position.x + labelWidth, position.y, valueWidth, position.height);
            Rect labelPositionY = new Rect(position.x + labelWidth + valueWidth, position.y, labelWidth, position.height);
            Rect positionY = new Rect(position.x + 2 * labelWidth + valueWidth, position.y, valueWidth, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.LabelField(labelPositionX, "X");
            EditorGUI.PropertyField(positionX, property.FindPropertyRelative("x"), GUIContent.none);
            EditorGUI.LabelField(labelPositionY, "Y");
            EditorGUI.PropertyField(positionY, property.FindPropertyRelative("y"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
