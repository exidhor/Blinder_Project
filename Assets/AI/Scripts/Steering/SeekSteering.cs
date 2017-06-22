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

        public bool DrawDebugSmoothPath = true;
        public bool DrawDebugPath = true;

        [SerializeField, UnityReadOnly] private float _currentTime;

        // for the path
        [SerializeField] private int _currentNodeIndex;
        [SerializeField] private List<Vector2> _path = new List<Vector2>();
        [SerializeField] private List<Vector2> _smoothPath = new List<Vector2>();
        [SerializeField] private List<Vector2i> _smoothCoordPath = new List<Vector2i>();
        [SerializeField] private List<Vector2i> _coordPath = new List<Vector2i>();

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
            if (_currentNodeIndex < _smoothPath.Count && _smoothPath.Count > 0)
            {
                Vector2i? coord = Map.instance.navGrid.GetCoordAt(_character.GetPosition());

                if (!coord.HasValue)
                {
                    Debug.LogWarning("The character exit the map");
                    _smoothPath.Clear();
                    _currentNodeIndex = 0;

                    return new SteeringOutput();
                }

                if (_currentNodeIndex < 0 || _currentNodeIndex >= _smoothCoordPath.Count)
                {
                    Debug.Break();
                }

                if (_smoothCoordPath[_currentNodeIndex] == coord.Value)
                {
                    _currentNodeIndex++;
                }
            }

            Vector2 targetPosition;

            if (_currentNodeIndex >= _smoothPath.Count || _smoothPath.Count == 0)
            {
                targetPosition = _target.position;
            }
            else
            {
                targetPosition = _smoothPath[_currentNodeIndex];
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
            _smoothPath.Clear();

            _currentNodeIndex = 0;

            if (!_target.isSet)
                return;

            NavGrid navGrid = Map.instance.navGrid;

            _coordPath = Pathfinder.A_Star(_character.GetPosition(), _target.position);

            for (int i = 0; i < _coordPath.Count; i++)
            {
                _path.Add(navGrid.GetCasePosition(_coordPath[i]));
            }

            //_currentNodeIndex = _coordPath.Count - 1;

            _smoothCoordPath = PathSmoother.SmoothPath(_coordPath);

            for (int i = 0; i < _smoothCoordPath.Count; i++)
            {
                _smoothPath.Add(navGrid.GetCasePosition(_smoothCoordPath[i]));
            }
        }

        public List<Vector2> GetDebugPath()
        {
            return _path;
        }

        public List<Vector2> GetSmoothedPath()
        {
            return _smoothPath;
        }
    }
}