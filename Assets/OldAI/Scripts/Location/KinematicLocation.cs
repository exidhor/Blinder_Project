using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OldAI
{
    public class KinematicLocation : Location
    {
        private Kinematic _kinematic;

        public KinematicLocation(Kinematic kinematic)
        {
            _kinematic = kinematic;
        }

        public override Vector2 GetPosition()
        {
            return _kinematic.GetPosition();
        }

        public override LocationType GetLocationType()
        {
            return LocationType.KinematicLocation;
        }

        public override Kinematic GetKinematic()
        {
            return _kinematic;
        }

        public override Transform GetTransform()
        {
            return _kinematic.transform;
        }
    }
}
