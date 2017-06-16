using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapEditor;
using Tools;
using UnityEditor;
using UnityEngine;

namespace MapEditorEditor
{
    [CustomEditor(typeof(EditModeGridSnap))]
    public class EditModeGridSnapInspector : Editor
    {
        private EditModeGridSnap _targetObject;

        private Bounds _globalBounds;
        private bool _globalBoundsIsFilled;

        void OnEnable()
        {
            _targetObject = (EditModeGridSnap) serializedObject.targetObject;

            _globalBounds = GlobalBoundsHelper.FindGlobalBounds(_targetObject.gameObject, _targetObject.BoundsType);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Auto Fit"))
            {
                _globalBounds = GlobalBoundsHelper.FindGlobalBounds(_targetObject.gameObject, _targetObject.BoundsType);

                _targetObject.Fit(_globalBounds);                
            }

            if (GUILayout.Button("Save Offset"))
            {
                _targetObject.SaveOffset();
            }
        }
    }
}