﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemoryManagement;
using UnityEngine;
using Tools;

namespace AI
{
    public class SteeringProperties : MonoBehaviour
    {
        // Global properties
        [SerializeField] private string _name;
        public string name { get { return _name; } }

        [SerializeField] private float _maxSpeed;
        public float maxSpeed { get { return _maxSpeed; } }

        [SerializeField] private float _maxAcceleration;
        public float maxAcceleration {  get { return _maxAcceleration; } }

        [SerializeField] private float _maxRotation;
        public float maxRotation { get { return _maxRotation; } }

        [SerializeField] private float _maxAngularAcceleration;
        public float maxAngularAcceleration { get { return _maxAngularAcceleration; } }

        // Arrive properties
        [SerializeField] private float _targetRadius;
        public float targetRadius { get { return _targetRadius; } }

        [SerializeField] private float _slowRadius;
        public float slowRadius { get { return _slowRadius; } }

        [SerializeField] private float _timeToTarget;
        public float timeToTarget { get { return _timeToTarget; } }

        // Face properties
        [SerializeField] private float _targetAngularRadius;
        public float targetAngularRadius { get { return _targetAngularRadius; } }

        [SerializeField] private float _slowAngularRadius;
        public float slowAngularRadius { get { return _slowAngularRadius; } }

        // Pathfinding properties
        [SerializeField] private float _refreshPathTime;
        public float refreshPathTime { get { return _refreshPathTime; } }

        // Pursue properties
        [SerializeField] private float _maxPredictionTime;
        public float maxPredictionTime { get { return _maxPredictionTime; } }
    }
}
