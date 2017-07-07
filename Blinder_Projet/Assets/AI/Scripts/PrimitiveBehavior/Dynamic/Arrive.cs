using UnityEngine;

namespace AI
{
    public static partial class PrimitiveBehavior
    {
        public static SteeringOutput Arrive(Body character,
                                            Vector2 target,
                                            SteeringProperties properties)
        {
            SteeringOutput output = new SteeringOutput();

            // First work out the direction
            output.Linear = target;
            output.Linear -= character.position;

            float squareDistance = output.Linear.SqrMagnitude();

            // If there is no direction, do nothing
            if (squareDistance < properties.targetRadius * properties.targetRadius)
            {
                output.Linear = Vector2.zero;
                output.StopVelocity = true;

                return output;
            }
            
            output.Linear.Normalize();
            output.Linear *= properties.maxSpeed;

            // if we are outside the slowRadius, then go maxSpeed (and no changement)
            // Otherwise calculate a scaled speed
            if (squareDistance < properties.slowRadius * properties.slowRadius)
            {
                output.Linear *= Mathf.Sqrt(squareDistance) / properties.slowRadius;
            }

            // acceleration tries to get to the target velocity
            output.Linear -= character.velocity;

            if (properties.timeToTarget == 0f)
            {
                output.Linear *= properties.maxAcceleration;
            }
            else
            {
                output.Linear /= properties.timeToTarget;
            }

            // If that is too fast, then clip the speed
            output.Linear = Vector2.ClampMagnitude(output.Linear, properties.maxAcceleration);

            return output;
        }
    }
}
