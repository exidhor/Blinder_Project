using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    [Serializable]
    public class NodeGrid : UnityGrid<NodeRecord>
    {
        public void ClearRecords()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    this[i][j].ClearReccords();
                }
            }
        }
    }
}
