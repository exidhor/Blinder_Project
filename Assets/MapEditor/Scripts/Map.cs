using Pathfinding;
using Tools;
using UnityEngine;

namespace MapEditor
{
    public class Map : MonoSingleton<Map>
    {
        [SerializeField]
        private MapEditorData _mapData;
        [SerializeField]
        private NavGrid _navGrid;

        public NavGrid currentNavGrid
        {
            get { return _navGrid; }
        }

        public void CreateNavGrid()
        {
            _navGrid = ScriptableObject.CreateInstance<NavGrid>();
            _navGrid.ConstructFrom(_mapData.Grid);
        }
    }
}
