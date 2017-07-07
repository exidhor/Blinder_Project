using UnityEngine;

namespace AI
{
    public static partial class PrimitiveBehavior
    {
        public static SteeringOutput Face(Body character,
                                           Vector2 target,
                                           SteeringSpecs specs)
        {
            // First work out the direction
            Vector2 direction = target - character.position;

            // check if zero direction, and make no change if so
            if (direction.magnitude < 0.01f)
            {
                return new SteeringOutput();
            }

            // Put the target together
            float targetOrientation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
             
            return Align(character, 
                targetOrientation,
                specs);
        }
    }
}