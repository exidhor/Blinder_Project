using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace AI
{
    public class StaticBody : Body
    {
        [SerializeField]
        private float _rotationInDegree;

        private float _rotationInRadian
        {
            get { return _rotationInDegree * Mathf.Deg2Rad; }
            set { _rotationInDegree = value * Mathf.Rad2Deg; }
        }

        protected override void Start()
        {
            base.Start();
            
            _rotationInDegree = 0;
        }

        public override void Actualize(SteeringOutput steering, float deltaTime)
        {
            //if (steering.IsInstantOrientation)
            //{
            //    OrientationInDegree = steering.AngularInDegree;
            //    _rotationInDegree = 0;

            //    ActualizeOrientation();
            //}
            //else
            //{
            //    Rotate(steering.AngularInDegree, deltaTime);
            //}

            //Vector2 movement = steering.Linear;

            //if (steering.IsOriented)
            //{
            //    movement = ApplyRotation(movement);
            //}

            //Move(movement);

            //CapVelocity();

            //FaceMovementDirection();
        }

        public void ResetVelocity()
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;
        }

        private void ActualizeOrientation()
        {
            _rigidbody.rotation = OrientationInDegree;
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
            Vector2 velocity = _rigidbody.velocity;

            if (velocity.sqrMagnitude > SqrMaxSpeed)
            {
                velocity.Normalize();
                velocity *= MaxSpeed;

                _rigidbody.velocity = velocity;
            }
        }

        private void Move(Vector2 velocity)
        {
            _rigidbody.velocity += velocity;
        }

        private Vector2 ApplyRotation(Vector2 movement)
        {
            return MathHelper.RotateVector(movement, OrientationInRadian);
        }

        public Vector2 GetPosition()
        {
            return _rigidbody.position;
        }

        public Location GetDynamicLocation()
        {
            _bufferLocation.Set(_rigidbody.transform);

            return _bufferLocation;
        }

        public Location GetInstantLocation()
        {
            _bufferLocation.Set(GetPosition());

            return _bufferLocation;
        }

        public Vector2 GetVelocity()
        {
            return _rigidbody.velocity;
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
            if (_rigidbody == null)
                return;

            Gizmos.color = Color.magenta;

            Vector2 direction = GetOrientationAsVector();

            Vector2 first = GetPosition();
            Vector2 second = first + direction;

            Gizmos.DrawLine(first, second);
        }

        public override string ToString()
        {
            return _rigidbody.name;
        }
    }
}