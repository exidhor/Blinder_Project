using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace MapEditor
{
    [ExecuteInEditMode]
    public class MapEditorModel : MonoBehaviour
    {
        public MapEditorData data;

        public Vector2i caseCount
        {
            get { return _caseCount; }
        }

        private Vector2i _caseCount;

        //public List<ECaseContent> grid
        //{
        //    get { return grid; }
        //}

        public List<ColorContent> colorTest;

        public List<ECaseContent> grid;

        void Awake()
        {
            grid = new List<ECaseContent>();

            data = ScriptableObject.CreateInstance<MapEditorData>();
        }

        public void SetData(MapEditorData data)
        {
            this.data = data;

            ConstructGrid();
        }

        private void ConstructGrid()
        {
            _caseCount.x = (int)(data.Bounds.x / data.CaseSize.x);
            _caseCount.y = (int)(data.Bounds.y / data.CaseSize.y);

            grid.Clear();

            for (int i = 0; i < _caseCount.x; i++)
            {
                for (int j = 0; j < _caseCount.y; j++)
                {
                    grid.Add(ECaseContent.None);
                }
            }
        }
    }
}
