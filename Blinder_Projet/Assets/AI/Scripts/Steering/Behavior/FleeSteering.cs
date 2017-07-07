using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI
{
    public class FleeSteering : Steering
    {
        public override void Recompute()
        {
            // nothing yet
        }

        public override SteeringOutput GetOutput()
        {
            if (!_target.isSet)
            {
                return new SteeringOutput();
            }

            // todo : make an escape more intelligent (using pathfinding)

            SteeringOutput output = PrimitiveBehavior.Flee(_character, _target.position, _properties);

            output.StopRotation = true;

            return output;
        }
    }
}
