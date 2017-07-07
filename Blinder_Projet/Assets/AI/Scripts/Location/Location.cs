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

                    case ELocationType.Body:
                        return _body != null;

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

                    case ELocationType.Body:
                        return _body.position;

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
                    case ELocationType.Body:
                        return _body.transform;

                    default:
                        return _transform;
                }
            }
        }

        public Body body
        {
            get { return _body; }
        }

        [SerializeField] private bool _isSet;

        // we set by default the type to Transform to ensure the "not set status" 
        // when serialize null ref
        [SerializeField] private ELocationType _type = ELocationType.Transform;

        [SerializeField] private Vector2 _position;
        [SerializeField] private Transform _transform;
        [SerializeField] private Body _body;

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

        public Location(Body body)
        {
            Set(body);
        }

        public void Copy(Location otherLocation)
        {
            _isSet = otherLocation._isSet;

            _type = otherLocation._type;

            _position = otherLocation._position;
            _transform = otherLocation._transform;
            _body = otherLocation._body;
        }

        public void Unset()
        {
            _isSet = false;

            _position = new Vector2();
            _transform = null;
            _body = null;
        }

        public void Set(Vector2 position)
        {
            _isSet = true;

            _type = ELocationType.Position;

            _position = position;
            _transform = null;
            _body = null;
        }

        public void Set(Transform transform)
        {
            _isSet = true;

            _type = ELocationType.Transform;

            _position = new Vector2();
            _transform = transform;
            _body = null;
        }

        public void Set(Body body)
        {
            _isSet = true;

            _type = ELocationType.Body;

            _position = new Vector2();
            _transform = null;
            _body = body;
        }
    }
}
