using System.Collections;
using System.Collections.Generic;

using Tools;
using MapEditor;
using UnityEngine;
using System;
using Pathfinding;

namespace AI
{
    public class PatrolSteering : Steering
    {
        [UnityReadOnly]
        public int currentPatrolIndex = 0;

        [UnityReadOnly]
        public Transform currentTransform;

        [Tooltip("List of passage points for the patrol")]
        public Transform[] patrolPoints;

        [Tooltip("Tells if the patrol is repeated")]
        public bool repeat = true;
        public bool DrawDebugPath = true;

        //private List< List <Vector2> > _path       = new List<List<Vector2>>();
        //private List< List <Vector2i> > _coordPath = new List<List<Vector2i>>();

        private int _currentPathIndex        = 0;
        [SerializeField]  private List < Vector2 > _path       = new List<Vector2> ();
        [SerializeField]  private List < Vector2i> _coordPath  = new List<Vector2i>();

        public PatrolSteering()
        {
           // ConstructPath(ref _path, out _coordPath, _character.position, patrolPoints[0].position);
        }

        private int GetNearestPatrolPoint()
        {
            // TODO

            return 0;
        }

        private void ConstructAllPath()
        {
           //for(int nIndex = 0; nIndex < patrolPoints.Length; ++nIndex)
           //{
           //
           //    ConstructPath(_path[nIndex], _coordPath[nIndex], _character.)
           //}
        }

        private void ConstructPath(ref List <Vector2> path, out List<Vector2i> coordPath, Vector2 start, Vector2 end) 
        {
            coordPath = Pathfinder.A_Star(start, end);
            coordPath.Add(Map.instance.navGrid.GetCoordAt(end).Value);

            path.Clear();
            foreach(Vector2i coord in coordPath)
            {
                path.Add(Map.instance.navGrid.GetCasePosition(coord));
            }
        }

        public override SteeringOutput GetOutput()
        {
            Vector2i? currentCoordinates = Map.instance.navGrid.GetCoordAt(_character.position);
            // Vector2i? targetCoordinates =  Map.instance.navGrid.GetCoordAt(patrolPoints[currentIndex].position);

            if(currentCoordinates == null /*|| targetCoordinates == null*/)
            {
                Debug.LogWarning("The character exit the map");
                return new SteeringOutput();
            }

            if(_coordPath.Count == 0)
            {
                return new SteeringOutput();
            }

            if (currentCoordinates == _coordPath[_currentPathIndex])
            {
                _currentPathIndex++;

                if(_currentPathIndex >= _coordPath.Count)
                {
                    currentPatrolIndex++;
                    _currentPathIndex = 0;

                    if (currentPatrolIndex >= patrolPoints.Length)
                    {
                        currentPatrolIndex = 0;
                    }

                    ConstructPath(ref _path, out _coordPath, _character.position, patrolPoints[currentPatrolIndex].position);
                }
            }

            return PrimitiveBehavior.KinematicSeek(_character, _path[_currentPathIndex], _specs.maxSpeed);
        }

        public override void Recompute()
        {
            ConstructPath(ref _path, out _coordPath, _character.position, patrolPoints[0].position);
        }

        public List<Vector2> GetDebugPath()
        {
            return _path;
        }
    }
}
