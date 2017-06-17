using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OldAI
{
    [Serializable]
    public class LocationComponent
    {
        private Location _location = null;
        private LocationType _type;

        private StationaryLocation StationaryLocation
        {
            get { return (StationaryLocation) _location; }
        }

        private TransformLocation TransformLocation
        {
            get { return (TransformLocation) _location; }
        }

        public LocationComponent()
        {
            ConstructStationaryLocation(new Vector2(0, 0));
        }

        public void SetLocation(Location location)
        {
            _type = location.GetLocationType();
            _location = location;
        }

        public void SetTargetTransform(Transform transform)
        {
            if (_type != LocationType.TransformLocation)
            {
                ConstructTransformLocation(transform);
            }
            else
            {
                SetInternalTargetTransform(transform);
            }
        }

        public void SetTargetPosition(Vector2 targetPosition)
        {
            if (_type != LocationType.StationaryLocation)
            {
                ConstructStationaryLocation(targetPosition);
            }
            else
            {
                SetInternalTargetPosition(targetPosition);
            }
        }

        public Vector2 GetPosition()
        {
            return _location.GetPosition();
        }

        private void ConstructStationaryLocation(Vector2 targetPosition)
        {
            _type = LocationType.StationaryLocation;
            _location = new StationaryLocation(targetPosition);
        }

        private void ConstructTransformLocation(Transform transform)
        {
            _type = LocationType.TransformLocation;
            _location = new TransformLocation(transform);
        }

        private void SetInternalTargetPosition(Vector2 targetPosition)
        {
            StationaryLocation.SetPosition(targetPosition);
        }

        private void SetInternalTargetTransform(Transform transform)
        {
            TransformLocation.SetTransform(transform);
        }

        public LocationType GetLocationType()
        {
            return _type;
        }

        public Transform GetTransform()
        {
            if (_type == LocationType.TransformLocation)
            {
                return TransformLocation.GetTransform();
            }

            return null;
        }
    }
}
