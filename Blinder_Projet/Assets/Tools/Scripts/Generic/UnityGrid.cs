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

        [SerializeField, UnityReadOnly] private Vector2 _halfSize;
        [SerializeField, UnityReadOnly] private float _halfCaseSize;
        [SerializeField, UnityReadOnly] private float _caseInverse;

        public override void ResizeFrom<U>(Grid<U> otherGrid)
        {
            base.ResizeFrom(otherGrid);

            UnityGrid<U> otherUnityGrid = otherGrid as UnityGrid<U>;

            if (otherUnityGrid != null)
            {
                Position = otherUnityGrid.Position;
                Size = otherUnityGrid.Size;
                CaseSize = otherUnityGrid.CaseSize;
                Color = otherUnityGrid.Color;
            }

            Bufferize();
        }

        public void Bufferize()
        {
            _halfSize = Size/2;
            _halfCaseSize = CaseSize/2;
            _caseInverse = 1/CaseSize;
        }

        public bool PointIsInGrid(Vector2 point)
        {
            return !(point.x < Position.x - _halfSize.x
                     || point.x > Position.x + _halfSize.x
                     || point.y < Position.y - _halfSize.y
                     || point.y > Position.y + _halfSize.y);
        }

        public Vector2i? GetCoordAt(float x, float y)
        {
            return GetCoordAt(new Vector2(x, y));
        }

        public Vector2i? GetCoordAt(Vector2 point)
        {
            if (!PointIsInGrid(point))
            {
                return null;
            }

            float dist_x = Mathf.Abs(Position.x - _halfSize.x - point.x);
            float dist_y = Mathf.Abs(Position.y - _halfSize.y - point.y);

            int x = (int) (dist_x * _caseInverse);
            int y = (int) (dist_y * _caseInverse);

            return new Vector2i(x, y);
        }

        public Vector2 GetCasePosition(Vector2i coord)
        {
            return GetCasePosition(coord.x, coord.y);
        }

        public Vector2 GetCasePosition(int x, int y)
        {
            return new Vector2(x*CaseSize + _halfCaseSize - Position.x - _halfSize.x,
                y*CaseSize + _halfCaseSize - Position.y - _halfSize.y);
        }
    }
}