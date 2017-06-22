using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapEditor;
using Tools;
using UnityEngine;

namespace Pathfinding
{
    public static class Pathfinder
    {
        private static List<WeightedNode> _neighbourList = new List<WeightedNode>(8);

        private static NavGrid _grid;

        private static Vector2 _floatCoordGoal;

        public static List<Vector2i> A_Star(Vector2 startPosition, Vector2 goalPosition, EHeuristicType heuristicType = EHeuristicType.OctileDistance)
        {
            _grid = Map.instance.navGrid;

            // find the start
            Vector2i? startCoord = _grid.GetCoordAt(startPosition);
            Vector2i? goalCoord = _grid.GetCoordAt(goalPosition);

            if (!startCoord.HasValue || !goalCoord.HasValue)
            {
                return new List<Vector2i>();
            }

            NodeRecord start = _grid.GetCaseAt(startCoord.Value);

            // verify if it's in the grid
            if (start == null)
            {
                return new List<Vector2i>();
            }

            // buffer this because it will be used for each estimation
            _floatCoordGoal = GetFloatCoordFrom(goalPosition);

            // prepare the record
            _grid.ClearRecords();
            PriorityQueue<NodeRecord, float> frontier = new PriorityQueue<NodeRecord, float>();

            start.EstimatedTotalCost = EstimatePosition(startPosition, heuristicType);

            EnqueueNodeRecord(start, frontier);

            NodeRecord endNode = null;

            while (frontier.Count > 0)
            {
                NodeRecord current = frontier.Dequeue().Value;
                current.State = ENodeRecordState.Closed;

                if (current.Coord == goalCoord)
                {
                    endNode = current;
                    break;
                }

                _neighbourList = _grid.GetNeighbour(current.Coord);

                for (int i = 0; i < _neighbourList.Count; i++)
                {
                    NodeRecord currentNeighbour = _neighbourList[i].Node;


                    float newCost = current.CostSoFar + _neighbourList[i].ImmediateCost + currentNeighbour.NodeCost;

                    if (currentNeighbour.State == ENodeRecordState.Unvisited
                        ||  currentNeighbour.CostSoFar > newCost)
                    {
                        float estimateCost = EstimateCoord(currentNeighbour.Coord, heuristicType);

                        currentNeighbour.CostSoFar = newCost;
                        currentNeighbour.EstimatedTotalCost = newCost + estimateCost;
                        EnqueueNodeRecord(currentNeighbour, frontier);
                        currentNeighbour.CameFrom = current;
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

            if (end != null)
            {
                NodeRecord current = end.CameFrom;

                while (current != null && current.CameFrom != null)
                {
                    path.Add(current.Coord);

                    current = current.CameFrom;
                }
            }

            path.Reverse();

            return path;
        }

        private static Vector2 GetFloatCoordFrom(Vector2 worldPosition)
        {
            Vector2i coord = _grid.GetCoordAt(worldPosition).Value;
            Vector2 casePosition = _grid.GetCasePosition(coord);

            Vector2 delta = worldPosition - casePosition;
            delta /= _grid.CaseSize;

            return coord + delta;
        }

        private static float EstimatePosition(Vector2 from, EHeuristicType type)
        {
            Vector2 floatCoord = GetFloatCoordFrom(from);

            return Estimate(floatCoord, type);
        }

        private static float EstimateCoord(Vector2i from, EHeuristicType type)
        {
            return Estimate(from, type);
        }

        private static float Estimate(Vector2 floatCoordFrom, EHeuristicType type)
        {
            switch (type)
            {
                case EHeuristicType.ManhattanDistance:
                    return Heuristic.ManhattanEstimation(floatCoordFrom, _floatCoordGoal);

                case EHeuristicType.OctileDistance:
                    return Heuristic.OctileEstimation(floatCoordFrom, _floatCoordGoal);

                default:
                    return Heuristic.ManhattanEstimation(floatCoordFrom, _floatCoordGoal);
            }
        }
    }
}
