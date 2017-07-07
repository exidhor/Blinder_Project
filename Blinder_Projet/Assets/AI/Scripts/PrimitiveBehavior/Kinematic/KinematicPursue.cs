using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    public static partial class PrimitiveBehavior
    {
        public static SteeringOutput KinematicPursue(Body character, Body target, float speed, float maxPredictionTime)
        {
            Vector2 predictedTargetPosition = KinematicPredictTargetDeplacement(character, target, maxPredictionTime);

            return KinematicSeek(character, predictedTargetPosition, speed);
        }
    }
}
