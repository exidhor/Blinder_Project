using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OldAI
{
    [RequireComponent(typeof(SteeringComponent))]
    public class SteeringTest : MonoBehaviour
    {
        public EBehavior Behavior;
        public Transform Target;

        private SteeringComponent _steering;
        
        void Awake()
        {
            _steering = GetComponent<SteeringComponent>();
        }

        void FixedUpdate()
        {
            if (Behavior != EBehavior.None && Target != null)
            {
                _steering.ClearBehaviors();

                _steering.AddBehavior(Behavior, new TransformLocation(Target), 1f);

                _steering.ActualizeSteering();
                _steering.ApplySteeringOnKinematic(Time.fixedDeltaTime);
            }
        }
    }
}
