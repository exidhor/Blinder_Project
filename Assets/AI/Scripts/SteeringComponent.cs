using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Tools;

namespace AI
{
    [RequireComponent(typeof(StaticBody))]
    public class SteeringComponent : MonoBehaviour
    {
        [SerializeField, UnityReadOnly] private SteeringOutput _outputBuffer;
        [SerializeField, UnityReadOnly] private Steering _steering;
        [SerializeField, UnityReadOnly] private SteeringSpecs _steeringSpecs;

        private StaticBody _staticBody;

        private void Awake()
        {
            _staticBody = GetComponent<StaticBody>();
        }

        public void ApplyOnStatic(float deltaTime)
        {
            _staticBody.ResetVelocity();

            if (_steering != null)
            {
                _outputBuffer = _steering.GetOutput();
                _staticBody.Actualize(_outputBuffer, deltaTime);
            }
        }

        public void SetSpecs(string specsName)
        {
            _steeringSpecs = SteeringSpecsTable.instance.GetSpecs(specsName);

            if (_steeringSpecs != null)
            {
                if (_steering)
                {
                    _steering.Init(_staticBody, _steeringSpecs, null);
                }
            }
            else
            {
                Debug.LogWarning("Can't find the specs in the table. Verify the name : \"" + specsName + "\"");
            }
        }

        public void SetSteering(ESteeringType type, Location target = null)
        {
            if (_steering != null)
            {
                _steering.Release();
            }

            _steering = SteeringTable.instance.GetFreeSteering(type, _staticBody, _steeringSpecs, target);
        }

        void Update()
        {
            ApplyOnStatic(Time.deltaTime);
        }

        /// <summary>
        /// Used by the editor
        /// </summary>
        public void RefreshSteering()
        {
            _steering.Recompute();
        }
    }
}
