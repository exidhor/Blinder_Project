﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace AI
{
    public static partial class PrimitiveBehavior
    {
        public static SteeringOutput KinematicFlee(Body character, Vector2 target, float speed)
        {
            SteeringOutput output = new SteeringOutput();

            // First work out the direction
            output.Linear = character.position;
            output.Linear -= target;

            // If there is no direction, do nothing
            output.Linear = MathHelper.ConstructMovement(output.Linear, speed, 0.01f);

            return output;
        }
    }
}
