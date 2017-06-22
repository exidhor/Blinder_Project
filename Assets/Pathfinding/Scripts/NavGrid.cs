using System;
using System.Collections.Generic;
using MapEditor;
using Tools;
using UnityEngine;

namespace Pathfinding
{
    [Serializable]
    public class NavGrid : NodeGrid
    {
        [SerializeField] private int _maxObjectCount;
        [SerializeField] private int _maxLevelCount;
        [SerializeField] private float _offset = 0.01f;

        private List<WeightedNode> _bufferNeighbourList = new List<WeightedNode>(4);

        //[SerializeField] private CoordQuadTree _quadTree;

        public void ConstructFrom(CaseContentGrid caseContentGrid)
        {
            ConstructNavGrid(caseContentGrid);
            ConstructQuadTree(caseContentGrid);
        }

        //public bool IsClearLine(Vector2 start, Vector2 end)
        //{
        //    Rect lineRect = GlobalBoundsHelper.GetGlobalRect(start, end);

        //    List<Vector2i> collidedCoords = _quadTree.Retrieve(lineRect);

        //    return collidedCoords.Count == 0;
        //}

        public bool IsClearLine(Vector2i start, Vector2i end)
        {
            List<Vector2i> collidedCoords = LineCast(start, end);

            bool isClear = collidedCoords.Count == 0;

            Debug.Log("is clear : " + isClear);

            Debug.Log("-------------------------------------");

            return isClear;
        }

        public List<Vector2i> LineCast(Vector2i start, Vector2i end)
        {
            List<Vector2i> visitedCoord = MyIntRayTrace(start.x, start.y, end.x, end.y);

            for (int i = visitedCoord.Count - 1; i >= 0; i--)
            {
                if (i < 0 || i >= visitedCoord.Count)
                {
                    Debug.Log("i : " + i);
                }

                if (GetCaseAt(visitedCoord[i]) != null)
                {
                    visitedCoord.RemoveAt(i);
                }
            }

            return visitedCoord;
        }

        // source : http://playtechs.blogspot.nl/2007/03/raytracing-on-grid.html
        private List<Vector2i> MyIntRayTrace(int x0, int y0, int x1, int y1)
        {
            List<Vector2i> visitedCoords = new List<Vector2i>();

            int dx = Mathf.Abs(x1 - x0);
            int dy = Mathf.Abs(y1 - y0);
            int x = x0;
            int y = y0;
            int n = 1 + dx + dy;
            int x_inc = (x1 > x0) ? 1 : -1;
            int y_inc = (y1 > y0) ? 1 : -1;
            int error = dx - dy;
            dx *= 2;
            dy *= 2;

            for (; n > 0; --n)
            {
                visitedCoords.Add(new Vector2i(x, y));

                if (error > 0)
                {
                    x += x_inc;
                    error -= dy;
                }
                else if(error < 0)
                {
                    y += y_inc;
                    error += dx;
                }
                else // error == 0
                {
                    visitedCoords.Add(new Vector2i(x + x_inc, y));

                    y += y_inc;
                    error += dx;
                }
            }

            return visitedCoords;
        }

        // source : http://playtechs.blogspot.nl/2007/03/raytracing-on-grid.html
        private List<Vector2i> MyRaytrace(float x0, float y0, float x1, float y1)
        {
            List<Vector2i> visitedCoords = new List<Vector2i>();

            float dx = Mathf.Abs(x1 - x0);
            float dy = Mathf.Abs(y1 - y0);

            Vector2i startCoord = GetCoordAt(x0, y0).Value;
            Vector2i endCoord = GetCoordAt(x1, y1).Value;

            Vector2 startCasePosition = GetCasePosition(startCoord);
            //Vector2 endCasePosition = GetCasePosition(endCoord);

            Debug.Log("Start : " + startCoord);
            Debug.Log("End : " + endCoord);

            int n = 1;
            int x_inc, y_inc;
            float error;

            if (dx == 0)
            {
                x_inc = 0;
                error = float.PositiveInfinity;
            }
            else if (x1 > x0)
            {
                x_inc = 1;
                n += endCoord.x - startCoord.x;
                error = (startCasePosition.x + 1 - x0) * dy;
            }
            else
            {
                x_inc = -1;
                n += startCoord.x - endCoord.x;
                error = (x0 - startCasePosition.x) * dy;
            }

            if (dy == 0)
            {
                y_inc = 0;
                error -= float.PositiveInfinity;
            }
            else if (y1 > y0)
            {
                y_inc = 1;
                n += endCoord.y - startCoord.y;
                error -= (startCasePosition.y + 1 - y0) * dx;
            }
            else
            {
                y_inc = -1;
                n += startCoord.y - endCoord.y;
                error -= (y0 - startCasePosition.y) * dx;
            }

            for (; n > 0; --n)
            {
                Vector2i visitedCoord = new Vector2i(startCoord.x, startCoord.y);

                visitedCoords.Add(visitedCoord);

                Debug.Log("Visit : " + visitedCoord + " (error : " + error + ")");

                if (error > 0)
                {
                    startCoord.y += y_inc;
                    error -= dx;
                }
                else
                {
                    startCoord.x += x_inc;
                    error += dy;
                }
            }

            return visitedCoords;
        }

