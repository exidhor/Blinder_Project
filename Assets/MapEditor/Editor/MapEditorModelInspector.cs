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
            DisplayGizmos(mapEditorModel);
        }

        [DrawGizmo(GizmoType.Selected)]
        static void DrawGizmo(MapEditorModel mapEditorModel, GizmoType type)
        {
            DisplayGizmos(mapEditorModel);
        }

        static void DisplayGizmos(MapEditorModel mapEditorModel)
        {
            MapEditorData data = mapEditorModel.data;

            if (data.DrawGrid)
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
            MapEditorData data = model.data;

            Gizmos.color = data.GridColor;

            Vector2 position = model.transform.position;
            position -= data.Bounds / 2;

            Rect rect = new Rect(position, data.Bounds);

            Vector2 startLine = rect.min;
            Vector2 endLine = new Vector2(rect.xMin, rect.yMax);
            Vector2 step = new Vector2(data.CaseSize, 0);

            for (int i = 0; i <= model.caseCount.x; i++)
            {
                Gizmos.DrawLine(startLine, endLine);

                startLine += step;
                endLine += step;
            }

            startLine = rect.min;
            endLine = new Vector2(rect.xMax, rect.yMin);
            step = new Vector2(0, data.CaseSize);

            for (int i = 0; i <= model.caseCount.x; i++)
            {
                Gizmos.DrawLine(startLine, endLine);

                startLine += step;
                endLine += step;
            }
        }

        private static void DrawGizmosCases(MapEditorModel model)
        {
            float offset = 0.5f;

            Vector3 gizmosCaseSize = new Vector3(model.data.CaseSize - offset * 2, 
                model.data.CaseSize - offset * 2, 
                0.1f);

            Vector3 firstPosition = model.transform.position;
            firstPosition.x -= (model.caseCount.x * model.data.CaseSize) / 2 - model.data.CaseSize / 2;
            firstPosition.y -= (model.caseCount.y * model.data.CaseSize) / 2 - model.data.CaseSize / 2;

            for (int i = 0; i < model.caseCount.x; i++)
            {
                for (int j = 0; j < model.caseCount.y; j++)
                {
                    ECaseContent caseContent = (ECaseContent) model.data.grid[i * model.caseCount.x + j];
                    Gizmos.color = model.data.Colors[(int) caseContent].Color;

                    Vector3 position = firstPosition;
                    position.x += i * model.data.CaseSize;
                    position.y += j * model.data.CaseSize;

                    Gizmos.DrawCube(position, gizmosCaseSize);   
                }  
            }
        }
    }
}
