using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    public static partial class PrimitiveBehavior
    {
        public static SteeringOutput KinematicFace(Body character, Vector2 target)
        {
            SteeringOutput output = new SteeringOutput();

            //output.IsInstantOrientation = true;
            //output.IsOriented = false;

            // work out the direction to target
            Vector2 direction = target - character.position;

            // Check for a zero direction, and make no change if so
            if (direction.sqrMagnitude < float.Epsilon*float.Epsilon)
            {
                return new SteeringOutput();
            }

            output.AngularInDegree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            return output;
        }
    }
}
