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

        private AnimBool _showContentColors;

        private SerializedObject _serializedData;

        private string _bufferNewNameAsset;

        private bool _showGridSettings = false;
        private bool _showColors = false;
        private bool _showLoad = false;
        private bool _showCreateNew = false;

        private static Vector2i? mouseCoord = null;
        private static Vector2 mousePosition;
        private static MapEditorWindow _instance;

        private readonly string folder = "Assets/Resources/Output";

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
            _data = _mapEditor.Data;
        }

        void OnEnable()
        {
            _instance = this;
            SceneView.onSceneGUIDelegate += OnScene;

            _mapEditor = GameObject.FindObjectOfType<MapEditorModel>();

            // todo check if there is a data
            _data = _mapEditor.Data;

            _showContentColors = new AnimBool(false);
            _showContentColors.valueChanged.AddListener(Repaint);

            if (_data != null)
            {
                Load();
            }
        }

        void OnGUI()
        {
            if (_data != null)
            {
                _serializedData = new SerializedObject(_data);

                _data.Grid.DrawGrid = EditorGUILayout.Toggle("Draw grid", _data.Grid.DrawGrid);
                _data.DrawCase = EditorGUILayout.Toggle("Draw case", _data.DrawCase);

                _showGridSettings = EditorGUILayout.Foldout(_showGridSettings, "Grid Settings");
                if (_showGridSettings)
                {
                    DrawGridSettings();
                }

                _showColors = EditorGUILayout.Foldout(_showColors, "Colors");
                if (_showColors)
                {
                    DrawColors();
                }

                _serializedData.ApplyModifiedProperties();
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

            if (_data != null)
            {
                string buttonName = "Bake & Save";

                EditorGUILayout.HelpBox("\"" + buttonName + "\" will regenerate the map and mark as \"isBlocking\" every case which contains" +
                                        " a collider2D. Then it will save the output in " + folder + " .", MessageType.Info);

                if (GUILayout.Button(buttonName))
                {
                    Bake();
                }
            }
        }

        private void DrawGridSettings()
        {
            EditorGUILayout.HelpBox("Modify this regenerate the grid.", MessageType.Info);

            Vector2 newSize = EditorGUILayout.Vector2Field("Grid Size", _data.Grid.Size);
            float newCaseSize = EditorGUILayout.FloatField("Case size", _data.Grid.CaseSize);

            if (newSize != _data.Grid.Size
                || newCaseSize != _data.Grid.CaseSize)
            {
                _data.Grid.Size = newSize;
                _data.Grid.CaseSize = newCaseSize;

                if (_data.Grid.CaseSize > 0)
                {
                    int width = (int)(_data.Grid.Size.x / _data.Grid.CaseSize);
                    int height = (int)(_data.Grid.Size.y / _data.Grid.CaseSize);

                    _data.Grid.Resize(width, height);
                }

                _data.Grid.Bufferize();
            }
        }

        private void DrawColors()
        {
            _data.Grid.Color = EditorGUILayout.ColorField("Grid", _data.Grid.Color);

            EditorGUILayout.LabelField("Cases");

            DrawColorContents();
        }

        private void DrawCreateNew()
        {
            EditorGUILayout.HelpBox("Create a new data, composed of a folder contained two assets." +
                        " The folder is saved in " + folder + " .", MessageType.Info);

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
            EditorGUILayout.HelpBox("Load the data and display its values in this window.", MessageType.Info);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Data", GUILayout.MaxWidth(60));

            _data = (MapEditorData) EditorGUILayout.ObjectField(_data, typeof(MapEditorData), false);

            if (GUILayout.Button("Load", GUILayout.MaxWidth(100)))
            {
                Load();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void Load()
        {
            _mapEditor = GameObject.FindObjectOfType<MapEditorModel>();

            _mapEditor.Data = _data;

            _mapEditor.Data.Grid.Bufferize();
        }

        private void CreateNew(string name)
        {
            string path = folder;

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

        private void DrawColorContents()
        {
            for (int i = 0; i < _data.Colors.Count; i++)
            {
                SerializedProperty property = _serializedData.FindProperty("Colors").GetArrayElementAtIndex(i);

                EditorGUILayout.PropertyField(property);
            }
        }
    }
}