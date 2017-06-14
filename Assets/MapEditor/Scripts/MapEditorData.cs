using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace MapEditor
{
    public class MapEditorData : ScriptableObject
    {
        public Vector2 Bounds;
        public float CaseSize;
        public Vector2 Position; // bot left

        public Vector2i CaseCount;

        public bool DrawGrid;
        public bool DrawCase;

        public Color GridColor;

        public List<ColorContent> Colors = new List<ColorContent>();

        //public List<ECaseContent> Grid = new List<ECaseContent>();
        public CaseContentGrid Grid = new CaseContentGrid();

        public void ClearGrid()
        {
            Grid.Clear(ECaseContent.None);

            //for (int i = 0; i < Grid.Count; i++)
            //{
            //    Grid[i] = ECaseContent.None;
            //}
        }
    }
}
