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

        static void DisplayGizmos(MapEditorModel mapEditorModel)
        {
            MapEditorData data = mapEditorModel.Data;

            if (data == null)
                return;

            if (data.Grid.DrawGrid)
            {
                GridDrawer.DrawGizmosGrid(mapEditorModel.Data.Grid);
            }

            if (data.DrawCase)
            {
                DrawGizmosCases(mapEditorModel);
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
