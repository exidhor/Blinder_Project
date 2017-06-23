using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Tools;

namespace AI
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Body : MonoBehaviour
    {
        public float MaxSpeed;
        public float OrientationInDegree;

        public float OrientationInRadian
        {
            get { return OrientationInDegree * Mathf.Deg2Rad; }
            set { OrientationInDegree = value * Mathf.Rad2Deg; }
        }

        public Vector2 position
        {
            get { return _rigidbody.position; }
        }

        public float SqrMaxSpeed
        {
            get { return MaxSpeed * MaxSpeed; }
        }

        public bool isMoving
        {
            get
            {
                return -float.Epsilon > _rigidbody.velocity.x || _rigidbody.velocity.x > float.Epsilon
                    || -float.Epsilon > _rigidbody.velocity.y || _rigidbody.velocity.y > float.Epsilon;
            }
        }

        protected Location _bufferLocation = new Location();

        protected Rigidbody2D _rigidbody;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            OrientationInDegree = 0;
        }

        public abstract void Actualize(SteeringOutput steering, float deltaTime);

        public Vector2 GetOrientationAsVector()
        {
            return MathHelper.GetDirectionFromAngle(OrientationInRadian);
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (_rigidbody == null)
                return;

            Gizmos.color = Color.magenta;

            Vector2 direction = GetOrientationAsVector();

            Vector2 first = position;
            Vector2 second = first + direction;

            Gizmos.DrawLine(first, second);
        }
    }
}
