using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using MapEditor;
using ToolsEditor;
using UnityEngine;

namespace MapEditorEditor
{
    [CustomEditor(typeof(MapEditorModel))]
    class MapEditorModelInspector : Editor
    {
        [DrawGizmo(GizmoType.NonSelected)]
        static void DrawGizmoNonSelected(MapEditorModel mapEditorModel, GizmoType type)
        {
            DisplayGizmos(mapEditorModel);
        }

        [DrawGizmo(GizmoType.Selected)]
        static void DrawGizmo(MapEditorModel mapEditorModel, GizmoType type)
        {
            DisplayGizmos(mapEditorModel);
        }

        //void OnSceneGUI()
        //{
        //    MapEditorModel model = target as MapEditorModel;

        //    DrawCoord(model);
        //}

        private static void DrawCoord(MapEditorModel model)
        {
            Vector3 mousePosition = Event.current.mousePosition;
            mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
            mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
            mousePosition.y = -mousePosition.y;


            Vector2? mouseCoord = model.Data.Grid.GetCoordAt(mousePosition);

            string coord;

            Debug.Log("Drawn");

            if (mouseCoord.HasValue)
            {
                coord = "Coord : " + mouseCoord.Value.x + ", " + mouseCoord.Value.y;
            }
            else
            {
                coord = "Coord : " + "None";
            }

            Handles.BeginGUI();
            GUILayout.Label(coord);
            Handles.EndGUI();
        }

        static void DisplayGizmos(MapEditorModel mapEditorModel)
        {
            MapEditorData data = mapEditorModel.Data;

            if (data == null)
                return;

            if (data.Grid.DrawGrid)
            {
                DrawGizmosGrid(mapEditorModel);
            }

            if (data.DrawCase)
            {
                DrawGizmosCases(mapEditorModel);
            }
        }

        private static void DrawGizmosGrid(MapEditorModel model)
        {
            MapEditorData data = model.Data;

            Gizmos.color = data.Grid.Color;

            Vector2 position = model.transform.position;
            position -= data.Grid.Size / 2;

            Rect rect = new Rect(position, data.Grid.Size);

            Vector2 startLine = rect.min;
            Vector2 endLine = new Vector2(rect.xMin, rect.yMax);
            Vector2 step = new Vector2(data.Grid.CaseSize, 0);

            for (int i = 0; i <= model.Data.Grid.width; i++)
            {
                Gizmos.DrawLine(startLine, endLine);

                startLine += step;
                endLine += step;
            }

            startLine = rect.min;
            endLine = new Vector2(rect.xMax, rect.yMin);
            step = new Vector2(0, data.Grid.CaseSize);

            for (int i = 0; i <= model.Data.Grid.height; i++)
            {
                Gizmos.DrawLine(startLine, endLine);

                startLine += step;
                endLine += step;
            }
        }

        private static void DrawGizmosCases(MapEditorModel model)
        {
            float offset = 0.1f * model.Data.Grid.CaseSize;

            Vector3 gizmosCaseSize = new Vector3(model.Data.Grid.CaseSize - offset * 2, 
                model.Data.Grid.CaseSize - offset * 2, 
                0.1f);

            Vector3 firstPosition = model.transform.position;
            firstPosition.x -= (model.Data.Grid.width * model.Data.Grid.CaseSize) / 2 - model.Data.Grid.CaseSize / 2;
            firstPosition.y -= (model.Data.Grid.height * model.Data.Grid.CaseSize) / 2 - model.Data.Grid.CaseSize / 2;

            for (int i = 0; i < model.Data.Grid.width; i++)
            {
                for (int j = 0; j < model.Data.Grid.height; j++)
                {
                    //ECaseContent caseContent = (ECaseContent) model.Data.grid[i * model.Data.Grid.width + j];
                    ECaseContent caseContent = model.Data.Grid[i][j];
                    Gizmos.color = model.Data.Colors[(int) caseContent].Color;

                    Vector3 position = firstPosition;
                    position.x += i * model.Data.Grid.CaseSize;
                    position.y += j * model.Data.Grid.CaseSize;

                    Gizmos.DrawCube(position, gizmosCaseSize);   
                }  
            }
        }
    }
}