        // source : http://playtechs.blogspot.nl/2007/03/raytracing-on-grid.html
        private List<Vector2i> Raytrace(float x0, float y0, float x1, float y1)
        {
            List<Vector2i> visitedCoords = new List<Vector2i>();

            float dx = Mathf.Abs(x1 - x0);
            float dy = Mathf.Abs(y1 - y0);

            int x = (int) Mathf.Floor(x0);
            int y = (int) Mathf.Floor(y0);

            int n = 1;
            int x_inc, y_inc;
            float error;

            if (dx == 0)
            {
                x_inc = 0;
                error = float.PositiveInfinity;
            }
            else if (x1 > x0)
            {
                x_inc = 1;
                n += (int)(Mathf.Floor(x1)) - x;
                error = (Mathf.Floor(x0) + 1 - x0) * dy;
            }
            else
            {
                x_inc = -1;
                n += x - (int) Mathf.Floor(x1);
                error = (x0 - Mathf.Floor(x0)) * dy;
            }

            if (dy == 0)
            {
                y_inc = 0;
                error -= float.PositiveInfinity;
            }
            else if (y1 > y0)
            {
                y_inc = 1;
                n += (int)(Mathf.Floor(y1)) - y;
                error -= (Mathf.Floor(y0) + 1 - y0) * dx;
            }
            else
            {
                y_inc = -1;
                n += y - (int)(Mathf.Floor(y1));
                error -= (y0 - Mathf.Floor(y0)) * dx;
            }

            for (; n > 0; --n)
            {
                visitedCoords.Add(new Vector2i(x, y));

                if (error > 0)
                {
                    y += y_inc;
                    error -= dx;
                }
                else
                {
                    x += x_inc;
                    error += dy;
                }
            }

            return visitedCoords;
        }

        private void ConstructNavGrid(CaseContentGrid caseContentGrid)
        {
            _cases = new List<List<NodeRecord>>();

            Copy(caseContentGrid);

            for (int i = 0; i < caseContentGrid.width; i++)
            {
                for (int j = 0; j < caseContentGrid.height; j++)
                {
                    if (caseContentGrid[i][j] == ECaseContent.None)
                    {
                        this[i][j] = new NodeRecord(new Vector2i(i, j));
                    }
                }
            }
        }

        private void ConstructQuadTree(CaseContentGrid caseContentGrid)
        {
            //_quadTree = new CoordQuadTree(new Rect(Position - Size/2, Size));

            //_quadTree.MAX_OBJECTS = _maxObjectCount;
            //_quadTree.MAX_LEVELS = _maxLevelCount;

            //float caseSideLength = caseContentGrid.CaseSize - _offset*2;

            //Vector2 caseSize = new Vector2(caseSideLength, caseSideLength);
            //Vector2 offset = caseSize/2 - new Vector2(_offset, _offset);

            //Rect caseBounds = new Rect(new Vector2(), caseSize);

            //for (int i = 0; i < caseContentGrid.width; i++)
            //{
            //    for (int j = 0; j < caseContentGrid.height; j++)
            //    {
            //        if (caseContentGrid[i][j] == ECaseContent.Blocking)
            //        {
            //            caseBounds.position = caseContentGrid.GetCasePosition(i, j) - offset;

            //            _quadTree.Insert(new Vector2i(i, j), caseBounds);
            //        }
            //    }
            //}
        }

        public List<WeightedNode> GetNeighbour(Vector2i coord)
        {
            _bufferNeighbourList.Clear();

            Vector2i neighbourCoord = coord;

            // compute sides
            // ------------------------------

            // left
            neighbourCoord.x -= 1;
            AddNeighbourSideIfValid(neighbourCoord);

            // right
            neighbourCoord.x += 2;
            AddNeighbourSideIfValid(neighbourCoord);

            // bot
            neighbourCoord.x = coord.x;
            neighbourCoord.y -= 1;
            AddNeighbourSideIfValid(neighbourCoord);

            // top
            neighbourCoord.y += 2;
            AddNeighbourSideIfValid(neighbourCoord);

            // ------------------------------
            // compute diagonals

            // top - left
            neighbourCoord.x -= 1;
            AddNeighbourDiagonalIfValid(coord, neighbourCoord);

            // top - right
            neighbourCoord.x += 2;
            AddNeighbourDiagonalIfValid(coord, neighbourCoord);

            // bot - right
            neighbourCoord.y -= 2;
            AddNeighbourDiagonalIfValid(coord, neighbourCoord);

            // bot - left
            neighbourCoord.x -= 2;
            AddNeighbourDiagonalIfValid(coord, neighbourCoord);

            // -----------------------------

            return _bufferNeighbourList;
        }

        private void AddNeighbourSideIfValid(Vector2i coord)
        {
            if (IsValidCoord(coord))
            {
                NodeRecord neighbour = GetCaseAt(coord);

                if (neighbour != null)
                {
                    _bufferNeighbourList.Add(new WeightedNode(neighbour, 1f));
                }
            }
        }

        private void AddNeighbourDiagonalIfValid(Vector2i from, Vector2i to)
        {
            if (IsValidCoord(to))
            {
                NodeRecord neighbour = GetCaseAt(to);

                if (neighbour != null)
                {
                    Vector2i offset = to - from;

                    Vector2i diagonalSideAbs = from;
                    diagonalSideAbs.x += offset.x;

                    Vector2i diagonalSideOrd = from;
                    diagonalSideOrd.y += offset.y;

                    if (!IsValidCoord(diagonalSideAbs) || !IsValidCoord(diagonalSideOrd))
                    {
                        return;
                    }

                    NodeRecord diagonalNeighbourAbs = GetCaseAt(diagonalSideAbs);
                    NodeRecord diagonalNeighbourOrd = GetCaseAt(diagonalSideOrd);

                    if (diagonalNeighbourAbs == null || diagonalNeighbourOrd == null)
                    {
                        return;
                    }

                    _bufferNeighbourList.Add(new WeightedNode(neighbour, MathHelper.TwoSqrt));
                }
            }
        }
    }
}