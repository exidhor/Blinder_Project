using System;
using System.Collections.Generic;
using MapEditor;
using Tools;

namespace Pathfinding
{
    [Serializable]
    public class NavGrid : NodeGrid
    {
        private List<NodeRecord> _bufferNeighbourList;

        public NavGrid(CaseContentGrid caseContentGrid)
        {
            ConstructFrom(caseContentGrid);

            _bufferNeighbourList = new List<NodeRecord>(4);
        }

        public void ConstructFrom(CaseContentGrid caseContentGrid)
        {
            _cases = new List<List<NodeRecord>>();

            Copy(caseContentGrid);

            //Resize(caseContentGrid.width, caseContentGrid.height);

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

        public List<NodeRecord> GetNeighbour(Vector2i coord)
        {
            _bufferNeighbourList.Clear();

            Vector2i neighbourCoord = coord;
            NodeRecord neighbour = null;

            // left
            neighbourCoord.x -= 1;
            AddNeighbour(neighbourCoord);

            // right
            neighbourCoord.x += 2;
            AddNeighbour(neighbourCoord);

            // bot
            neighbourCoord.x = coord.x;
            neighbourCoord.y -= 1;
            AddNeighbour(neighbourCoord);

            // top
            neighbourCoord.y += 2;
            AddNeighbour(neighbourCoord);

            return _bufferNeighbourList;
        }

        private void AddNeighbour(Vector2i coord)
        {
            NodeRecord neighbour = null;

            if (IsValidCoord(coord))
            {
                neighbour = GetCaseAt(coord);

                if (neighbour != null)
                {
                    _bufferNeighbourList.Add(neighbour);
                }
            }
        }
    }
}