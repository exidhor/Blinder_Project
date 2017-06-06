using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace AI
{
    public static partial class Behavior
    {
        public static void Flee(ref SteeringOutput output, Kinematic character, SteeringSpecs specs, Vector2 target)
        {
            // Reset Wander orientation, to be sure to start facing the right direction at next wander call
            ResetWander(specs);

            // First work out the direction
            output.Linear = character.GetPosition();
            output.Linear -= target;

            // If there is no direction, do nothing
            output.Linear = MathHelper.GetKinematicMovement_MinCheck(output.Linear, specs.MaxSpeed, 0.01f);
        }
    }
}
