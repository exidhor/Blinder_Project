using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OldAI
{
    public static partial class Behavior
    {
        public static void Arrive(ref SteeringOutput output, Kinematic character, SteeringSpecs specs, Vector2 target)
        {
            // Reset Wander orientation, to be sure to start facing the right direction at next wander call
            ResetWander(specs);

            // First work out the direction
            output.Linear = target;
            output.Linear -= character.GetPosition();

            float squareDistance = output.Linear.SqrMagnitude();

            // If there is no direction, do nothing
            if (squareDistance < specs.RadiusMarginError * specs.RadiusMarginError)
            {
                output.Linear = Vector2.zero;
            }
            else
            {
                output.Linear.Normalize();
                output.Linear *= specs.MaxSpeed;

                // if we are outside the slowRadius, then go maxSpeed (and no changement)
                // Otherwise calculate a scaled speed
                if (squareDistance < specs.SlowArriveRadius * specs.SlowArriveRadius)
                {
                    output.Linear *= Mathf.Sqrt(squareDistance) / specs.SlowArriveRadius;
                }

                // Acceleration tries to get the target velocity
                //output.Linear -= character.GetVelocity();

                //if (specs.MinTimeToTarget > 0)
                //    output.Linear /= specs.MinTimeToTarget;

                // If that is too fast, then clip the speed
                output.Linear = Vector2.ClampMagnitude(output.Linear, specs.MaxSpeed);
            }
        }
    }
}
