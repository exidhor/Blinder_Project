using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class KinematicBody : MonoBehaviour
    {
        public readonly float EPSILON = 0.001f;  
        
        public bool IsDynamic;
        public float LinearAcceleration;
        public float AngularAcceleration;

        public float MaxSpeed;
        public float MaxRotation;

        public float OrientationInDegree
        {
            get { return _rigidBody.rotation; }
        }

        public float OrientationInRadian
        {
            get { return OrientationInDegree * Mathf.Deg2Rad; }
            //set { OrientationInDegree = value * Mathf.Rad2Deg; }
        }

        public float SqrMaxSpeed
        {
            get { return MaxSpeed * MaxSpeed; }
        }

        public Vector2 velocity
        {
            get { return _rigidBody.velocity; }
        }

        public bool isMoving
        {
            get
            {
                return -float.Epsilon > _rigidBody.velocity.x || _rigidBody.velocity.x > float.Epsilon
                    || -float.Epsilon > _rigidBody.velocity.y || _rigidBody.velocity.y > float.Epsilon;
            }
        }

        private Rigidbody2D _rigidBody;

        private float _angularVelocity;

        private Location _bufferLocation = new Location();

        private Vector2 _oldVelocity;
        private float _oldAngularVelocity;
        private float _oldOrientation;

        void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            //OrientationInDegree = 0;
            //_rotationInDegree = 0;
        }

        public void Actualize(SteeringOutput steering, float deltaTime)
        {
            float inverseTime = 1/deltaTime;

            // 1 - we handle the rotation and orientation
            if (steering.IsInstantOrientation)
            {
                // if it's an instant orientation, we reset the orientation
                ResetOrientation(steering.AngularInDegree);
            }
            // else we increment the current rotation with the angular value
            else
            {
                Rotate(steering.AngularInDegree, deltaTime);
            }

            // 2 - we move the object from the linear value
            Vector2 movement = steering.Linear;

            // if the movement is orientated, we rotate first the vector
            // (it's a movement on the local coordinate)
            if (steering.IsOriented)
            {
                movement = MathHelper.RotateVector(movement, OrientationInRadian);
            }

            // we actualize the velocity of the rigidbody
            Move(movement);

            if (IsDynamic)
            {
                CapLinearAcceleration(deltaTime, inverseTime);

                ComputeAngularVelocity(inverseTime);
                CapAngularAcceleration(deltaTime, inverseTime);
            }
            else
            {
                ComputeAngularVelocity(inverseTime);
            }

            // we actualize the orientation to face the movement, if there is one
            FaceMovementDirection(_rigidBody.velocity);

            // we verify that the velocity is not greater than the max speed
            CapVelocity();
        }

        public void PrepareForUpdate()
        {
            // buffer last values
            _oldVelocity = _rigidBody.velocity;
            _oldAngularVelocity = _angularVelocity;
            _oldOrientation = _rigidBody.rotation;

            _rigidBody.velocity = Vector2.zero;
            _rigidBody.angularVelocity = 0;
            _angularVelocity = 0f;
        }

        private void ResetOrientation(float newOrientation)
        {
            _rigidBody.rotation = newOrientation;

            _angularVelocity = 0f;
        }

        private void Rotate(float angularInDegree, float deltaTime)
        {
            _rigidBody.rotation += angularInDegree * deltaTime;
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

        private void CapLinearAcceleration(float deltaTime, float inverseTime)
        {
            float currentSpeed = _rigidBody.velocity.magnitude;
            float lastSpeed = _oldVelocity.magnitude;

            float accelerationDelta = (currentSpeed - lastSpeed);
            float currentAcceleration = 0f;

            if (accelerationDelta > EPSILON)
            {
                currentAcceleration = accelerationDelta * inverseTime;
            }

            if (currentAcceleration > LinearAcceleration)
            {
                float targetSpeed = LinearAcceleration*deltaTime + lastSpeed;

                Vector2 velocity = _rigidBody.velocity;

                velocity.Normalize();
                velocity *= targetSpeed;

                _rigidBody.velocity = velocity;
            }
        }

        private void CapAngularAcceleration(float deltaTime, float inverseTime)
        {
            float accelerationDelta = (_angularVelocity - _oldAngularVelocity);
            float currentAcceleration = 0f;

            if (accelerationDelta > EPSILON)
            {
                currentAcceleration = Mathf.Abs(accelerationDelta)*inverseTime;
            }

            if (currentAcceleration > AngularAcceleration)
            {
                float validAngularVelocity = AngularAcceleration * deltaTime + _oldAngularVelocity;

                if (validAngularVelocity > MaxRotation)
                {
                    validAngularVelocity = MaxRotation;
                }
                else if (validAngularVelocity < -MaxRotation)
                {
                    validAngularVelocity = -MaxRotation;
                }

                //float delta = _angularVelocity - validAngularVelocity;
                //float newOrientation = delta*deltaTime;
                //float rotation = newOrientation - _rigidBody.rotation;

                float speed = _rigidBody.velocity.magnitude;

                _oldVelocity.Normalize();
                _oldVelocity *= speed;

                float rotation = validAngularVelocity*deltaTime;

                _rigidBody.velocity = MathHelper.RotateVector(_oldVelocity, rotation);

                _angularVelocity = validAngularVelocity;
            }
        }

        private void Move(Vector2 velocity)
        {
            _rigidBody.velocity = velocity;
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

        private void FaceMovementDirection(Vector2 movement)
        {
            // verify if it need actualization, otherwise
            // we keep the old value
            if (movement.sqrMagnitude > EPSILON * EPSILON)
            {
                float orientationInRadian = Mathf.Atan2(movement.y, movement.x);
                _rigidBody.rotation = orientationInRadian * Mathf.Rad2Deg;
            }
        }

        public Vector2 GetOrientationAsVector()
        {
            return MathHelper.GetDirectionFromAngle(OrientationInRadian);
        }

        private void ComputeAngularVelocity(float inverseTime)
        {
            float delta = _rigidBody.rotation - _oldOrientation;

            if (-EPSILON < delta && delta > EPSILON)
            {
                _angularVelocity = delta * inverseTime;
            }
            else
            {
                _angularVelocity = 0;
            }
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