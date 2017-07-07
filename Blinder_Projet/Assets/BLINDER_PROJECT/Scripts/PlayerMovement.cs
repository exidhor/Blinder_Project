using System.Collections;
using System.Collections.Generic;
using AI;
using Tools;
using UnityEngine;


namespace BlinderProject
{
    [RequireComponent(typeof(Body))]
    public class PlayerMovement : MonoBehaviour
    {
        public float Speed;

        private AxisTrigger _horizontalAction = new AxisTrigger();
        private AxisTrigger _verticalAction = new AxisTrigger();

        [SerializeField, UnityReadOnly] private SteeringOutput _steeringOutput; 

        private Body _body;

        void Awake()
        {
            _body = GetComponent<Body>();
        }
        
        void Update()
        {
            HandleInputAction();
        }

        private void HandleInputAction()
        {
            _horizontalAction.axis = Input.GetAxisRaw("Horizontal");
            _verticalAction.axis = Input.GetAxisRaw("Vertical");
        }

        void FixedUpdate()
        {
            float horizontal = _horizontalAction.axis * Speed;
            float vertical = _verticalAction.axis * Speed;

            if (horizontal != 0f && vertical != 0)
            {
                horizontal /= MathHelper.TwoSqrt;
                vertical /= MathHelper.TwoSqrt;
            }

            _steeringOutput.Reset();
            _steeringOutput.IsInstantVelocity = true;

            _steeringOutput.Linear = new Vector2(horizontal, vertical);

            _body.Actualize(_steeringOutput, Speed, Time.fixedDeltaTime);
        }
    }
}
