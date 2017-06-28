using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Tools;

// source : https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
// source : http://www.grapefruitgames.com/blog/2013/11/a-min-max-range-for-unity/

namespace ToolsEditor
{
    [CustomPropertyDrawer(typeof(MinMaxAttribute))]
    public class MinMaxDrawer : PropertyDrawer
    {
        private static float valueWidth = 30;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // First get the attribute since it contains the range for the slider
            MinMaxAttribute minMax = attribute as MinMaxAttribute;

            Vector2 interval = property.vector2Value;

            Rect sliderPosition = position;
            sliderPosition.width -= valueWidth * 2;

            EditorGUI.MinMaxSlider(sliderPosition, label, ref interval.x, ref interval.y, minMax.min, minMax.max);

            Rect valuePosition = position;
            valuePosition.x = sliderPosition.x + sliderPosition.width;
            valuePosition.width = valueWidth;

            Vector2 roundedInterval = interval;
            roundedInterval.x = GetRoundedValue(roundedInterval.x);
            roundedInterval.y = GetRoundedValue(roundedInterval.y);

            interval.x = EditorGUI.FloatField(valuePosition, interval.x);

            valuePosition.x += valueWidth;

            interval.y = EditorGUI.FloatField(valuePosition, interval.y);

            property.vector2Value = interval;
        }

        private float GetRoundedValue(float value)
        {
            return Mathf.Round(value * 100) / 100;
        }
    }
}
