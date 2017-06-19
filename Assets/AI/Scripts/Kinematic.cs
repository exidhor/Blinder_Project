using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Kinematic : MonoBehaviour
    {
        public float MaxSpeed;
        public float OrientationInDegree;

        public float OrientationInRadian
        {
            get { return OrientationInDegree * Mathf.Deg2Rad; }
            set { OrientationInDegree = value * Mathf.Rad2Deg; }
        }

        public float SqrMaxSpeed
        {
            get { return MaxSpeed * MaxSpeed; }
        }

        public bool isMoving
        {
            get
            {
                return -float.Epsilon > _rigidBody.velocity.x || _rigidBody.velocity.x > float.Epsilon
                    || -float.Epsilon > _rigidBody.velocity.y || _rigidBody.velocity.y > float.Epsilon;
            }
        }

        [SerializeField]
        private float _rotationInDegree;

        private Rigidbody2D _rigidBody;

        private Location _bufferLocation = new Location();

        private float _rotationInRadian
        {
            get { return _rotationInDegree * Mathf.Deg2Rad; }
            set { _rotationInDegree = value * Mathf.Rad2Deg; }
        }

        private void Start()
        {
            OrientationInDegree = 0;
            _rotationInDegree = 0;

            _rigidBody = GetComponent<Rigidbody2D>();
        }

        public void Actualize(SteeringOutput steering, float deltaTime)
        {
            if (steering.IsInstantOrientation)
            {
                OrientationInDegree = steering.AngularInDegree;
                _rotationInDegree = 0;

                ActualizeOrientation();
            }
            else
            {
                Rotate(steering.AngularInDegree, deltaTime);
            }

            Vector2 movement = steering.Linear;

            if (steering.IsOriented)
            {
                movement = ApplyRotation(movement);
            }

            Move(movement);

            CapVelocity();

            FaceMovementDirection();
        }

        public void ResetVelocity()
        {
            _rigidBody.velocity = Vector2.zero;
            _rigidBody.angularVelocity = 0f;
        }

        private void ActualizeOrientation()
        {
            _rigidBody.rotation = OrientationInDegree;
        }

        private void Rotate(float angularInDegree, float deltaTime)
        {
            OrientationInDegree += _rotationInDegree * deltaTime;
            OrientationInDegree %= 360f;

            _rotationInDegree += angularInDegree * deltaTime;

            ActualizeOrientation();
        }

        private void CapVelocity()
        {
            Vector2 velocity = _rigidBody.velocity;

            if (velocity.sqrMagnitude > SqrMaxSpeed)
            {
                velocity.Normalize();
                velocity *= MaxSpeed;

                _rigidBody.velocity = velocity;
            }
        }

        private void Move(Vector2 velocity)
        {
            _rigidBody.velocity += velocity;
        }

        private Vector2 ApplyRotation(Vector2 movement)
        {
            return MathHelper.RotateVector(movement, OrientationInRadian);
        }

        public Vector2 GetPosition()
        {
            return _rigidBody.position;
        }

        public Location GetDynamicLocation()
        {
            _bufferLocation.Set(_rigidBody.transform);

            return _bufferLocation;
        }

        public Location GetInstantLocation()
        {
            _bufferLocation.Set(GetPosition());

            return _bufferLocation;
        }

        public Vector2 GetVelocity()
        {
            return _rigidBody.velocity;
        }

        private void FaceMovementDirection()
        {
            // verify if it need actualization, otherwise
            // we keep the old value
            if (GetVelocity().sqrMagnitude > float.Epsilon * float.Epsilon)
            {
                OrientationInRadian = Mathf.Atan2(GetVelocity().y, GetVelocity().x);
            }
        }

        public Vector2 GetOrientationAsVector()
        {
            return MathHelper.GetDirectionFromAngle(OrientationInRadian);
        }

        void OnDrawGizmosSelected()
        {
            if (_rigidBody == null)
                return;

            Gizmos.color = Color.magenta;

            Vector2 direction = GetOrientationAsVector();

            Vector2 first = GetPosition();
            Vector2 second = first + direction;

            Gizmos.DrawLine(first, second);
        }

        public override string ToString()
        {
            return _rigidBody.name;
        }
    }
}
