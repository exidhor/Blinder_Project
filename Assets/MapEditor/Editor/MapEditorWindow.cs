using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MapEditor;
using Tools;
using ToolsEditor;
using UnityEditor.AnimatedValues;

namespace MapEditorEditor
{
    public class MapEditorWindow : EditorWindow
    {
        private MapEditorModel _mapEditor;
        private MapEditorData _data;
        private MapEditorData _dataBuffer;

        private AnimBool _showContentColors;

        private SerializedObject _serializedData;

        private string _bufferNewNameAsset;

        private bool _showSave = false;
        private bool _showLoad = false;
        private bool _showCreateNew = false;

        [MenuItem("Window/Map Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            MapEditorWindow instance = (MapEditorWindow) EditorWindow.GetWindow(typeof(MapEditorWindow));
            instance.titleContent = new GUIContent("Map Editor");

            instance.Show();
        }

        void Awake()
        {
            Debug.Log("w Awake " + GetInstanceID());
        }

        private void OnDisable()
        {
            Debug.Log("w OnDisable " + GetInstanceID());

            _mapEditor = GameObject.FindObjectOfType<MapEditorModel>();
            _dataBuffer = _mapEditor.Data;
        }

        private void OnDestroy()
        {
            Debug.Log("w OnDestroy " + GetInstanceID());
        }

        void OnEnable()
        {
            //Debug.Log("w OnEnable " + GetInstanceID());

            _mapEditor = GameObject.FindObjectOfType<MapEditorModel>();

            _dataBuffer = _mapEditor.Data;

            _showContentColors = new AnimBool(false);
            _showContentColors.valueChanged.AddListener(Repaint);

            //// tmp
            //int colorsSize = 2;

            //_dataBuffer.Colors = new List<ColorContent>(colorsSize);

            //for (int i = 0; i < colorsSize; i++)
            //{
            //    _dataBuffer.Colors.Add(new ColorContent((ECaseContent) i));
            //}

            //_showContentColors = new AnimBool(false);
            //_showContentColors.valueChanged.AddListener(Repaint);

            //_serializedData = new SerializedObject(_dataBuffer);
        }

        void OnGUI()
        {
            if (_data != null)
            {
                _serializedData = new SerializedObject(_data);

                bool reconstructGrid = false;

                _data.DrawGrid = EditorGUILayout.Toggle("Draw grid", _data.DrawGrid);
                _data.DrawCase = EditorGUILayout.Toggle("Draw case", _data.DrawCase);

                Vector2 newBounds = EditorGUILayout.Vector2Field("Bounds", _data.Bounds);
                float newCaseSize = EditorGUILayout.FloatField("Case size", _data.CaseSize);

                if (newBounds != _data.Bounds
                    || newCaseSize != _data.CaseSize)
                {
                    _data.Bounds = newBounds;
                    _data.CaseSize = newCaseSize;

                    if (_data.CaseSize > 0)
                    {
                        _data.CaseCount.x = (int)(_data.Bounds.x / _data.CaseSize);
                        _data.CaseCount.y = (int)(_data.Bounds.y / _data.CaseSize);

                        _mapEditor.ReconstructGrid();
                    }
                }

                _data.GridColor = EditorGUILayout.ColorField("Grid color", _data.GridColor);

                DrawColors();

                _serializedData.ApplyModifiedProperties();

                if (GUILayout.Button("Bake"))
                {
                    Bake();
                }
            }

            _showCreateNew = EditorGUILayout.Foldout(_showCreateNew, "Create New");
            if (_showCreateNew)
            {
                DrawCreateNew();
            }

            _showLoad = EditorGUILayout.Foldout(_showLoad, "Load");
            if (_showLoad)
            {
                DrawLoad();
            }
        }

        private void DrawCreateNew()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Name", GUILayout.MaxWidth(60));

            _bufferNewNameAsset = EditorGUILayout.TextField(_bufferNewNameAsset);

            if (GUILayout.Button("Create", GUILayout.MaxWidth(100)))
            {
                CreateNew(_bufferNewNameAsset);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawLoad()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Data", GUILayout.MaxWidth(60));

            _dataBuffer = (MapEditorData) EditorGUILayout.ObjectField(_dataBuffer, typeof(MapEditorData), false);

            if (GUILayout.Button("Load", GUILayout.MaxWidth(100)))
            {
                Load();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void Load()
        {
            _data = _dataBuffer;

            _mapEditor.Data = _data;
        }

        private void CreateNew(string name)
        {
            string path = "Assets/Output";

            _data = ScriptableObjectUtility.CreateAsset<MapEditorData>(path, name);

            int colorsSize = 2;

            _data.Colors = new List<ColorContent>(colorsSize);

            for (int i = 0; i < colorsSize; i++)
            {
                _data.Colors.Add(new ColorContent((ECaseContent) i));
            }


            _dataBuffer = _data;
            _mapEditor.Data = _data;

            _serializedData = new SerializedObject(_data);
        }

        private void Bake()
        {
            // Find all the obstacles in the scene
            Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

            // Remove old values
            _data.ClearGrid();

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

            coord.x = (int) ((point.x + gridBounds.size.x/2)/_data.CaseSize);
            coord.y = (int) ((point.y + +gridBounds.size.y/2)/_data.CaseSize);

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
                        //int index = i*_mapEditor.Data.CaseCount.x + j;

                        //_data.grid[index] = ECaseContent.Blocking;
                        _data.Grid[i][j] = ECaseContent.Blocking;

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

                for (int i = 0; i < _data.Colors.Count; i++)
                {
                    SerializedProperty property = _serializedData.FindProperty("Colors").GetArrayElementAtIndex(i);

                    EditorGUILayout.PropertyField(property);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }
    }
}