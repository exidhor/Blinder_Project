using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    public static partial class PrimitiveBehavior
    {
        public static SteeringOutput Pursue(Body character, Body target, SteeringProperties properties)
        {
            // 1 - Calculate the target to delegate to Seek

            SteeringOutput output = new SteeringOutput();

            // Work out the distance to target
            output.Linear = target.position - character.position;
            float sqrDistance = output.Linear.sqrMagnitude;

            // Work out our current speed;
            float sqrSpeed = character.velocity.sqrMagnitude;

            float predictionTime;

            // Check if speed is too small to give a reasonable prediction time
            if (sqrSpeed * properties.maxPredictionTime * properties.maxPredictionTime < sqrDistance)
            {
                predictionTime = properties.maxPredictionTime;
            }
            else // Otherwise calculate the prediction tome
            {
                predictionTime = Mathf.Sqrt(sqrDistance / sqrSpeed);
            }

            Vector2 targetPosition = target.position + target.velocity * predictionTime;

            // 2 - Delegate to Seek
            return Seek(character, targetPosition, properties);
        }
    }
}
