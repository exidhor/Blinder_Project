using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace Pathfinding
{
    // not used yet, WIP

    [Serializable]
    public class CoordQuadTree : QuadTree<Vector2i>
    {
        public CoordQuadTree(Rect bounds) : base(bounds)
        {
        }
    }
}
