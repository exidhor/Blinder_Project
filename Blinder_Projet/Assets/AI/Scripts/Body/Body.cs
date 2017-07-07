using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Body : MonoBehaviour
    {
        public readonly float EPSILON = 0.001f;  
        
        public bool IsDynamic;
        public float LinearAcceleration;
        public float AngularAcceleration;

        public float MaxSpeed;
        public float MaxAngularSpeed;

        public float orientationInDegree
        {
            get { return _rigidbody.rotation; }
        }

        public float orientationInRadian
        {
            get { return orientationInDegree * Mathf.Deg2Rad; }
            //set { OrientationInDegree = value * Mathf.Rad2Deg; }
        }

        public float rotation
        {
            get { return _rigidbody.angularVelocity; }
        }

        public float sqrMaxSpeed
        {
            get { return MaxSpeed * MaxSpeed; }
        }

        public Vector2 position
        {
            get { return _rigidbody.position; }
        }

        public Vector2 velocity
        {
            get { return _rigidbody.velocity; }
        }

        public bool isMoving
        {
            get
            {
                return -float.Epsilon > _rigidbody.velocity.x || _rigidbody.velocity.x > float.Epsilon
                    || -float.Epsilon > _rigidbody.velocity.y || _rigidbody.velocity.y > float.Epsilon;
            }
        }

        private Rigidbody2D _rigidbody;

        private float _angularVelocity;

        private Location _bufferLocation = new Location();

        private Vector2 _oldVelocity;
        private float _oldAngularVelocity;
        private float _oldOrientation;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            //OrientationInDegree = 0;
            //_rotationInDegree = 0;
        }

        public void Actualize(SteeringOutput steering, float deltaTime)
        {
            if (steering.StopVelocity)
            {
                _rigidbody.velocity = Vector2.zero;
            }
            else
            {
                Vector2 currentVelocity = _rigidbody.velocity;

                currentVelocity += steering.Linear * deltaTime;

                if (currentVelocity.sqrMagnitude > MaxSpeed * MaxSpeed)
                {
                    currentVelocity.Normalize();
                    currentVelocity *= MaxSpeed;
                }

                _rigidbody.velocity = currentVelocity;
                FaceMovementDirection(currentVelocity);
            }
            
            if (steering.StopRotation)
            {
                _rigidbody.angularVelocity = 0;
            }
            else
            {
                _rigidbody.angularVelocity += steering.AngularInDegree;
            }
        }

        public void ResetBody()
        {
            _rigidbody.angularVelocity = 0f;
            _rigidbody.velocity = Vector2.zero;
        }


        //public void Actualize(SteeringOutput steering, float deltaTime)
        //{
        //    float inverseTime = 1/deltaTime;

        //    // 1 - we handle the rotation and orientation
        //    if (steering.IsInstantOrientation)
        //    {
        //        // if it's an instant orientation, we reset the orientation
        //        ResetOrientation(steering.AngularInDegree);
        //    }
        //    // else we increment the current rotation with the angular value
        //    else
        //    {
        //        Rotate(steering.AngularInDegree, deltaTime);
        //    }

        //    // 2 - we compute the movement from the linear value
        //    Vector2 movement = steering.Linear;

        //    // if the movement is orientated, we rotate first the vector
        //    // (it's a movement on the local coordinate)
        //    if (steering.IsOriented)
        //    {
        //        movement = MathHelper.RotateVector(movement, OrientationInRadian);
        //    }

        //    // we actualize the velocity of the rigidbody
        //    Move(movement);

        //    if (IsDynamic)
        //    {
        //        CapLinearAcceleration(deltaTime, inverseTime);

        //        // we actualize the orientation to face the movement, if there is one
        //        FaceMovementDirection(_rigidbody.velocity);

        //        ComputeAngularVelocity(inverseTime);
        //        CapAngularAcceleration(deltaTime, inverseTime);

        //        // we actualize the orientation to face the movement, if there is one
        //        FaceMovementDirection(_rigidbody.velocity);
        //    }
        //    else
        //    {
        //        // we actualize the orientation to face the movement, if there is one
        //        FaceMovementDirection(_rigidbody.velocity);

        //        ComputeAngularVelocity(inverseTime);
        //    }

        //    // we verify that the velocity is not greater than the max speed
        //    CapVelocity();
        //}

        public void PrepareForUpdate()
        {
            // buffer last values
            _oldVelocity = _rigidbody.velocity;
            _oldAngularVelocity = _angularVelocity;
            _oldOrientation = _rigidbody.rotation;

            //_rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0;
            //_angularVelocity = 0f;
        }

        private void ResetOrientation(float newOrientation)
        {
            _rigidbody.rotation = newOrientation;

            _angularVelocity = 0f;
        }

        private void Rotate(float angularInDegree, float deltaTime)
        {
            _rigidbody.rotation += angularInDegree * deltaTime;
        }

        private void CapVelocity()
        {
            Vector2 velocity = _rigidbody.velocity;

            if (velocity.sqrMagnitude > sqrMaxSpeed)
            {
                velocity.Normalize();
                velocity *= MaxSpeed;

                _rigidbody.velocity = velocity;
            }
        }

        private void CapLinearAcceleration(float deltaTime, float inverseTime)
        {
            float currentSpeed = _rigidbody.velocity.magnitude;
            float lastSpeed = _oldVelocity.magnitude;

            float speedDelta = (currentSpeed - lastSpeed);
            float currentAcceleration = 0f;

            if (speedDelta > EPSILON)
            {
                currentAcceleration = speedDelta * inverseTime;
            }

            if (currentAcceleration > LinearAcceleration)
            {
                float targetSpeed = LinearAcceleration*deltaTime + lastSpeed;

                Vector2 velocity = _rigidbody.velocity;

                velocity.Normalize();
                velocity *= targetSpeed;

                _rigidbody.velocity = velocity;
            }
        }

        private void CapAngularAcceleration(float deltaTime, float inverseTime)
        {
            float speedDelta = Mathf.Abs(_angularVelocity - _oldAngularVelocity);
            float currentAcceleration = 0f;

            if (speedDelta > EPSILON)
            {
                currentAcceleration = Mathf.Abs(speedDelta)*inverseTime;
            }

            if (currentAcceleration > AngularAcceleration)
            {
                float validAngularVelocity = AngularAcceleration * deltaTime * Mathf.Sign(_angularVelocity) + _oldAngularVelocity;

                if (validAngularVelocity > MaxAngularSpeed)
                {
                    validAngularVelocity = MaxAngularSpeed;
                }
                else if (validAngularVelocity < -MaxAngularSpeed)
                {
                    validAngularVelocity = -MaxAngularSpeed;
                }

                float linearSpeed = _rigidbody.velocity.magnitude;
                float rotation = validAngularVelocity*deltaTime;

                Vector2 movement = _oldVelocity;
                movement.Normalize();
                movement = MathHelper.RotateVector(movement, rotation);
                movement *= linearSpeed;

                _rigidbody.velocity = movement;

                _angularVelocity = validAngularVelocity;
            }
        }

        private void Move(Vector2 velocity)
        {
            _rigidbody.velocity = velocity;
        }

        //public Vector2 GetPosition()
        //{
        //    return _rigidBody.position;
        //}

        public Location GetDynamicLocation()
        {
            _bufferLocation.Set(_rigidbody.transform);

            return _bufferLocation;
        }

        public Location GetInstantLocation()
        {
            _bufferLocation.Set(position);

            return _bufferLocation;
        }

        private void FaceMovementDirection(Vector2 movement)
        {
            // verify if it need actualization, otherwise
            // we keep the old value
            if (movement.sqrMagnitude > EPSILON * EPSILON)
            {
                float orientationInRadian = Mathf.Atan2(movement.y, movement.x);
                _rigidbody.rotation = orientationInRadian * Mathf.Rad2Deg;
            }
        }

        public Vector2 GetOrientationAsVector()
        {
            return MathHelper.GetDirectionFromAngle(orientationInRadian);
        }

        private void ComputeAngularVelocity(float inverseTime)
        {
            float delta = _rigidbody.rotation - _oldOrientation;

            if (-EPSILON < delta || delta > EPSILON)
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
            if (_rigidbody == null)
                return;

            Gizmos.color = Color.magenta;

            Vector2 direction = GetOrientationAsVector();

            Vector2 first = position;
            Vector2 second = first + direction;

            Gizmos.DrawLine(first, second);
        }

        public override string ToString()
        {
            return _rigidbody.name;
        }
    }
}