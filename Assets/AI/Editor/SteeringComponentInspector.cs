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

        private bool _showSpecs = false;
        private bool _showSteering = false;

        void OnEnable()
        {
            _targetObject = (SteeringComponent)serializedObject.targetObject;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (Application.isPlaying)
            {
                _showSpecs = EditorGUILayout.Foldout(_showSpecs, "Search & Load Specs");
                if (_showSpecs)
                {
                    DisplaySpecs();
                }
            } 

            if (Application.isPlaying)
            {
                _showSteering = EditorGUILayout.Foldout(_showSteering, "Set New Steering");
                if (_showSteering)
                {
                    DisplaySteering();
                }

                EditorGUILayout.HelpBox("Asked for a recomputing of the Steering. This can be usefull if you" +
                        " modify manually (by the inspector) some internal values of the steering.", MessageType.Info);


                if (GUILayout.Button("Force Refresh"))
                {
                    _targetObject.RefreshSteering();
                }
            }
        }

        private void DisplaySpecs()
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.HelpBox("Search into the SteeringSpecsTable the right specs, and set it" +
                                    " to the SteeringComponent. Be sure to type an existant name." +
                                    "You can find names in children of SteeringSpecsTable.", MessageType.Info);

            _specsName = EditorGUILayout.TextField("name", _specsName);

            if (GUILayout.Button("Load Specs"))
            {
                _targetObject.SetSpecs(_specsName);
            }

            EditorGUI.indentLevel--;
        }

        private void DisplaySteering()
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.HelpBox("Set the asked steering to the SteeringComponent.", MessageType.Info);

            _steeringType = (ESteeringType)EditorGUILayout.EnumPopup("type", _steeringType);

            if (GUILayout.Button("Load Steering"))
            {
                _targetObject.SetSteering(_steeringType);
            }

            EditorGUILayout.Space();

            EditorGUI.indentLevel--;
        }
    }
}
