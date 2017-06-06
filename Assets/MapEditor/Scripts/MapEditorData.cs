using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MapEditor
{
    public class MapEditorData : ScriptableObject
    {
        public Vector2 Bounds;
        public float CaseSize;

        public bool DrawGrid;
        public bool DrawCase;

        public Color GridColor;

        public List<ColorContent> Colors = new List<ColorContent>();

        public List<ECaseContent> grid = new List<ECaseContent>();

        public void ClearGrid()
        {
            for (int i = 0; i < grid.Count; i++)
            {
                grid[i] = ECaseContent.None;
            }
        }
    }
}
