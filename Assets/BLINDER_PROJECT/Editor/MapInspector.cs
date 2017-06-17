using System.Collections;
using System.Collections.Generic;
using BlinderProject;
using MapEditor;
using Pathfinding;
using Tools;
using ToolsEditor;
using UnityEditor;
using UnityEngine;

namespace BlinderProjectEditor
{
    [CustomEditor(typeof(MapInspector))]
    public class MapInspector : Editor
    {
        [DrawGizmo(GizmoType.NonSelected)]
        static void DrawGizmoNonSelected(Map map, GizmoType type)
        {
            DisplayGizmos(map);
        }

        [DrawGizmo(GizmoType.Selected)]
        static void DrawGizmo(Map map, GizmoType type)
        {
            DisplayGizmos(map);
        }

        static void DisplayGizmos(Map map)
        {
            NavGrid grid = map.GetNavGrid();

            if (grid == null)
                return;

            if (grid.DrawGrid)
            {
                DrawGizmoGraph(grid);
            }
        }

        static void DrawGizmoGraph(NavGrid grid)
        {
            for (int i = 0; i < grid.width; i++)
            {
                for (int j = 0; j < grid.height; j++)
                {
                    if (grid[i][j] != null)
                    {
                        DrawConnections(grid, grid[i][j].Coord);
                    }
                }
            }
        }

        static void DrawConnections(NavGrid grid, Vector2i coord)
        {
            Vector2i left = coord;
            left.x -= 1;

            if (grid.IsValidCoord(left) && grid.GetCaseAt(left) != null)
            {
                DrawLine(grid, coord, left);
            }

            Vector2i bot = coord;
            bot.y -= 1;

            if (grid.IsValidCoord(bot) && grid.GetCaseAt(bot) != null)
            {
                DrawLine(grid, coord, bot);
            }
        }

        static void DrawLine(NavGrid grid, Vector2i start, Vector2i end)
        {
            Vector2 startPosition = grid.GetCasePosition(start);
            Vector2 endPosition = grid.GetCasePosition(end);

            Gizmos.color = grid.Color;
            Gizmos.DrawLine(startPosition, endPosition);
        }
    }
}