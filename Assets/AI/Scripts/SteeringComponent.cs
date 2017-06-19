﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Tools;

namespace AI
{
    [RequireComponent(typeof(Kinematic))]
    public class SteeringComponent : MonoBehaviour
    {
        [SerializeField, UnityReadOnly] private SteeringOutput _outputBuffer;
        [SerializeField, UnityReadOnly] private Steering _steering;
        [SerializeField, UnityReadOnly] private SteeringSpecs _steeringSpecs;

        private Kinematic _kinematic;

        private void Awake()
        {
            _kinematic = GetComponent<Kinematic>();
        }

        public void ApplyOnKinematic(float deltaTime)
        {
            _kinematic.ResetVelocity();

            if (_steering != null)
            {
                _outputBuffer = _steering.GetOutput();
                _kinematic.Actualize(_outputBuffer, deltaTime);
            }
        }

        public void SetSpecs(string specsName)
        {
            _steeringSpecs = SteeringSpecsTable.instance.GetSpecs(specsName);

            if (_steeringSpecs != null)
            {
                if (_steering)
                {
                    _steering.Init(_kinematic, _steeringSpecs, null);
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

            _steering = SteeringTable.instance.GetFreeSteering(type, _kinematic, _steeringSpecs, target);
        }

        void Update()
        {
            ApplyOnKinematic(Time.deltaTime);
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
