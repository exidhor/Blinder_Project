using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlinderProject;
using MapEditor;
using Pathfinding;
using Tools;
using UnityEngine;

namespace AI
{
    public class SeekSteering : Steering
    {
        //[SerializeField] private Location _target;

        [SerializeField] private float _currentTime;
        //[SerializeField] private float _refreshPathTime;

        // for the path
        [SerializeField] private int _currentNodeIndex;
        [SerializeField] private List<Vector2> _path = new List<Vector2>();
        [SerializeField] private List<Vector2i> _coordPath = new List<Vector2i>();

        //// specs
        //[SerializeField] private float _speed;
        //[SerializeField] private float _radiusMarginError;
        //[SerializeField] private float _slowRadius;

        //public void Init(Kinematic character, 
        //    Location target, 
        //    float refreshPathTime,
        //    float speed,
        //    float radiusMarginError,
        //    float slowRadius)
        //{
        //    _character = character;
        //    _target = target;

        //    _refreshPathTime = refreshPathTime;
        //    _currentTime = RandomGenerator.instance.NextFloat(_refreshPathTime);

        //    _speed = speed;
        //    _radiusMarginError = radiusMarginError;
        //    _slowRadius = slowRadius;

        //    _type = ESteeringType.Seek;

        //    ConstructPath();
        //}

        public override void Recompute()
        {
            ConstructPath();
        }

        public override SteeringOutput GetOutput()
        {
            if (!_target.isSet)
            {
                return new SteeringOutput();
            }

            // check if the character has reached the targetNode
            if (_currentNodeIndex >= 0 && _path.Count > 0)
            {
                Vector2i? coord = Map.instance.currentNavGrid.GetCoordAt(_character.GetPosition());

                if (!coord.HasValue)
                {
                    Debug.LogWarning("The character exit the map");
                    _path.Clear();
                    _currentNodeIndex = 0;

                    return new SteeringOutput();
                }

                if (_currentNodeIndex < 0 || _currentNodeIndex >= _coordPath.Count)
                {
                    Debug.Break();
                }

                if (_coordPath[_currentNodeIndex] == coord.Value)
                {
                    _currentNodeIndex--;
                }
            }

            Vector2 targetPosition;

            if (_currentNodeIndex < 0 || _path.Count == 0)
            {
                targetPosition = _target.position;
            }
            else
            {
                targetPosition = _path[_currentNodeIndex];
            }

            return Behavior.Arrive(_character, targetPosition, _specs.radiusMarginError, _specs.speed, _specs.slowRadius);
        }

        void Update()
        {
            _currentTime += Time.deltaTime;

            if (_specs != null && _currentTime > _specs.refreshPathTime)
            {
                RefreshPath();
            }
        }

        private void RefreshPath()
        {
            _currentTime = 0;
            ConstructPath();
        }

        private void ConstructPath()
        {
            _path.Clear();
            _currentNodeIndex = 0;

            if (!_target.isSet)
                return;

            NavGrid navGrid = Map.instance.currentNavGrid;

            Vector2i? startCoord = navGrid.GetCoordAt(_character.GetPosition());
            Vector2i? endCoord = navGrid.GetCoordAt(_target.position);

            if (!startCoord.HasValue || !endCoord.HasValue)
            {
                return;
            }

            _coordPath = Pathfinder.A_Star(startCoord.Value, endCoord.Value);

            for (int i = 0; i < _coordPath.Count; i++)
            {
                _path.Add(navGrid.GetCasePosition(_coordPath[i]));
            }

            _currentNodeIndex = _coordPath.Count - 1;
        }

        public List<Vector2> GetDebugPath()
        {
            return _path;
        }
    }
}