using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace AI
{
    public static partial class PrimitiveBehavior
    {
        public static SteeringOutput Align(KinematicBody character,
                                           float targetOrientation,
                                           SteeringSpecs specs)
        {
            SteeringOutput output = new SteeringOutput();

            // Get the naive direction to the target
            float rotation = targetOrientation - character.orientationInDegree;

            rotation = MathHelper.NormalizeAngle(rotation, false);

            float rotationSize = Mathf.Abs(rotation);

            // check if we are already there, is yes, return a void output
            if (rotationSize < specs.targetRadius)
            {
                return output;
            }

            float targetRotation = specs.maxRotation;

            // if we are inside the slowRadius, then we calculate a scaled rotation
            if (rotationSize < specs.slowRadius)
            {
                targetRotation *= rotationSize/ specs.slowRadius;
            }

            // the final target rotation combines speed (already in the variable)
            // and the direction 
            targetRotation *= Mathf.Sign(rotation);

            // Acceleration tries to get to the target rotation
            output.AngularInDegree = targetRotation - character.rotation;
            output.AngularInDegree /= specs.timeToTarget;

            // Check if acceleration is too great
            float angularAcceleration = Mathf.Abs(output.AngularInDegree);
            if (angularAcceleration > specs.maxAngularAcceleration)
            {
                output.AngularInDegree /= angularAcceleration;
                output.AngularInDegree *= specs.maxAngularAcceleration;
            }

            return output;
        }
    }
}
