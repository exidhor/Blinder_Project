using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MemoryManagement
{
    /// <summary>
    /// Store the Data necessary to create a Pool
    /// into the PoolTable 
    /// </summary>
    [Serializable]
    public class PoolEntry
    {
        public PoolObject Prefab = null;
        public uint PoolSize = 100;
        public uint ExpandPoolSize = 10;
    }
}
