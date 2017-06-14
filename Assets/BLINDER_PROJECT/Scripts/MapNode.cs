using MapEditor;
using Pathfinding;
using Tools;
using UnityEngine;

namespace BlinderProject
{
    public class MapNode : MonoSingleton<MapNode>
    {
        public bool DrawGrid;
        public Color GridColor;

        private NodeGrid _nodeGrid;
        private MapEditorData _data;

        public void Load(MapEditorData data)
        {
            _data = data;

            _nodeGrid = new NodeGrid((int)(_data.Bounds.x / _data.CaseSize),
                (int)(_data.Bounds.y / _data.CaseSize));

            for (int i = 0; i < _data.CaseCount.x; i++)
            {
                for (int j = 0; j < _data.CaseCount.y; j++)
                {
                    //int gridIndex = i* _data.CaseCount.x + j;

                    _nodeGrid.Get(i, j).IsBlocking = (_data.Grid[i][j] == ECaseContent.Blocking);
                }
            }
        }

        public bool PointIsInGrid(Vector2 point)
        {
            return !(point.x < _data.Position.x - _data.CaseSize / 2 || point.x > _data.Position.x +_data.Bounds.x - _data.CaseSize / 2
                   || point.y < _data.Position.y - _data.CaseSize / 2 || point.y > _data.Position.y + _data.Bounds.y - _data.CaseSize / 2);
        }

        public Node GetNodeAt(Vector2 position)
        {
            if (!PointIsInGrid(position))
            {
                return null;
            }

            int x = (int)((position.x - _data.Position.x) / _data.CaseSize + _data.CaseSize / 2);
            int y = (int)((position.y - _data.Position.y) / _data.CaseSize + _data.CaseSize / 2);

            //Debug.Log("(position.x - _roomPosition.x)/_caseSize.x = " + "( " + position.x + " - " + _roomPosition.x + " ) " + " / " + _caseSize.x + ") => " + x);
            //Debug.Log("(position.y - _roomPosition.y)/_caseSize.y = " + "( " + position.y + " - " + _roomPosition.y + " ) " + " / " + _caseSize.y + ") => " + y);

            return _nodeGrid.Get(x, y);
        }

        public NodeGrid DEBUG_GetNodeGrid()
        {
            return _nodeGrid;
        }
    }
}
