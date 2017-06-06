using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    public enum LocationType
    {
        StationaryLocation,
        TransformLocation,
        KinematicLocation
    }

    public abstract class Location
    {
        public abstract Vector2 GetPosition();

        public abstract LocationType GetLocationType();

        public virtual Transform GetTransform()
        {
            return null;
        }

        public virtual Kinematic GetKinematic()
        {
            return null;
        }
    }
}
