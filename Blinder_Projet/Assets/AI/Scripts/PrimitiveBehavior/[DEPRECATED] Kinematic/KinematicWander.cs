//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Tools;
//using UnityEngine;

//namespace AI
//{
//    public static partial class Behavior
//    {
//        public static void KinematicWander(Kinematic character)
//        {
//            //output.IsOriented = true;

//            // calculate the target to delegate to face

//            // update the wanderorientation
//            specs.WanderOrientation = RandomGenerator.instance.NextBinomialFloat(1) * specs.WanderRate;

//            // calculate the combined target orientation
//            float targetOrientationInDegree = specs.WanderOrientation + character.OrientationInDegree;

//            Vector2 characterOrientationAsVector = character.GetOrientationAsVector();

//            // calculate the center of the wander circle
//            //Vector2 targetMovement = character.GetPosition() + specs.MaxWanderOffset * characterOrientationAsVector;
//            Vector2 targetMovement = specs.MaxWanderOffset * characterOrientationAsVector;

//            // calculate the target location
//            targetMovement += specs.WanderRadius * MathHelper.GetDirectionFromAngle(
//                targetOrientationInDegree * Mathf.Deg2Rad);

//            //Face(ref output, character, specs, targetPosition);

//            // now set the speed
//            // output.AngularInDegree -= character.OrientationInDegree;
//            targetMovement.Normalize();
//            output.Linear = specs.MaxSpeed * targetMovement;
//        }

//        private static void ResetWander(SteeringSpecs specs)
//        {
//            specs.WanderOrientation = 0;
//        }
//    }
//}
