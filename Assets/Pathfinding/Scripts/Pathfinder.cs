using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace Pathfinding
{
    public static class Pathfinder
    {
        private static List<NodeRecord> _neighbourList = new List<NodeRecord>(8);

        public static List<Vector2i> Classical_A_Star(NavGrid grid, Vector2i coordStart, Vector2i coordGoal, EHeuristicType heuristicType)
        {
            // find the start
            NodeRecord start = grid.GetCaseAt(coordStart);

            // verify if it's in the grid
            if (start == null)
            {
                return new List<Vector2i>();
            }

            // prepare the record
            grid.ClearRecords();
            PriorityQueue<NodeRecord, float> frontier = new PriorityQueue<NodeRecord, float>();

            start.EstimatedTotalCost = Estimate(coordStart, coordGoal, heuristicType);

            EnqueueNodeRecord(start, frontier);

            NodeRecord endNode = null;

            while (frontier.Count > 0)
            {
                NodeRecord current = frontier.Dequeue().Value;
                current.State = ENodeRecordState.Closed;

                if (current.Coord == coordGoal)
                {
                    endNode = current;
                    break;
                }

                _neighbourList = grid.GetNeighbour(current.Coord);

                for (int i = 0; i < _neighbourList.Count; i++)
                {
                    float newCost = current.CostSoFar + _neighbourList[i].NodeCost;

                    if (_neighbourList[i].State == ENodeRecordState.Unvisited
                        || newCost < _neighbourList[i].CostSoFar)
                    {
                        _neighbourList[i].CostSoFar = newCost;
                        _neighbourList[i].EstimatedTotalCost = newCost + Estimate(_neighbourList[i].Coord, coordGoal, heuristicType);
                        EnqueueNodeRecord(_neighbourList[i], frontier);
                        _neighbourList[i].CameFrom = current;
                    }
                }
            }

            return ReconstructPath(endNode);
        }

        private static void EnqueueNodeRecord(NodeRecord node, PriorityQueue<NodeRecord, float> frontier)
        {
            //Debug.Log("Enqueue " + node);
            frontier.Enqueue(node, -node.EstimatedTotalCost);
            node.State = ENodeRecordState.Open;
        }

        private static List<Vector2i> ReconstructPath(NodeRecord end)
        {
            List<Vector2i> path = new List<Vector2i>();

            NodeRecord current = end;

            while (current != null)
            {
                path.Add(current.Coord);

                current = current.CameFrom;
            }

            return path;
        }

        private static float Estimate(Vector2i from, Vector2i to, EHeuristicType type)
        {
            switch (type)
            {
                case EHeuristicType.ManhattanDistance:
                    return Heuristic.ManhattanEstimation(from, to);

                case EHeuristicType.OctileDistance:
                    return Heuristic.OctileEstimation(from, to);

                default:
                    return Heuristic.ManhattanEstimation(from, to);
            }
        }
    }
}
