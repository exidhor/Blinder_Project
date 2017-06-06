using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MapEditor;
using UnityEditor.AnimatedValues;

namespace MapEditorEditor
{
    public class MapEditorWindow : EditorWindow
    {
        private MapEditorModel _mapEditor;
        private MapEditorData _dataBuffer;

        private AnimBool _showContentColors;

        private SerializedObject _serializedData;

        [MenuItem("Window/Map Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            MapEditorWindow instance = (MapEditorWindow)EditorWindow.GetWindow(typeof(MapEditorWindow));
            instance.titleContent = new GUIContent("Map Editor");

            instance.Show();
        }

        void OnEnable()
        {
            _mapEditor = GameObject.FindObjectOfType<MapEditorModel>();

            _dataBuffer = _mapEditor.data;

            // tmp
            int colorsSize = 2;

            _dataBuffer.Colors = new List<ColorContent>(colorsSize);

            for (int i = 0; i < colorsSize; i++)
            {
                _dataBuffer.Colors.Add(new ColorContent((ECaseContent)i));
            }

            _showContentColors = new AnimBool(false);
            _showContentColors.valueChanged.AddListener(Repaint);

            _serializedData = new SerializedObject(_dataBuffer);
        }

        void OnGUI()
        {
            _dataBuffer.DrawGrid = EditorGUILayout.Toggle("Draw grid", _dataBuffer.DrawGrid);
            _dataBuffer.DrawCase = EditorGUILayout.Toggle("Draw case", _dataBuffer.DrawCase);
            _dataBuffer.Bounds = EditorGUILayout.Vector2Field("Bounds", _dataBuffer.Bounds);
            _dataBuffer.CaseSize = EditorGUILayout.Vector2Field("Case size", _dataBuffer.CaseSize);
            _dataBuffer.GridColor = EditorGUILayout.ColorField("Grid color", _dataBuffer.GridColor);

            DrawColors();

            _mapEditor.SetData(_dataBuffer);

            //SceneView.RepaintAll();
        }

        private void DrawColors()
        {
            _showContentColors.target = EditorGUILayout.Foldout(_showContentColors.target, "Color content");

            if (EditorGUILayout.BeginFadeGroup(_showContentColors.faded))
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < _dataBuffer.Colors.Count; i++)
                {
                    EditorGUILayout.PropertyField(_serializedData.FindProperty("Colors").GetArrayElementAtIndex(i));
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }
    }
}
