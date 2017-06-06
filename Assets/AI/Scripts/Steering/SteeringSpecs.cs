using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    [Serializable]
    public class SteeringSpecs
    {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _minTimeToTarget;
        [SerializeField] private float _radiusMarginError;
        [SerializeField] private float _slowArriveRadius;
        [SerializeField] private float _maxPredictionTime;
        [SerializeField] private float _maxWanderOffset;
        [SerializeField] private float _wanderRadius;
        [SerializeField] private float _wanderRate;
        [SerializeField] private float _avoidanceRadius;

        // variable which is stored and modify
        public float WanderOrientation;
        //public List<Node> PathToFollow;
        public int LastNodeIndex;

        //private Kinematic _kinematic;
        //private Location _targetLocation;

        public float MaxSpeed
        {
            get { return _maxSpeed; }
        }

        public float MinTimeToTarget
        {
            get { return _minTimeToTarget; }
        }

        public float RadiusMarginError
        {
            get { return _radiusMarginError; }
        }

        public float SlowArriveRadius
        {
            get { return _slowArriveRadius; }
        }

        public float MaxPredictionTime
        {
            get { return _maxPredictionTime; }
        }

        public float MaxWanderOffset
        {
            get { return _maxWanderOffset; }
        }

        public float WanderRadius
        {
            get { return _wanderRadius; }
        }

        public float WanderRate
        {
            get { return _wanderRate; }
        }

        public float AvoidanceRadius
        {
            get { return _avoidanceRadius; }
        }

        //public Vector2 TargetPosition
        //{
        //    get
        //    {
        //        if (_kinematic != null)
        //        {
        //            return _kinematic.GetPosition();
        //        }

        //        return _targetLocation.GetPosition();
        //    }
        //}

        //public Vector2 TargetVelocity
        //{
        //    get
        //    {
        //        return _kinematic.GetVelocity();
        //    }
        //}
    }
}