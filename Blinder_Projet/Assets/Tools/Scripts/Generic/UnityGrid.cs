using System;
using UnityEngine;

namespace Tools
{
    /// <summary>
    /// This is a grid with a case size.
    /// This allows to compute collisions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class UnityGrid<T> : Grid<T>
    {
        public bool DrawGrid;

        /// <summary>
        /// The center of the grid
        /// </summary>
        public Vector2 Position;
        public Vector2 Size;
        public float CaseSize;

        public Color Color;

        [SerializeField, UnityReadOnly] private Vector2 _halfSize;
        [SerializeField, UnityReadOnly] private float _halfCaseSize;
        [SerializeField, UnityReadOnly] private float _caseInverse;

        /// <summary>
        /// Resize the grid, and make it the same as the model.
        /// This metohd doesn't copy the case content, only the grid structure
        /// (size, width, etc ...).
        /// Thank's to polymorphism, this can copy Grid or UnityGrid.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="otherGrid">The grid model</param>
        public override void ResizeFrom<U>(Grid<U> otherGrid)
        {
            base.ResizeFrom(otherGrid);

            UnityGrid<U> otherUnityGrid = otherGrid as UnityGrid<U>;

            // if the grid is a UnityGrid, then the ref is not null
            if (otherUnityGrid != null)
            {
                Position = otherUnityGrid.Position;
                Size = otherUnityGrid.Size;
                CaseSize = otherUnityGrid.CaseSize;
                Color = otherUnityGrid.Color;
            }

            // we dont forget to reload the buffer
            Bufferize();
        }

        /// <summary>
        /// The precompute some divisions to optimize
        /// often called methods.
        /// </summary>
        public void Bufferize()
        {
            _halfSize = Size/2;
            _halfCaseSize = CaseSize/2;
            _caseInverse = 1/CaseSize;
        }

        /// <summary>
        /// Check if the point is inside the grid or not
        /// </summary>
        /// <param name="x">The abs of the point</param>
        /// <param name="y">the ord of the point</param>
        /// <returns>True if the point if inside the grid,
        /// false otherwise.</returns>
        public bool PointIsInGrid(float x, float y)
        {
            return !(x < Position.x - _halfSize.x
                     || x > Position.x + _halfSize.x
                     || y < Position.y - _halfSize.y
                     || y > Position.y + _halfSize.y);
        }

        /// <summary>
        /// Check if the point is inside the grid or not
        /// </summary>
        /// <param name="point"></param>
        /// <returns>True if the point if inside the grid,
        /// false otherwise.</returns>
        public bool PointIsInGrid(Vector2 point)
        {
            return PointIsInGrid(point.x, point.y);
        }

        /// <summary>
        /// Try to find the case coordinate where the point is.
        /// This method check if the point is inside the grid,
        /// and return null if not. This operator can be avoid
        /// by calling <see cref="GetCoordAt_WithoutCheck(float,float)"/>
        /// </summary>
        /// <param name="x">The abs of the point</param>
        /// <param name="y">the ord of the point</param>
        /// <returns></returns>
        public Vector2i? GetCoordAt(float x, float y)
        {
            if (!PointIsInGrid(x, y))
            {
                return null;
            }

            return GetCoordAt_WithoutCheck(x, y);
        }

        /// <summary>
        /// Try to find the case coordinate where the point is.
        /// This method check if the point is inside the grid,
        /// and return null if not. This operator can be avoid
        /// by calling <see cref="GetCoordAt_WithoutCheck(float,float)"/>
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2i? GetCoordAt(Vector2 point)
        {
            return GetCoordAt(point.x, point.y);
        }

        /// <summary>
        /// [UNSAFE] Quick method to get the case coordinate where the point is.
        /// This method is unsafe because it doesn't check if the point is inside the grid.
        /// To get the safe method, call <see cref="GetCoordAt(float,float)"/>
        /// </summary>
        /// <param name="x">The abs of the point</param>
        /// <param name="y">the ord of the point</param>
        /// <returns></returns>
        public Vector2i GetCoordAt_WithoutCheck(float x, float y)
        {
            float dist_x = Mathf.Abs(Position.x - _halfSize.x - x);
            float dist_y = Mathf.Abs(Position.y - _halfSize.y - y);

            int coord_x = (int)(dist_x * _caseInverse);
            int coord_y = (int)(dist_y * _caseInverse);

            return new Vector2i(coord_x, coord_y);
        }

        /// <summary>
        /// [UNSAFE] Quick method to get the case coordinate where the point is.
        /// This method is unsafe because it doesn't check if the point is inside the grid.
        /// To get the safe method, call <see cref="GetCoordAt(float,float)"/>
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2i GetCoordAt_WithoutCheck(Vector2 position)
        {
            return GetCoordAt_WithoutCheck(position.x, position.y);
        }

        /// <summary>
        /// Return the case position (it center)
        /// </summary>
        /// <param name="coord">The case coordinate</param>
        /// <returns></returns>
        public Vector2 GetCasePosition(Vector2i coord)
        {
            return GetCasePosition(coord.x, coord.y);
        }

        public Vector2i GetClosestCoord(Vector2 point)
        {
            if (PointIsInGrid(point))
            {
                return GetCoordAt_WithoutCheck(point);
            }

            Vector2 closestPointInGrid = MathHelper.ClosestPointToBounds(new Bounds(Position, Size), point);

            if (closestPointInGrid.x >= Position.x + _halfSize.x)
            {
                closestPointInGrid.x -= _halfCaseSize;
            }
            else if(closestPointInGrid.x <= Position.x - _halfSize.x)
            {
                closestPointInGrid.x += _halfCaseSize;
            }

            if (closestPointInGrid.y >= Position.y + _halfSize.y)
            {
                closestPointInGrid.y -= _halfCaseSize;
            }
            else if (closestPointInGrid.x <= Position.y - _halfSize.y)
            {
                closestPointInGrid.y += _halfCaseSize;
            }

            return GetCoordAt_WithoutCheck(closestPointInGrid);
        }

        /// <summary>
        /// Return the case position (it center)
        /// </summary>
        /// <param name="x">The abs of the coordinate</param>
        /// <param name="y">the ord of the coordinate</param>
        /// <returns></returns>
        public Vector2 GetCasePosition(int x, int y)
        {
            float position_x = x*CaseSize + _halfCaseSize - Position.x - _halfSize.x;
            float position_y = y*CaseSize + _halfCaseSize - Position.y - _halfSize.y;

            return new Vector2(position_x, position_y);
        }
    }
}