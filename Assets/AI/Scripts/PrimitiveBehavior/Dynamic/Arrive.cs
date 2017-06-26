using UnityEngine;

namespace AI
{
    public static partial class PrimitiveBehavior
    {
        public static SteeringOutput Arrive(KinematicBody character,
                                            Vector2 target,
                                            SteeringSpecs specs)
        {
            SteeringOutput output = new SteeringOutput();

            // First work out the direction
            output.Linear = target;
            output.Linear -= character.position;

            float squareDistance = output.Linear.SqrMagnitude();

            // If there is no direction, do nothing
            if (squareDistance < specs.targetRadius * specs.targetRadius)
            {
                output.Linear = Vector2.zero;
                output.StopVelocity = true;

                return output;
            }
            
            output.Linear.Normalize();
            output.Linear *= specs.maxSpeed;

            // if we are outside the slowRadius, then go maxSpeed (and no changement)
            // Otherwise calculate a scaled speed
            if (squareDistance < specs.slowRadius * specs.slowRadius)
            {
                output.Linear *= Mathf.Sqrt(squareDistance) / specs.slowRadius;
            }

            // acceleration tries to get to the target velocity
            output.Linear -= character.velocity;

            if (specs.timeToTarget == 0f)
            {
                output.Linear *= specs.maxAcceleration;
            }
            else
            {
                output.Linear /= specs.timeToTarget;
            }

            // If that is too fast, then clip the speed
            output.Linear = Vector2.ClampMagnitude(output.Linear, specs.maxAcceleration);

            return output;
        }
    }
}
