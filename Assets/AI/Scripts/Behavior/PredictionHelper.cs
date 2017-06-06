using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    public static partial class Behavior
    {
        private static Vector2 PredictTargetDeplacement(Kinematic character, SteeringSpecs specs, Kinematic target)
        {
            // Work out the distance to target
            Vector2 direction = target.GetPosition() - character.GetPosition();

            float distance = direction.magnitude;

            // Work out our current speed
            float currentSpeed = character.GetVelocity().magnitude;

            float prediction = specs.MaxPredictionTime;

            // Check if speed is too small to give a reasonable prediction time
            // Otherwise calculate the prediction time
            if (currentSpeed > distance / specs.MaxPredictionTime)
            {
                prediction = distance / currentSpeed;
            }

            // Put the target together
            Vector2 targetPosition = target.GetPosition();
            targetPosition += target.GetVelocity() * prediction;

            return targetPosition;
        }
    }
}
