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

            _nodeGrid = new NodeGrid(_data.Grid.width, _data.Grid.height);

            for (int i = 0; i < _data.Grid.width; i++)
            {
                for (int j = 0; j < _data.Grid.height; j++)
                {
                    _nodeGrid.Get(i, j).IsBlocking = (_data.Grid[i][j] == ECaseContent.Blocking);
                }
            }
        }

        public bool PointIsInGrid(Vector2 point)
        {
            return !(point.x < _data.Grid.Position.x - _data.Grid.CaseSize / 2 
                   || point.x > _data.Grid.Position.x +_data.Grid.Size.x - _data.Grid.CaseSize / 2
                   || point.y < _data.Grid.Position.y - _data.Grid.CaseSize / 2 
                   || point.y > _data.Grid.Position.y + _data.Grid.Size.y - _data.Grid.CaseSize / 2);
        }

        public Node GetNodeAt(Vector2 position)
        {
            if (!PointIsInGrid(position))
            {
                return null;
            }

            int x = (int)((position.x - _data.Grid.Position.x) / _data.Grid.CaseSize + _data.Grid.CaseSize / 2);
            int y = (int)((position.y - _data.Grid.Position.y) / _data.Grid.CaseSize + _data.Grid.CaseSize / 2);

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
