﻿using System.Collections.Generic;
using MapEditor;
using Pathfinding;
using Tools;
using UnityEngine;
using UnityEngine.Profiling;

namespace AI
{
    public class SeekSteering : Steering
    {
        public bool DrawDebugSmoothPath = true;
        public bool DrawDebugPath = true;

        public bool TargetIsTheEnd = true;

        [SerializeField, UnityReadOnly] private float _currentTime;

        // for the path
        [SerializeField] protected int _currentNodeIndex;
        [SerializeField] protected List<Vector2> _path = new List<Vector2>();
        [SerializeField] protected List<Vector2> _smoothPath = new List<Vector2>();
        [SerializeField] protected List<Vector2i> _smoothCoordPath = new List<Vector2i>();
        [SerializeField] protected List<Vector2i> _coordPath = new List<Vector2i>();

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
                Vector2i? coord = Map.instance.navGrid.GetCoordAt(_character.position);

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

            SteeringOutput output;

            if (_currentNodeIndex >= _smoothPath.Count || _smoothPath.Count == 0)
            {
                targetPosition = _target.position;

                if (TargetIsTheEnd)
                {
                     output = PrimitiveBehavior.Arrive(_character,
                                            targetPosition,
                                            _properties);
                }
                else
                {
                    output = PrimitiveBehavior.Seek(_character, targetPosition, _properties);
                }
            }
            else
            {
                targetPosition = _smoothPath[_currentNodeIndex];

                output = PrimitiveBehavior.Seek(_character, targetPosition, _properties);
            }

            output.StopRotation = true;

            return output;
        }

        void Update()
        {
            _currentTime += Time.deltaTime;

            if (_properties != null && _currentTime > _properties.refreshPathTime)
            {
                RefreshPath();
            }
        }

        private void RefreshPath()
        {
            Profiler.BeginSample("pathfinding");
            _currentTime = 0;
            ConstructPath();
            Profiler.EndSample();
        }

        private void ConstructPath()
        {
            _path.Clear();
            _smoothPath.Clear();

            _currentNodeIndex = 0;

            if (!_target.isSet)
                return;

            NavGrid navGrid = Map.instance.navGrid;

            // We first try to see if we can reach the target straightly without 
            // needed a pathfinding

            // to see that, we look for a clear line between the character and the target
            if (CanReachTargetDirectly())
                return;

            Profiler.BeginSample("A*");
            _coordPath = Pathfinder.A_Star(_character.position, _target.position);
            Profiler.EndSample();

            for (int i = 0; i < _coordPath.Count; i++)
            {
                _path.Add(navGrid.GetCasePosition(_coordPath[i]));
            }

            Profiler.BeginSample("smooth");
            _smoothCoordPath = PathSmoother.SmoothPath(_coordPath);
            Profiler.EndSample();

            for (int i = 0; i < _smoothCoordPath.Count; i++)
            {
                _smoothPath.Add(navGrid.GetCasePosition(_smoothCoordPath[i]));
            }
        }

        private bool CanReachTargetDirectly()
        {
            NavGrid navGrid = Map.instance.navGrid;

            Vector2i? characterCoord = navGrid.GetCoordAt(_character.position);
            Vector2i? targetCoord = navGrid.GetCoordAt(_target.position);

            if (characterCoord == null || targetCoord == null)
            {
                return false;
            }

            return navGrid.IsClearLine(characterCoord.Value, targetCoord.Value);
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