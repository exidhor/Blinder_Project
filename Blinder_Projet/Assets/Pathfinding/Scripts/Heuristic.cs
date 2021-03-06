﻿using UnityEngine;

namespace Pathfinding
{
    // source : http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
    public static class Heuristic
    {
        public static float SideCost = 1f;
        public static float DiagonalCost = Mathf.Sqrt(2);

        public static float ManhattanEstimation(Vector2 node, Vector2 goal)
        {
            float dx = Mathf.Abs(node.x - goal.x);
            float dy = Mathf.Abs(node.y - goal.y);

            return SideCost * (dx + dy);
        }

        public static float OctileEstimation(Vector2 node, Vector2 goal)
        {
            float dx = Mathf.Abs(node.x - goal.x);
            float dy = Mathf.Abs(node.y - goal.y);

            return SideCost * (dx + dy) + (DiagonalCost - 2 * SideCost) * Mathf.Min(dx, dy);
        }
    }
}