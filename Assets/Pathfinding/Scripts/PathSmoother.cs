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
        public static List<Vector2i> SmoothPath(List<Vector2i> path)
        {
            List<Vector2i> smoothPath = new List<Vector2i>();

            if (path.Count == 0)
            {
                return smoothPath;
            }

            // To keep the current path structure, we need
            // to keep the start and the end.
            smoothPath.Add(path[0]);

            NavGrid navGrid = Map.instance.navGrid;

            // we run through each node of the previous path
            for (int i = 0; i < path.Count - 1; i++)
            {
                int bestClearIndex = -1;

                // And compare them with the next nodes
                // (we don't need to check the immediate next node
                // because, thank's to the grid structure, it's necessarily
                // reachable)
                for (int j = i + 2; j < path.Count; j++)
                {
                    // if we can't find a clear line bewteen them, we stop the process
                    if (!navGrid.IsClearLine(path[i], path[j]))
                    {
                        break;
                    }

                    bestClearIndex = j;
                }

                // We don't want the last clear line, because it means that it's
                // really close to an obstacle.
                // In place, we want the previous last clear line, to be sure that
                // the character can easly reach it
                // bestClearIndex--;

                // This mean that the actual index point out a next node, and
                // node the current node or a previous node.
                if (bestClearIndex > i)
                {
                    smoothPath.Add(path[bestClearIndex]);

                    // we want to try start at the last added node
                    i = bestClearIndex - 1;
                }
                else // if (i < path.Count - 3)
                {
                    // no shortcut found, we add the immediate next node
                    smoothPath.Add(path[i + 1]);
                }
            }

            // we add the last node to be sure that the smmoth path has
            // the same destination as the previous path
            //smoothPath.Add(path.Last());
            
            return smoothPath;
        } 
    }
}
