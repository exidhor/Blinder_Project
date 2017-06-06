using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    public static partial class Behavior
    {
        public static void Pursue(ref SteeringOutput output, Kinematic character, SteeringSpecs specs, Kinematic target)
        {
            Vector2 predictedTargetPosition = PredictTargetDeplacement(character, specs, target);

            Seek(ref output, character, specs, predictedTargetPosition);
        }
    }
}
