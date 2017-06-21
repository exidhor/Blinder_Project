using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapEditor;
using Tools;
using UnityEngine;

namespace Pathfinding
{
    public static class PathSmoother
    {
        public static List<Vector2> SmoothPath(List<Vector2> path)
        {
            List<Vector2> smoothPath = new List<Vector2>();

            if (path.Count == 0)
            {
                return smoothPath;
            }

            smoothPath.Add(path[0]);

            NavGrid navGrid = Map.instance.navGrid;

            for (int i = 1; i < path.Count - 1; i++)
            {
                if (!navGrid.IsClearLine(smoothPath.Last(), path[i]))
                {
                    smoothPath.Add(path[i]);
                }
            }

            smoothPath.Add(path.Last());
            
            return smoothPath;
        } 
    }
}
