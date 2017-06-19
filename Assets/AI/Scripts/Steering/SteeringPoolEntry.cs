using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemoryManagement;

namespace AI
{
    [Serializable]
    public class SteeringPoolEntry : UnityPoolEntry
    {
        public ESteeringType Type;
    }
}
