using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace MapEditor
{
    [ExecuteInEditMode]
    public class MouseCoordDisplayer : MonoSingleton<MouseCoordDisplayer>
    {
        public Vector2i? mouseCoord = null;
        private string textCoord;

        void OnGUI()
        {
            Debug.Log("Drawn");

            if (mouseCoord.HasValue)
            {
                textCoord = "Coord : " + mouseCoord.Value.x + ", " + mouseCoord.Value.y;
            }
            else
            {
                textCoord = "Coord : " + "None";
            }

            GUI.Label(new Rect(10, 10, 100, 20), textCoord);
        }
    }
}
