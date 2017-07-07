using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI
{
    public class PursueSteering : Steering
    {
        public override void Recompute()
        {
            // Nothing yet
        }

        public override SteeringOutput GetOutput()
        {
            if (!_target.isSet)
            {
                return new SteeringOutput();
            }

            SteeringOutput output = PrimitiveBehavior.Pursue(_character,
                                                            _target.body,
                                                            _properties);

            output.StopRotation = true;

            return output;
        }
    }
}
