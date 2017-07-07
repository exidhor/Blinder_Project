using System;
using UnityEngine;

namespace AI
{
    [Serializable]
    public struct SteeringOutput
    {
        //public bool IsOriented;
        //public bool IsInstantOrientation;

        public bool IsInstantVelocity;

        public bool StopVelocity;
        public bool StopRotation;

        public Vector2 Linear;
        public float AngularInDegree;

        public float AngularInRadian
        {
            get { return AngularInDegree * Mathf.Deg2Rad; }
            set { AngularInDegree = value * Mathf.Rad2Deg; }
        }

        public SteeringOutput(Vector2 linear, float angularInDegree)
        {
            Linear = linear;
            AngularInDegree = angularInDegree;

            StopVelocity = false;
            StopRotation = false;
            IsInstantVelocity = false;

            //IsOriented = false;
            //IsInstantOrientation = false;
        }

        public void Reset()
        {
            Linear = Vector2.zero;
            AngularInDegree = 0;
            IsInstantVelocity = false;
            //IsOriented = false;
            //IsInstantOrientation = false;
        }

        public bool IsFilled()
        {
            if (AngularInDegree > Mathf.Epsilon || AngularInDegree < -Mathf.Epsilon)
                return true;

            return Linear.x > Mathf.Epsilon || Linear.x < -Mathf.Epsilon
                || Linear.y > Mathf.Epsilon || Linear.y < -Mathf.Epsilon;
        }

        public void Scale(float value)
        {
            Linear *= value;
            AngularInDegree *= value;
        }

        public void Add(SteeringOutput steeringOutput)
        {
            Linear += steeringOutput.Linear;
            AngularInDegree += steeringOutput.AngularInDegree;
        }
    }
}
