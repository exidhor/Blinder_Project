﻿using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace AI
{
    public static partial class PrimitiveBehavior
    {
        public static SteeringOutput Seek(Body character, Vector2 target, SteeringProperties properties)
        {
            SteeringOutput output = new SteeringOutput();

            output.StopRotation = true;

            // First work out the direction
            output.Linear = target;
            output.Linear -= character.position;

            // If there is no direction, do nothing
            output.Linear = MathHelper.ConstructMovement(output.Linear, properties.maxAcceleration, 0.01f);

            return output;
        }
    }
}