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

        [SerializeField] private CoordQuadTree _quadTree;

        public void ConstructFrom(CaseContentGrid caseContentGrid)
        {
            ConstructNavGrid(caseContentGrid);
            ConstructQuadTree(caseContentGrid);
        }

        public bool IsClearLine(Vector2 start, Vector2 end)
        {
            Rect lineBounds = new Rect(start, end - start);

            List<Vector2i> collidedCoords = _quadTree.Retrieve(lineBounds);

            return collidedCoords.Count == 0;
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
            _quadTree = new CoordQuadTree(new Rect(Position - Size/2, Size));

            _quadTree.MAX_OBJECTS = _maxObjectCount;
            _quadTree.MAX_LEVELS = _maxLevelCount;

            float caseSideLength = caseContentGrid.CaseSize - _offset*2;

            Vector2 caseSize = new Vector2(caseSideLength, caseSideLength);
            Vector2 offset = caseSize/2 - new Vector2(_offset, _offset);

            Rect caseBounds = new Rect(new Vector2(), caseSize);

            for (int i = 0; i < caseContentGrid.width; i++)
            {
                for (int j = 0; j < caseContentGrid.height; j++)
                {
                    if (caseContentGrid[i][j] == ECaseContent.Blocking)
                    {
                        caseBounds.position = caseContentGrid.GetCasePosition(i, j) - offset;

                        _quadTree.Insert(new Vector2i(i, j), caseBounds);
                    }
                }
            }
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