using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace AI
{
    [Serializable]
    public class SteeringOutput
    {
        public bool IsKinematic;
        public bool IsOriented;
        public bool IsInstantOrientation;

        public Vector2 Linear;

        public float AngularInDegree;

        public float AngularInRadian
        {
            get { return AngularInDegree * Mathf.Deg2Rad;}
            set { AngularInDegree = value * Mathf.Rad2Deg; }
        }

        public SteeringOutput()
        {
            Reset();
        }

        public SteeringOutput(Vector2 linear, float angularInDegree, bool isKinematic)
        {
            Linear = linear;
            AngularInDegree = angularInDegree;
            IsKinematic = isKinematic;
        }

        public void Reset()
        {
            Linear = Vector2.zero;
            AngularInDegree = 0;
            IsKinematic = true;
            IsOriented = false;
            IsInstantOrientation = false;
        }

        public bool IsFilled()
        {
            if (AngularInDegree > Mathf.Epsilon || AngularInDegree < -Mathf.Epsilon)
                return true;

            return Linear.x > Mathf.Epsilon || Linear.x < -Mathf.Epsilon
                || Linear.y > Mathf.Epsilon || Linear.y < - Mathf.Epsilon;
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
