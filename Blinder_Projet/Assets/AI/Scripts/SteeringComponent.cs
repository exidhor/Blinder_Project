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
        [SerializeField, UnityReadOnly] private SteeringProperties _steeringProperties;

        private Body _body;

        void Awake()
        {
            _body = GetComponent<Body>();
        }

        void Start()
        {
            // tmp
            SetProperties("Human");
            SetSteering(ESteeringType.Seek, new Location(GameManager.instance.player.transform));
        }

        public void ApplyOnBody(float deltaTime)
        {
            // _kinematic.PrepareForUpdate();

            if (_steering != null)
            {
                Profiler.BeginSample("steering");
                _outputBuffer = _steering.GetOutput();
                Profiler.EndSample();
                Profiler.BeginSample("kinematic");
                _body.Actualize(_outputBuffer, deltaTime);
                Profiler.EndSample();
            }
        }

        public void SetProperties(string propertiesName)
        {
            _steeringProperties = SteeringPropertiesTable.instance.GetProperties(propertiesName);

            if (_steeringProperties != null)
            {
                if (_steering)
                {
                    _steering.Init(_body, _steeringProperties, null);
                }
            }
            else
            {
                Debug.LogWarning("Can't find the properties in the table. Verify the name : \"" + propertiesName + "\"");
            }
        }

        public void SetSteering(ESteeringType type, Location target = null)
        {
            if (_steering != null)
            {
                _steering.Release();
            }

            _steering = SteeringTable.instance.GetFreeSteering(type, _body, _steeringProperties, target);

            _body.ResetBody();
        }

        void FixedUpdate()
        {
            Profiler.BeginSample("naviguation");
            ApplyOnBody(Time.fixedDeltaTime);
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
