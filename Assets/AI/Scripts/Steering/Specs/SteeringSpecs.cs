using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemoryManagement;
using UnityEngine;
using Tools;

namespace AI
{
    public class SteeringSpecs : MonoBehaviour
    {
        [SerializeField] private string _name;
        public string name { get { return _name; } }

        // global specs
        //[SerializeField, UnityReadOnly] private Kinematic _character;
        //public Kinematic character { get { return _character; } }

        //[SerializeField] private Location _target;
        //public Location target { get { return _target; } }

        [SerializeField] private float _speed;
        public float speed { get { return _speed; } }

        // Arrive specs
        [SerializeField] private float _radiusMarginError;
        public float radiusMarginError { get { return _radiusMarginError; } }

        [SerializeField] private float _slowRadius;
        public float slowRadius { get { return _slowRadius; } }

        // Pathfinding specs
        [SerializeField] private float _refreshPathTime;
        public float refreshPathTime { get { return _refreshPathTime; } }
    }
}
