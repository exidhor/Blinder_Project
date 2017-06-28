using UnityEngine;

namespace Tools
{
    /// <summary>
    /// Let the target, inside the Bounds, and move the attached GameObject
    /// if it's not the case anymore.
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        public Transform TargetToFollow;

        public float Bounds;

        void FixedUpdate()
        {
            float distance = Vector2.Distance(TargetToFollow.position, transform.position);

            if (distance > Bounds)
            {
                Vector2 newPosition = MathHelper.ShootPoint(transform.position, TargetToFollow.position, distance - Bounds);

                transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            }
        }
    }
}
