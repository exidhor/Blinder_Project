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
        public bool DrawCase;

        public bool DrawCoord;

        public float tmpoffset;

        public List<ColorContent> Colors = new List<ColorContent>();

        public CaseContentGrid Grid = new CaseContentGrid();
    }
}
