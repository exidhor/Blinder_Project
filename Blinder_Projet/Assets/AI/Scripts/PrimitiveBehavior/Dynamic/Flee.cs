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
        public static SteeringOutput Flee(Body character,
                                            Vector2 target,
                                            SteeringProperties properties)
        {
            SteeringOutput output = new SteeringOutput();

            // First work out the direction
            output.Linear = character.position;
            output.Linear -= target;

            // If there is no direction, do nothing
            output.Linear = MathHelper.ConstructMovement(output.Linear, properties.maxAcceleration, 0.01f);

            return output;
        }
    }
}
