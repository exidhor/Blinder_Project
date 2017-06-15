using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public class UnityGrid<T> : Grid<T>
    {
        public bool DrawGrid;

        public Vector2 Position;
        public Vector2 Size;
        public float CaseSize;

        public Color Color;

        public bool PointIsInGrid(Vector2 point)
        {
            float halfCaseSize = 0; // = CaseSize/2;
            Vector2 halfSize = Size/2;

            return !(point.x < Position.x - halfSize.x - halfCaseSize
                   || point.x > Position.x + halfSize.x  - halfCaseSize
                   || point.y < Position.y - halfSize.y - halfCaseSize
                   || point.y > Position.y + halfSize.y - halfCaseSize);
        }

        public Vector2i? GetCoordAt(Vector2 point)
        {
            if (!PointIsInGrid(point))
            {
                return null;
            }

            Vector2 halfSize = Size / 2;

            float dist_x = Mathf.Abs(Position.x - halfSize.x - point.x);
            float dist_y = Mathf.Abs(Position.y - halfSize.y - point.y);

            int x = (int)(dist_x / CaseSize);
            int y = (int)(dist_y / CaseSize);

            return new Vector2i(x, y);
        }

        public T GetCaseAt(Vector2i coord)
        {
            return this[coord.x][coord.y];
        }
    }
}
