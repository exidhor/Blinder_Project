using System;

namespace MemoryManagement
{
    /// <summary>
    /// Store the Data necessary to create a Pool
    /// into the PoolTable 
    /// </summary>
    [Serializable]
    public class UnityPoolEntry
    {
        public UnityPoolObject Prefab = null;
        public uint PoolSize = 100;
        public uint ExpandPoolSize = 10;
    }
}
