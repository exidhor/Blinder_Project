using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI
{
    public enum EBehavior
    {
        None,
        Seek,
        Flee,
        Arrive,
        Wander,
        Face,
        Pursue,
        Evade,
        DelegateWander,
        //PathFollowing,

        CollisionAvoidance
    }
}
