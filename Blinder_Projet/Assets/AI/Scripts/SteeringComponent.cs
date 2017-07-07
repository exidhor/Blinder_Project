using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlinderProject;
using UnityEngine;
using Tools;
using UnityEngine.Profiling;

namespace AI
{
    [RequireComponent(typeof(Body))]
    public class SteeringComponent : MonoBehaviour
    {
        [SerializeField, UnityReadOnly] private SteeringOutput _outputBuffer;
        [SerializeField, UnityReadOnly] private Steering _steering;
        [SerializeField, UnityReadOnly] private SteeringSpecs _steeringSpecs;

        private Body _kinematic;

        void Awake()
        {
            _kinematic = GetComponent<Body>();
        }

        void Start()
        {
            // tmp
            SetSpecs("Human");
            SetSteering(ESteeringType.Seek, new Location(GameManager.instance.player.transform));
        }

        public void ApplyOnKinematic(float deltaTime)
        {
            // _kinematic.PrepareForUpdate();

            if (_steering != null)
            {
                Profiler.BeginSample("steering");
                _outputBuffer = _steering.GetOutput();
                Profiler.EndSample();
                Profiler.BeginSample("kinematic");
                _kinematic.Actualize(_outputBuffer, deltaTime);
                Profiler.EndSample();
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

            _kinematic.ResetBody();
        }

        void FixedUpdate()
        {
            Profiler.BeginSample("naviguation");
            ApplyOnKinematic(Time.fixedDeltaTime);
            Profiler.EndSample();
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
