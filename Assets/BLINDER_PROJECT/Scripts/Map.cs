using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapEditor;
using Pathfinding;
using Tools;
using UnityEngine;

namespace BlinderProject
{
    public class Map : MonoSingleton<Map>
    {
        [SerializeField]
        private MapEditorData _mapData;
        [SerializeField]
        private NavGrid _navGrid;

        public void CreateNavGrid()
        {
            _navGrid = ScriptableObject.CreateInstance<NavGrid>();
            _navGrid.ConstructFrom(_mapData.Grid);
        }

        public NavGrid GetNavGrid()
        {
            return _navGrid;
        }
    }
}
