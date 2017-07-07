using System;

namespace AI
{
    public class FaceSteering : Steering
    {
        public override void Recompute()
        {
            // nothing ?
        }

        public override SteeringOutput GetOutput()
        {
            if (!_target.isSet)
            {
                return new SteeringOutput();
            }

            SteeringOutput output = PrimitiveBehavior.Face(_character,
                                                    _target.position,
                                                    _properties);

            output.StopVelocity = true;

            return output;
        }
    }
}
