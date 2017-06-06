using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using MapEditor;
using UnityEngine;

namespace MapEditorEditor
{
    [CustomEditor(typeof(MapEditorModel))]
    class MapEditorModelInspector : Editor
    {
        [DrawGizmo(GizmoType.NonSelected)]
        static void DrawGizmoNonSelected(MapEditorModel mapEditorModel, GizmoType type)
        {
            DrawGizmosGrid(mapEditorModel);
        }

        [DrawGizmo(GizmoType.Selected)]
        static void DrawGizmo(MapEditorModel mapEditorModel, GizmoType type)
        {
            DrawGizmosGrid(mapEditorModel);
        }

        void OnSceneGUI()
        {
            if (Event.current.type == EventType.Repaint)
            {
                DrawHandlesGrid((MapEditorModel)target);
            }
        }

        private static void DrawGizmosGrid(MapEditorModel model)
        {
            MapEditorData data = model.data;

            if (data.DrawGrid)
            {
                Gizmos.color = data.GridColor;

                Vector2 position = model.transform.position;
                position -= data.Bounds / 2;

                Rect rect = new Rect(position, data.Bounds);

                Vector2 startLine = rect.min;
                Vector2 endLine = new Vector2(rect.xMin, rect.yMax);
                Vector2 step = new Vector2(data.CaseSize.x, 0);

                for (int i = 0; i <= model.caseCount.x; i++)
                {
                    Gizmos.DrawLine(startLine, endLine);

                    startLine += step;
                    endLine += step;
                }

                startLine = rect.min;
                endLine = new Vector2(rect.xMax, rect.yMin);
                step = new Vector2(0, data.CaseSize.y);

                for (int i = 0; i <= model.caseCount.x; i++)
                {
                    Gizmos.DrawLine(startLine, endLine);

                    startLine += step;
                    endLine += step;
                }
            }
        }

        private static void DrawHandlesGrid(MapEditorModel model)
        {
            for (int i = 0; i < model.caseCount.x; i++)
            {
                for (int j = 0; j < model.caseCount.y; j++)
                {
                    ECaseContent caseContent = model.grid[i * model.caseCount.x + j];
                    Color color = model.data.Colors[(int) caseContent].Color;

                    //DrawHandleCase(caseContent, model.transform.position, i, j, color);       
                }  
            }
        }

        private static void DrawHandleCase(ECaseContent caseContent, Vector3 position, Quaternion rotation,
            int x, int y, Color color)
        {
            Handles.color = color;

            Handles.RectangleHandleCap(
                0,
                position,
                rotation,
                1f,
                EventType.Repaint
                );
        }
    }
}
