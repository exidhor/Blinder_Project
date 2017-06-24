using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI
{
    public enum ESteeringType
    {
        Seek = 0,
        Flee,
        Wander,
        Pursue,
        Patrol,

        None // this has to be the last because we map the enum index
             // into the pool list (see SteeringTable) 
    }
}
