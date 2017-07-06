using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapEditor;
using Tools;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MapEditorEditor
{
    [CustomEditor(typeof(EditModeGridSnap))]
    [CanEditMultipleObjects]
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
                for (int i = 0; i < targets.Length; i++)
                {
                    EditModeGridSnap gridSnap = (EditModeGridSnap) targets[i];

                    Bounds globalBounds = GlobalBoundsHelper.FindGlobalBounds(gridSnap.gameObject, gridSnap.BoundsType);

                    gridSnap.Fit(globalBounds);
                }
            }

            if (GUILayout.Button("Save Offset"))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    EditModeGridSnap gridSnap = (EditModeGridSnap)targets[i];

                    gridSnap.SaveOffset();
                }
            }
        }
    }
}