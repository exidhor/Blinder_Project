using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    [Serializable]
    public class NodeRecord
    {
        public Vector2i Coord;

        public float NodeCost;
        public float CostSoFar;
        public float EstimatedTotalCost;
        public NodeRecord CameFrom;

        public ENodeRecordState State;

        public NodeRecord(Vector2i coord, float nodeCost = 1f)
        {
            Coord = coord;

            NodeCost = nodeCost;
            CostSoFar = 0;
            EstimatedTotalCost = 0;
            CameFrom = null;

            State = ENodeRecordState.Unvisited;
        }

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
