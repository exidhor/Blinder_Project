using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OldAI
{
    public static partial class Behavior
    {
        public static void Evade(ref SteeringOutput output, Kinematic character, SteeringSpecs specs, Kinematic target)
        {
            Vector2 predictedTargetPosition = PredictTargetDeplacement(character, specs, target);

            Flee(ref output, character, specs, predictedTargetPosition);
        }
    }
}
