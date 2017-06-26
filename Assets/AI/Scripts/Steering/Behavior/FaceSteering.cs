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

            return PrimitiveBehavior.Face(_character,
                _target.position,
                _specs);
        }
    }
}
