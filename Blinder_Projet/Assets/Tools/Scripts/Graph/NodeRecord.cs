using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    /// <summary>
    /// A buffer class used to course the GridNode quickly,
    /// for things like pathfinding.
    /// </summary>
    [Serializable]
    public class NodeRecord
    {
        // The coord of the node into the grid
        public Vector2i Coord;

        // "NodeCost" is used to modify the cost needed to cross the node.
        // It's not the connection cost, which is computed in the grid.
        public float NodeCost; 
                              
        // The cost from the start node to this point
        public float CostSoFar;

        // The cost from the start passing by this node, and finishing to the goal node
        public float EstimatedTotalCost;

        // The best previous node
        public NodeRecord CameFrom;

        // The sate of the node is relative of the algorithm curse
        public ENodeRecordState State;

        /// <summary>
        /// Construct the NodeRecord and init it data
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="nodeCost"></param>
        public NodeRecord(Vector2i coord, float nodeCost = 0f)
        {
            Coord = coord;

            NodeCost = nodeCost;
            CostSoFar = 0;
            EstimatedTotalCost = 0;
            CameFrom = null;

            State = ENodeRecordState.Unvisited;
        }

        /// <summary>
        /// Reset the data to it initial value
        /// </summary>
        public void ClearReccords()
        {
            CostSoFar = 0;
            EstimatedTotalCost = 0;
            CameFrom = null;

            State = ENodeRecordState.Unvisited;
        }

        public override string ToString()
        {
            return "NodeRecord (" + Coord.x + ", " + Coord.y + ")";
        }
    }
}
