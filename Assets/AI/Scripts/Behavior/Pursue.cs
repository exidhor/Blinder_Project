using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    public static partial class Behavior
    {
        public static SteeringOutput Pursue(Kinematic character, Kinematic target, float speed, float maxPredictionTime)
        {
            Vector2 predictedTargetPosition = PredictTargetDeplacement(character, target, maxPredictionTime);

            return Seek(character, predictedTargetPosition, speed);
        }
    }
}
