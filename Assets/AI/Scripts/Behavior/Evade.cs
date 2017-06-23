﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    public static partial class Behavior
    {
        public static SteeringOutput Evade(KinematicBody character, KinematicBody target, float speed, float maxPredictionTime)
        {
            Vector2 predictedTargetPosition = PredictTargetDeplacement(character, target, maxPredictionTime);

            return Flee(character, predictedTargetPosition, speed);
        }
    }
}
