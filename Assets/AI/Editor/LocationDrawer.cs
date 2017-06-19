using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapEditor;
using UnityEditor;
using UnityEngine;
using AI;

namespace MapEditorEditor
{
    [CustomPropertyDrawer(typeof(Location))]
    public class LocationDrawer : PropertyDrawer
    {
        private readonly float rows = 4f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * rows;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);


            SerializedProperty locationTypeProperty = property.FindPropertyRelative("_type");

            float propertyHeight = GetPropertyHeight(locationTypeProperty, new GUIContent());
            propertyHeight = position.height / rows;

            //Rect locationTypePosition = new Rect(position.x, position.y, position.width, propertyHeight);
            //Rect locationValuePosition = new Rect(position.x, position.y + propertyHeight, position.width, propertyHeight); 

            var locationLabel = new Rect(position.x, position.y, position.width, propertyHeight);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Draw label
            position = EditorGUI.PrefixLabel(locationLabel, GUIUtility.GetControlID(FocusType.Passive), label, GUIStyle.none);

            //EditorGUI.indentLevel++;

            var locationTypePosition = new Rect(position.x, position.y + propertyHeight, position.width, propertyHeight);
            var locationValuePosition = new Rect(position.x, position.y + propertyHeight * 2, position.width, propertyHeight);
            var isSetPosition = new Rect(position.x, position.y + propertyHeight * 3, position.width, propertyHeight);

            ELocationType locationType = (ELocationType)(locationTypeProperty.intValue);
            EditorGUI.PropertyField(locationTypePosition, locationTypeProperty, GUIContent.none);

            switch (locationType)
            {
                case ELocationType.Position:
                    {
                        EditorGUI.PropertyField(locationValuePosition, property.FindPropertyRelative("_position"), GUIContent.none);
                        break;
                    }

                case ELocationType.Transform:
                    {
                        EditorGUI.PropertyField(locationValuePosition, property.FindPropertyRelative("_transform"), GUIContent.none);
                        break;
                    }

                case ELocationType.Kinematic:
                    {
                        EditorGUI.PropertyField(locationValuePosition, property.FindPropertyRelative("_kinematic"), GUIContent.none);
                        break;
                    }
            }

            EditorGUI.PropertyField(isSetPosition, property.FindPropertyRelative("_isSet"));

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
