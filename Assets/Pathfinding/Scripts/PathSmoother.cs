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
            // to keep the start and the end nodes in the smoothed path
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

                // If it's true, this means that we found a shortcut, then we can
                // directly add the found index
                if (bestClearIndex > 0)
                {
                    smoothPath.Add(path[bestClearIndex]);

                    // we want to restart at the last added node
                    // -1 : to count the for-incrementation
                    i = bestClearIndex - 1;
                }
                else // if (i < path.Count - 3)
                {
                    // no shortcut found, we add the immediate next node
                    smoothPath.Add(path[i + 1]);
                }
            }

            // The last node doesn't need to be add because it's necessarily added to the smoothed path.
            // When i = path.count - 2, j = i + 2
            //  then j > path.count
            //      then bestclearindex still is -1
            //          then we add the i + 1 = path.count - 1 which is the last node
            //              [end of the demonstration]
            
            return smoothPath;
        } 
    }
}
