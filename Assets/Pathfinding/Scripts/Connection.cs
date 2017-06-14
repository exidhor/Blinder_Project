using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinding
{
    public class Connection
    {
        public float Cost;
        public Node Node0;
        public Node Node1;

        public Connection(Node node0, Node node1, float cost)
        {
            Node0 = node0;
            Node1 = node1;
            Cost = cost;
        }

        public Node GetOtherNode(Node node)
        {
            if (node != Node0)
            {
                return Node0;
            }

            return Node1;
        }

        public static int GetOppositeIndex(int connectionIndex)
        {
            return (connectionIndex + 4) % 8;
        }
    }
}
