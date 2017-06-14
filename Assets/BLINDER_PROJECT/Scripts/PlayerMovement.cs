using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;


namespace BlinderProject
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        public float Speed;

        private AxisTrigger _horizontalAction = new AxisTrigger();
        private AxisTrigger _verticalAction = new AxisTrigger();

        private Rigidbody2D _rigidbody;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
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

            _rigidbody.velocity = new Vector2(horizontal, vertical);
        }
    }
}
