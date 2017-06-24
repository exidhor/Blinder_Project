using System.Collections;
using System.Collections.Generic;

using Tools;
using MapEditor;
using UnityEngine;

namespace AI
{
    public class PatrolSteering : SeekSteering
    {
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

            if (_currentNodeIndex >= _smoothPath.Count || _smoothPath.Count == 0)
            {
                targetPosition = _target.position;
                return Behavior.Arrive(_character, targetPosition, _specs.radiusMarginError, _specs.speed, _specs.slowRadius);
            }

            targetPosition = _smoothPath[_currentNodeIndex];
            return Behavior.Seek(_character, targetPosition, _specs.speed);
        }
    }
}
