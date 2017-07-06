using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapEditor;
using Tools;
using UnityEngine;

namespace ToolsEditor
{
    public static class GridDrawer
    {
        public static void DrawGizmosGrid<T>(UnityGrid<T> grid)
        {
            Gizmos.color = grid.Color;

            Rect rect = new Rect(grid.Position - grid.WorldSize / 2, grid.WorldSize);

            Vector2 startLine = rect.min;
            Vector2 endLine = new Vector2(rect.xMin, rect.yMax);
            Vector2 step = new Vector2(grid.CaseSize, 0);

            for (int i = 0; i <= grid.width; i++)
            {
                Gizmos.DrawLine(startLine, endLine);

                startLine += step;
                endLine += step;
            }

            startLine = rect.min;
            endLine = new Vector2(rect.xMax, rect.yMin);
            step = new Vector2(0, grid.CaseSize);

            for (int i = 0; i <= grid.height; i++)
            {
                Gizmos.DrawLine(startLine, endLine);

                startLine += step;
                endLine += step;
            }
        }
    }
}
