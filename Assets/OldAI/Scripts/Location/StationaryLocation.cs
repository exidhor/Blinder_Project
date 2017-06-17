using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OldAI
{
    [Serializable]
    public class StationaryLocation : Location
    {
        [SerializeField]
        private Vector2 _position;

        public StationaryLocation(Vector2 position)
        {
            _position = position;
        }

        public StationaryLocation(float x, float y)
        {
            _position.x = x;
            _position.y = y;
        }

        public override Vector2 GetPosition()
        {
            return _position;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public override LocationType GetLocationType()
        {
            return LocationType.StationaryLocation;
        }
    }
}
