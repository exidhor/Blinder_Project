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
        public MapEditorData data;

        public Vector2i caseCount
        {
            get { return _caseCount; }
        }

        public Bounds bounds
        {
            get { return new Bounds(boundsCenter, boundsSize); }
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

        private Vector2i _caseCount;

        void Awake()
        {
            data = ScriptableObject.CreateInstance<MapEditorData>();

            Debug.DebugBreak();

            Debug.Log("m Awake " + GetInstanceID());
        }

        private void OnEnable()
        {
            Debug.Log("m OnEnable " + GetInstanceID());
        }

        private void OnDisable()
        {
            Debug.Log("m OnDisable " + GetInstanceID());
        }

        private void OnDestroy()
        {
            Debug.Log("m OnDestroy " + GetInstanceID());
        }

        public void ReconstructGrid()
        {
            ConstructGrid();
        }

        private void ConstructGrid()
        {
            _boundsSize = data.Bounds;

            _caseCount.x = (int) (data.Bounds.x/data.CaseSize);
            _caseCount.y = (int) (data.Bounds.y/data.CaseSize);

            data.grid.Clear();

            for (int i = 0; i < _caseCount.x; i++)
            {
                for (int j = 0; j < _caseCount.y; j++)
                {
                    data.grid.Add((int)ECaseContent.None);
                }
            }
        }
    }
}