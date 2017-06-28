using System;

namespace MemoryManagement
{
    [Serializable]
    public sealed class UnityResource
    {
        public UnityPoolObject PoolObject;
        public bool IsUsed;

        public UnityResource(UnityPoolObject poolObject)
        {
            this.PoolObject = poolObject;
            IsUsed = false;
        }
    }
}
