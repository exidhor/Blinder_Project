using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace AI
{
    [Serializable]
    public class Location
    {
        public ELocationType type
        {
            get { return _type; }
        }

        public bool isSet
        {
            get
            {
                if (!_isSet)
                    return false;

                switch (_type)
                {
                    case ELocationType.Transform:
                        return _transform != null;

                    case ELocationType.StaticBody:
                        return _staticBody != null;

                    default:
                        return true;
                }
            }
        }

        public Vector2 position
        {
            get
            {
                switch (_type)
                {
                    case ELocationType.Transform:
                        return _transform.position;

                    case ELocationType.StaticBody:
                        return _staticBody.GetPosition();

                    default:
                        return _position;
                }
            }
        }

        public Transform transform
        {
            get
            {
                switch (_type)
                {
                    case ELocationType.StaticBody:
                        return _staticBody.transform;

                    default:
                        return _transform;
                }
            }
        }

        public StaticBody staticBody
        {
            get { return _staticBody; }
        }

        [SerializeField] private bool _isSet;

        // we set by default the type to Transform to ensure the "not set status" 
        // when serialize null ref
        [SerializeField] private ELocationType _type = ELocationType.Transform;

        [SerializeField] private Vector2 _position;
        [SerializeField] private Transform _transform;
        [SerializeField] private StaticBody _staticBody;

        public Location()
        {
            Unset();
        }

        public Location(Vector2 position)
        {
            Set(position);
        }

        public Location(Transform transform)
        {
            Set(transform);
        }

        public Location(StaticBody staticBody)
        {
            Set(staticBody);
        }

        public void Copy(Location otherLocation)
        {
            _isSet = otherLocation._isSet;

            _type = otherLocation._type;

            _position = otherLocation._position;
            _transform = otherLocation._transform;
            _staticBody = otherLocation._staticBody;
        }

        public void Unset()
        {
            _isSet = false;

            _position = new Vector2();
            _transform = null;
            _staticBody = null;
        }

        public void Set(Vector2 position)
        {
            _isSet = true;

            _type = ELocationType.Position;

            _position = position;
            _transform = null;
            _staticBody = null;
        }

        public void Set(Transform transform)
        {
            _isSet = true;

            _type = ELocationType.Transform;

            _position = new Vector2();
            _transform = transform;
            _staticBody = null;
        }

        public void Set(StaticBody staticBody)
        {
            _isSet = true;

            _type = ELocationType.StaticBody;

            _position = new Vector2();
            _transform = null;
            _staticBody = staticBody;
        }
    }
}
