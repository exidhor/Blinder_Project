using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5;
using MapEditor;
using UnityEngine;

namespace Pathfinding
{
    public class OptiPathfinder
    {
        private IntervalHeap<ECaseContent> _heap;

        public void Init()
        {
            _heap = new IntervalHeap<ECaseContent>();

            Vector2[] array = new Vector2[100];

            //Array.Clear();
        }
    }
}
