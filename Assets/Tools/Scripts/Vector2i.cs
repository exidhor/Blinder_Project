using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public struct Vector2i
    {
        public int x;
        public int y;

        public Vector2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2i operator +(Vector2i c1, Vector2i c2)
        {
            return new Vector2i(c1.x + c2.x, c1.y + c2.y);
        }

        public static Vector2i operator -(Vector2i c1, Vector2i c2)
        {
            return new Vector2i(c1.x - c2.x, c1.y - c2.y);
        }

        public static Vector2 operator *(Vector2i c, float f)
        {
            return new Vector2(c.x * f, c.y * f);
        }

        public static Vector2 operator *(float f, Vector2i c)
        {
            return new Vector2(c.x * f, c.y * f);
        }

        public static implicit operator Vector2(Vector2i c)
        {
            return new Vector2(c.x, c.y);
        }

        public override string ToString()
        {
            return "Vector2i(" + x + ", " + y + ")";
        }
    }
}
