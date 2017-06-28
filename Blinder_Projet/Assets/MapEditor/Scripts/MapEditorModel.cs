using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    [ExecuteInEditMode]
    public class MapEditorModel : EditModeSingleton<MapEditorModel>
    {
        public MapEditorData Data;

        public float caseSize
        {
            get
            {
                if (Data == null)
                    return 0;

                return Data.Grid.CaseSize;
            }
        }

        public CaseContentGrid grid
        {
            get
            {
                if (Data == null)
                    return null;

                return Data.Grid;
            }
        }

        public Bounds bounds
        {
            get { return new Bounds(center, Data.Grid.Size); }
        }

        public Vector2 center
        {
            get { return transform.position; }
        }

        public Vector2 size
        {
            get { return _size; }
        }

        private Vector2 _size;

        public void ConstructGrid()
        {
            //Data.Grid.Clear();

            Data.Grid.Resize(Data.Grid.width, Data.Grid.height);
        }

        void Awake()
        {
            if (Application.isPlaying)
            {
                //DontDestroyOnLoad(transform.gameObject);
            }
        }

        void Update()
        {
            if (Data != null)
            {
                Data.Grid.Position = transform.position;
            }
        }
    }
}