using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tools
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform TargetToFollow;

        public float Bounds;

        void LateUpdate()
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
