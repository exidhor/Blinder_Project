using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    public static partial class PrimitiveBehavior
    {
        private static Vector2 KinematicPredictTargetDeplacement(Body character, Body target, float maxPredictionTime)
        {
            // Work out the distance to target
            Vector2 direction = target.position - character.position;

            float distance = direction.magnitude;

            // Work out our current speed
            float currentSpeed = character.velocity.magnitude;

            float prediction = maxPredictionTime;

            // Check if speed is too small to give a reasonable prediction time
            // Otherwise calculate the prediction time
            if (currentSpeed > distance / maxPredictionTime)
            {
                prediction = distance / currentSpeed;
            }

            // Put the target together
            Vector2 targetPosition = target.position;
            targetPosition += target.velocity * prediction;

            return targetPosition;
        }
    }
}
