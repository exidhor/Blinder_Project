using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    public class TransformLocation : Location
    {
        private Transform _transform;

        public TransformLocation(Transform transform)
        {
            _transform = transform;
        }

        public override Vector2 GetPosition()
        {
            return _transform.position;
        }

        public override LocationType GetLocationType()
        {
            return LocationType.TransformLocation;
        }

        public override Transform GetTransform()
        {
            return _transform;
        }

        public void SetTransform(Transform transform)
        {
            _transform = transform;
        }
    }
}
