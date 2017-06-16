using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        private static Vector2i? mouseCoord = null;
        private static Vector2 mousePosition;
        private static MapEditorWindow _instance;

        [MenuItem("Window/Map Editor/Create Window")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            _instance = (MapEditorWindow) EditorWindow.GetWindow(typeof(MapEditorWindow));
            _instance.titleContent = new GUIContent("Map Editor");

            _instance.Show();
        }

        private static void OnScene(SceneView sceneview)
        {
            mousePosition = Event.current.mousePosition;
            mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
            mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
            mousePosition.y = -mousePosition.y;
            mousePosition.y *= -1;

            mouseCoord = _instance._mapEditor.Data.Grid.GetCoordAt(mousePosition);

            //Debug.Log("Drawn : " + mousePosition);

            Handles.BeginGUI();

            string coordText;
            string mousePositionText = "Mouse Position : " + mousePosition + " (World coordinate)";

            if (mouseCoord.HasValue)
            {
                coordText = "Coord : " +  mouseCoord.Value.x + ", " + mouseCoord.Value.y;
            }
            else
            {
                coordText = "Coord : " + "None";
            }

            GUILayout.Label(mousePositionText);
            GUILayout.Label(coordText);
            Handles.EndGUI();
        }

        private void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= OnScene;

            _mapEditor = GameObject.FindObjectOfType<MapEditorModel>();
            _dataBuffer = _mapEditor.Data;
        }

        void OnEnable()
        {
            _instance = this;
            SceneView.onSceneGUIDelegate += OnScene;

            _mapEditor = GameObject.FindObjectOfType<MapEditorModel>();

            _dataBuffer = _mapEditor.Data;

            _showContentColors = new AnimBool(false);
            _showContentColors.valueChanged.AddListener(Repaint);

            if (_dataBuffer != null)
            {
                Load();
            }
        }

        void OnGUI()
        {
            if (_data != null)
            {
                CaseContentGrid grid = _data.Grid;

                _serializedData = new SerializedObject(_data);

                bool reconstructGrid = false;

                grid.DrawGrid = EditorGUILayout.Toggle("Draw grid", grid.DrawGrid);
                _data.DrawCase = EditorGUILayout.Toggle("Draw case", _data.DrawCase);

                Vector2 newSize = EditorGUILayout.Vector2Field("Size", grid.Size);
                float newCaseSize = EditorGUILayout.FloatField("Case size", grid.CaseSize);

                if (newSize != grid.Size
                    || newCaseSize != grid.CaseSize)
                {
                    grid.Size = newSize;
                    grid.CaseSize = newCaseSize;

                    if (grid.CaseSize > 0)
                    {
                        int width = (int)(grid.Size.x / grid.CaseSize);
                        int height = (int)(grid.Size.y / grid.CaseSize);

                        grid.Resize(width, height);
                    }
                }

                grid.Color = EditorGUILayout.ColorField("Grid color", grid.Color);

                DrawColors();

                _serializedData.ApplyModifiedProperties();

                if (GUILayout.Button("Bake & Save"))
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

            string guid = AssetDatabase.CreateFolder(path, name);

            path = AssetDatabase.GUIDToAssetPath(guid);

            name = Path.GetFileName(path);

            _data = ScriptableObjectUtility.CreateAsset<MapEditorData>(path, name);
            CaseContentGrid grid = ScriptableObjectUtility.CreateAsset<CaseContentGrid>(path, name + "_grid");

            _data.Grid = grid;

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
            _data.Grid.Clear();

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

            //AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(_data.Grid);
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
            //if (!gridBounds.Contains(point))
            //{
            //    return null;
            //}

            //Vector2i coord = new Vector2i();

            //coord.x = (int) ((point.x + gridBounds.size.x/2)/_data.Grid.CaseSize);
            //coord.y = (int) ((point.y + +gridBounds.size.y/2)/_data.Grid.CaseSize);

            //return coord;

            return _mapEditor.Data.Grid.GetCoordAt(point);
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
                        _data.Grid[i][j] = ECaseContent.Blocking;

                        //Debug.Log("Add blocking at " + i + ", " + j);
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