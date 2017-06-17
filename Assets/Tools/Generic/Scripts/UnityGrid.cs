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

        public override void Copy<U>(Grid<U> otherGrid)
        {
            base.Copy(otherGrid);

            UnityGrid<U> otherUnityGrid = otherGrid as UnityGrid<U>;

            if (otherUnityGrid != null)
            {
                Position = otherUnityGrid.Position;
                Size = otherUnityGrid.Size;
                CaseSize = otherUnityGrid.CaseSize;
                Color = otherUnityGrid.Color;
            }
        }

        public bool PointIsInGrid(Vector2 point)
        {
            float halfCaseSize = 0; // = CaseSize/2;
            Vector2 halfSize = Size/2;

            return !(point.x < Position.x - halfSize.x - halfCaseSize
                     || point.x > Position.x + halfSize.x - halfCaseSize
                     || point.y < Position.y - halfSize.y - halfCaseSize
                     || point.y > Position.y + halfSize.y - halfCaseSize);
        }

        public Vector2i? GetCoordAt(Vector2 point)
        {
            if (!PointIsInGrid(point))
            {
                return null;
            }

            Vector2 halfSize = Size/2;

            float dist_x = Mathf.Abs(Position.x - halfSize.x - point.x);
            float dist_y = Mathf.Abs(Position.y - halfSize.y - point.y);

            int x = (int) (dist_x/CaseSize);
            int y = (int) (dist_y/CaseSize);

            return new Vector2i(x, y);
        }

        public Vector2 GetCasePosition(Vector2i coord)
        {
            return GetCasePosition(coord.x, coord.y);
        }

        public Vector2 GetCasePosition(int x, int y)
        {
            float halfCaseSize = CaseSize/2;

            return new Vector2(x*CaseSize + halfCaseSize - Position.x - Size.x/2,
                y*CaseSize + halfCaseSize - Position.y - Size.y/2);
        }
    }
}