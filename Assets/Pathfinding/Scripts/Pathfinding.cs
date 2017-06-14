using System.Collections.Generic;
using System.Linq;
using Tools;

namespace Pathfinding
{
    public static class PathFinding
    {
        public static List<Node> Classical_A_Star(NodeGrid grid, Node start, Node goal, HeuristicType heuristicType)
        {
            grid.ClearReccords();

            PriorityQueue<Node, float> frontier = new PriorityQueue<Node, float>();
            start.EstimatedTotalCost = Estimate(start, goal, heuristicType);

            EnqueueNode(start, frontier);

            while (frontier.Count > 0)
            {
                Node current = frontier.Dequeue().Value;
                current.State = NodeRecordState.Closed;

                if (current == goal)
                {
                    break;
                }

                List<Connection> connections = current.Connections.GetConnections();

                for (int i = 0; i < connections.Count; i++)
                {
                    float newCost = current.CostSoFar + connections[i].Cost;

                    Node connectedNode = connections[i].GetOtherNode(current);

                    if (connectedNode.State == NodeRecordState.Unvisited
                        || newCost < connectedNode.CostSoFar)
                    {
                        connectedNode.CostSoFar = newCost;
                        connectedNode.EstimatedTotalCost = newCost + Estimate(connectedNode, goal, heuristicType);
                        EnqueueNode(connectedNode, frontier);
                        connectedNode.CameFrom = current;
                    }
                }
            }

            return ReconstructPath(goal, start);
        }

        private static void EnqueueNode(Node node, PriorityQueue<Node, float> frontier)
        {
            //Debug.Log("Enqueue " + node);
            frontier.Enqueue(node, -node.EstimatedTotalCost);
            node.State = NodeRecordState.Open;
        }

        private static List<Node> ReconstructPath(Node goal, Node start)
        {
            List<Node> path = new List<Node>();

            Node current = goal;

            while (current != null)
            {
                path.Add(current);

                current = current.CameFrom;
            }

            return path;
        }

        public static void A_Star(NodeGrid grid, Node start, Node goal, HeuristicType heuristicType)
        {
            // Initialize the record for the start node
            start.CostSoFar = 0;
            start.EstimatedTotalCost = Estimate(start, goal, heuristicType);

            // Initialize the open list
            List<Node> openList = new List<Node>();
            openList.Add(start);
            start.State = NodeRecordState.Open;

            //Iterate through processing each node
            while (openList.Count > 0)
            {
                // Find the smallest element in the open list
                // using the estimated total cost
                Node current = GetSmallest(openList);

                // if it is the goal node, then terminate
                if (current == goal)
                {
                    break;
                }

                // Otherwise get its outgoing connections
                List<Connection> connections = current.Connections.GetConnections();

                // Loop through each connection in turn 
                for (int i = 0; i < connections.Count; i++)
                {
                    // Get the cost estimate for the end node
                    float connectedNodeCost = current.CostSoFar + connections[i].Cost;
                    float connectedNodeHeuristic = 0;

                    Node connectedNode = connections[i].GetOtherNode(current);

                    // if the node is closed we may have to skip, or remove the closed state
                    if (connectedNode.State == NodeRecordState.Closed)
                    {
                        // if we didn't find a shorter route, skip 
                        if (connectedNode.CostSoFar <= connectedNodeCost)
                        {
                            continue;
                        }

                        // otherwise remove the closed state
                        connectedNode.State = NodeRecordState.Unvisited;

                        // we can use the node's old cost values to calculate its
                        // heuristic whitout calling the possibly expensive heuristic function
                        connectedNodeHeuristic = connectedNode.EstimatedTotalCost - connectedNode.CostSoFar;
                    }

                    // Skip if the node is open and we've not found a better
                    // route
                    else if (connectedNode.State == NodeRecordState.Open)
                    {
                        // if our route is no better, then skip
                        if (connectedNode.CostSoFar <= connectedNodeCost)
                        {
                            continue;
                        }

                        // we can use the node's old cost values to calculate its
                        // heuristic whitout calling the possibly expensive heuristic function
                        connectedNodeHeuristic = connectedNode.EstimatedTotalCost - connectedNode.CostSoFar;
                    }

                    // otherwise we know we've got an unvisited node
                    else
                    {
                        // we'll need to calculate the heuristic value using the function
                        // since we don't have an existing record to use
                        connectedNodeHeuristic = Estimate(connectedNode, goal, heuristicType);
                    }

                    // we're here if we need to update the node, update the cost and estimate
                    connectedNode.CostSoFar = connectedNodeCost;
                    connectedNode.EstimatedTotalCost = connectedNodeCost + connectedNodeHeuristic;
                }
            }
        }

        private static float Estimate(Node node, Node goal, HeuristicType type)
        {
            switch (type)
            {
                case HeuristicType.ManhattanDistance:
                    return Heuristic.ManhattanEstimation(node.Coord, goal.Coord);

                case HeuristicType.OctileDistance:
                    return Heuristic.OctileEstimation(node.Coord, goal.Coord);

                default:
                    return Heuristic.ManhattanEstimation(node.Coord, goal.Coord);
            }
        }

        private static Node GetSmallest(List<Node> openList)
        {
            Node smallest = openList.Last();
            openList.RemoveAt(openList.Count - 1);

            return smallest;
        }

        private static void SortOpenList(List<Node> openList)
        {
            openList.Sort((x, y) => y.EstimatedTotalCost.CompareTo(x.EstimatedTotalCost));
        }
    }
}