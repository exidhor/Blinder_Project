using UnityEngine;

namespace AI
{
    public static partial class PrimitiveBehavior
    {
        public static SteeringOutput KinematicArrive(KinematicBody character, 
            Vector2 target, 
            float targetRadius, 
            float speed,
            float slowRadius)
        {
            SteeringOutput output = new SteeringOutput();

            // First work out the direction
            output.Linear = target;
            output.Linear -= character.position;

            float squareDistance = output.Linear.SqrMagnitude();

            // If there is no direction, do nothing
            if (squareDistance < targetRadius * targetRadius)
            {
                output.Linear = Vector2.zero;
            }
            else
            {
                output.Linear.Normalize();
                output.Linear *= speed;

                // if we are outside the slowRadius, then go maxSpeed (and no changement)
                // Otherwise calculate a scaled speed
                if (squareDistance < slowRadius * slowRadius)
                {
                    output.Linear *= Mathf.Sqrt(squareDistance) / slowRadius;
                }

                // If that is too fast, then clip the speed
                output.Linear = Vector2.ClampMagnitude(output.Linear, speed);
            }

            return output;
        }
    }
}
