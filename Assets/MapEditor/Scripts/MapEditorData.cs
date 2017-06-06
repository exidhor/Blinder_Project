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
        public Vector2 CaseSize;

        public bool DrawGrid;
        public bool DrawCase;

        public Color GridColor;

        public List<ColorContent> Colors = new List<ColorContent>();
    }
}
