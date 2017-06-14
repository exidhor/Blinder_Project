using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinding
{
    public enum ConnectionIndex
    {
        Right,
        Top,
        TopLeft,
        BotLeft,


        // the opposite
        Left,
        Bottom,
        BotRight,
        TopRight
    }
}
