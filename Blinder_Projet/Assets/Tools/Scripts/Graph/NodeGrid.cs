using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    /// <summary>
    /// A grid Node with record to handle course into it,
    /// like pathfinding.
    /// </summary>
    [Serializable]
    public class NodeGrid : UnityGrid<NodeRecord>
    {
        /// <summary>
        /// Clear all the buffer to prepare the next course.
        /// </summary>
        public void ClearRecords()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (this[i][j] != null)
                    {
                        this[i][j].ClearReccords();
                    }
                }
            }
        }
    }
}
