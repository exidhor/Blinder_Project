using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace Pathfinding
{
    [Serializable]
    public struct WeightedNode
    {
        public NodeRecord Node;
        public float ImmediateCost;

        public WeightedNode(NodeRecord node, float immediateCost)
        {
            Node = node;
            ImmediateCost = immediateCost;
        }
    }
}
