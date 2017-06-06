using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MapEditor;
using Tools;
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
            bool reconstructGrid = false;

            _dataBuffer.DrawGrid = EditorGUILayout.Toggle("Draw grid", _dataBuffer.DrawGrid);
            _dataBuffer.DrawCase = EditorGUILayout.Toggle("Draw case", _dataBuffer.DrawCase);

            Vector2 newBounds = EditorGUILayout.Vector2Field("Bounds", _dataBuffer.Bounds);
            float newCaseSize = EditorGUILayout.FloatField("Case size", _dataBuffer.CaseSize);

            if (newBounds != _dataBuffer.Bounds
                || newCaseSize != _dataBuffer.CaseSize)
            {
                _mapEditor.ReconstructGrid();
            }

            _dataBuffer.Bounds = newBounds;
            _dataBuffer.CaseSize = newCaseSize;

            _dataBuffer.GridColor = EditorGUILayout.ColorField("Grid color", _dataBuffer.GridColor);

            DrawColors();

            if (GUILayout.Button("Bake"))
            {
                Bake();
            }
        }

        private void Bake()
        {
            // Find all the obstacles in the scene
            Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

            // Remove old values
            _dataBuffer.ClearGrid();

            // Add the found obstacles
            for (int i = 0; i < obstacles.Length; i++)
            {
                BoxCollider2D[] boxColliders = obstacles[i].GetComponents<BoxCollider2D>();

                for (int j = 0; j < boxColliders.Length; j++)
                {
                    AddBlockingCollider(boxColliders[j]);
                }

                boxColliders = obstacles[i].GetComponentsInChildren<BoxCollider2D>();

                for (int j = 0; j < boxColliders.Length; j++)
                {
                    AddBlockingCollider(boxColliders[j]);
                }
            }
        }

        private void AddBlockingCollider(BoxCollider2D boxCollider)
        {
            Bounds colliderBounds = boxCollider.bounds;
            Bounds gridBounds = _mapEditor.bounds;

            // find bot left position in grid (case unit)
            Vector2 botLeft = colliderBounds.min;
            Vector2i? coordBotLeft = FindCoordInGrid(botLeft, gridBounds);

            // find top right position in grid (case unit)
            Vector2 topRight = colliderBounds.max;
            Vector2i? coordTopRight = FindCoordInGrid(topRight, gridBounds);

            FillGrid(coordBotLeft, coordTopRight, boxCollider.name);
        }

        private Vector2i? FindCoordInGrid(Vector2 point, Bounds gridBounds)
        {
            if (!gridBounds.Contains(point))
            {
                return null;
            }

            Vector2i coord = new Vector2i();

            coord.x = (int) ((point.x + gridBounds.size.x/2) / _dataBuffer.CaseSize);
            coord.y = (int) ((point.y + +gridBounds.size.y/2) / _dataBuffer.CaseSize);

            return coord;
        }

        private void FillGrid(Vector2i? min, Vector2i? max, string objectName)
        {
            bool minIsOutside = min == null;

            bool maxIsOutside = max == null;

            if (minIsOutside && maxIsOutside)
            {
                Debug.Log("Can't handle " + objectName
                    + " because both the min and max bounds are outside the grid.");

                return;
            }

            Vector2i delta = max.Value - min.Value;
            Vector2i start = min.Value;
            Vector2i end = max.Value;

            if (minIsOutside)
            {
                // todo
            }
            else if (maxIsOutside)
            {
                // todo
            }
            else
            {
                for (int i = start.x; i <= end.x; i++)
                {
                    for (int j = start.y; j <= end.y; j++)
                    {
                        int index = i*_mapEditor.caseCount.x + j;

                        _dataBuffer.grid[index] = ECaseContent.Blocking;

                        Debug.Log("Add blocking at " + i + ", " + j);
                    }
                }
            }
        }

        private void DrawColors()
        {
            _showContentColors.target = EditorGUILayout.Foldout(_showContentColors.target, "Color content");

            if (EditorGUILayout.BeginFadeGroup(_showContentColors.faded))
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < _dataBuffer.Colors.Count; i++)
                {
                    SerializedProperty property = _serializedData.FindProperty("Colors").GetArrayElementAtIndex(i);

                    bool isModified = EditorGUILayout.PropertyField(property);

                    if (isModified)
                    {
                        _dataBuffer.Colors[i].Color = property.colorValue;
                    }
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }
    }
}
