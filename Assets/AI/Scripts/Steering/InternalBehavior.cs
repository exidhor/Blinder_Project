using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI
{
    [Serializable]
    public class InternalBehavior
    {
        public EBehavior Behavior;
        public Location Target;
        public float Weight;

        public InternalBehavior(EBehavior behavior, Location target, float weight)
        {
            Behavior = behavior;
            Target = target;
            Weight = weight;
        }
    }
}
