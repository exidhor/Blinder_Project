using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MapEditor
{
    [Serializable]
    public class ColorContent 
    {
        public ECaseContent CaseContent;
        public Color Color;

        public ColorContent(ECaseContent caseContent)
        {
            CaseContent = caseContent;
            Color = new Color();
        }
    }
}
