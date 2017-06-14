using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace Pathfinding
{
    // source : http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
    public static class Heuristic
    {
        public static float SideCost = 1f;
        public static float DiagonalCost = Mathf.Sqrt(2);

        public static float ManhattanEstimation(Vector2i node, Vector2i goal)
        {
            int dx = Mathf.Abs(node.x - goal.x);
            int dy = Mathf.Abs(node.y - goal.y);

            return SideCost*(dx + dy);
        }

        public static float OctileEstimation(Vector2i node, Vector2i goal)
        {
            int dx = Mathf.Abs(node.x - goal.x);
            int dy = Mathf.Abs(node.y - goal.y);

            return SideCost * (dx + dy) + (DiagonalCost - 2 * SideCost) * Mathf.Min(dx, dy);
        }
    }
}