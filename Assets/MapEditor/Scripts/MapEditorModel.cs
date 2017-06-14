using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace MapEditor
{
    [ExecuteInEditMode]
    public class MapEditorModel : MonoSingleton<MapEditorModel>
    {
        public MapEditorData Data;

        public float caseSize
        {
            get
            {
                if (Data == null)
                    return 0;

                return Data.CaseSize;
            }
        }

        public Bounds bounds
        {
            get { return new Bounds(boundsCenter, Data.Bounds); }
        }

        public Vector2 boundsCenter
        {
            get { return transform.position; }
        }

        public Vector2 boundsSize
        {
            get { return _boundsSize; }
        }

        private Vector2 _boundsSize;


        public void ReconstructGrid()
        {
            ConstructGrid();
        }

        private void ConstructGrid()
        {
            Data.Grid.Clear();

            Data.Grid.Resize(Data.CaseCount.x, Data.CaseCount.y);

            //for (int i = 0; i < Data.CaseCount.x; i++)
            //{
            //    for (int j = 0; j < Data.CaseCount.y; j++)
            //    {
            //        Data.Grid.Add((int)ECaseContent.None);
            //    }
            //}
        }

        void Update()
        {
            if (Data != null)
            {
                Data.Position = transform.position;
            }
        }
    }
}