using System.Collections;
using System.Collections.Generic;
using BlinderProject;
using Pathfinding;
using UnityEditor;
using UnityEngine;

namespace BlinderProjectEditor
{
    [CustomEditor(typeof(MapNode))]
    public class MapNodeInspector : Editor
    {
        [DrawGizmo(GizmoType.NonSelected)]
        static void DrawGizmoNonSelected(MapNode mapNode, GizmoType type)
        {
            DisplayGizmos(mapNode);
        }

        [DrawGizmo(GizmoType.Selected)]
        static void DrawGizmo(MapNode mapNode, GizmoType type)
        {
            DisplayGizmos(mapNode);
        }

        static void DisplayGizmos(MapNode mapNode)
        {
            NodeGrid nodeGrid = mapNode.DEBUG_GetNodeGrid();

            if (mapNode.DrawGrid)
            {
                //DrawGizmosGrid(nodeGrid, mapNode.GridColor, mapNode.transform.position);
            }
        }

        //private static void DrawGizmosGrid(NodeGrid nodeGrid, Color gridColor, Vector2 position)
        //{
        //    Gizmos.color = gridColor;

        //    //Vector2 position = model.transform.position;
        //    position -= data.Bounds / 2;

        //    Rect rect = new Rect(position, data.Bounds);

        //    Vector2 startLine = rect.min;
        //    Vector2 endLine = new Vector2(rect.xMin, rect.yMax);
        //    Vector2 step = new Vector2(data.CaseSize, 0);

        //    for (int i = 0; i <= model.data.CaseCount.x; i++)
        //    {
        //        Gizmos.DrawLine(startLine, endLine);

        //        startLine += step;
        //        endLine += step;
        //    }

        //    startLine = rect.min;
        //    endLine = new Vector2(rect.xMax, rect.yMin);
        //    step = new Vector2(0, data.CaseSize);

        //    for (int i = 0; i <= model.data.CaseCount.x; i++)
        //    {
        //        Gizmos.DrawLine(startLine, endLine);

        //        startLine += step;
        //        endLine += step;
        //    }
        //}

        //private static void DrawGizmosCases(MapEditorModel model)
        //{
        //    float offset = 0.5f;

        //    Vector3 gizmosCaseSize = new Vector3(model.data.CaseSize - offset * 2,
        //        model.data.CaseSize - offset * 2,
        //        0.1f);

        //    Vector3 firstPosition = model.transform.position;
        //    firstPosition.x -= (model.data.CaseCount.x * model.data.CaseSize) / 2 - model.data.CaseSize / 2;
        //    firstPosition.y -= (model.data.CaseCount.y * model.data.CaseSize) / 2 - model.data.CaseSize / 2;

        //    for (int i = 0; i < model.data.CaseCount.x; i++)
        //    {
        //        for (int j = 0; j < model.data.CaseCount.y; j++)
        //        {
        //            ECaseContent caseContent = (ECaseContent)model.data.grid[i * model.data.CaseCount.x + j];
        //            Gizmos.color = model.data.Colors[(int)caseContent].Color;

        //            Vector3 position = firstPosition;
        //            position.x += i * model.data.CaseSize;
        //            position.y += j * model.data.CaseSize;

        //            Gizmos.DrawCube(position, gizmosCaseSize);
        //        }
        //    }
        //}
    }
}
