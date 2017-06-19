using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AI;
using UnityEditor;
using UnityEngine;

namespace AIEditor
{
    [CustomEditor(typeof(SteeringComponent))]
    class SteeringComponentInspector : Editor
    {
        private SteeringComponent _targetObject;

        private string _specsName = "";
        private ESteeringType _steeringType;

        void OnEnable()
        {
            _targetObject = (SteeringComponent)serializedObject.targetObject;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _specsName = EditorGUILayout.TextField("name", _specsName);

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Load Specs"))
                {
                    _targetObject.SetSpecs(_specsName);
                }
            }

            EditorGUILayout.Space();

            _steeringType = (ESteeringType) EditorGUILayout.EnumPopup("type", _steeringType);

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Load Steering"))
                {
                    _targetObject.SetSteering(_steeringType);
                }

                EditorGUILayout.Space();

                if (GUILayout.Button("Refresh Steering"))
                {
                    _targetObject.RefreshSteering();
                }
            }
        }
    }
}
