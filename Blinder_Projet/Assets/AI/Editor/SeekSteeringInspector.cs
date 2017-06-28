using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AI;
using MapEditor;
using UnityEditor;
using UnityEngine;

namespace AIEditor
{
    [CustomEditor(typeof(SeekSteering))]
    public class SeekSteeringInspector : Editor
    {
        private SeekSteering _targetObject;

        private static readonly float depth = -3;

        private string _specsName = "";
        private ESteeringType _steeringType;

        void OnEnable()
        {
            _targetObject = (SeekSteering) serializedObject.targetObject;
        }

        [DrawGizmo(GizmoType.NonSelected)]
        static void DrawGizmoNonSelected(SeekSteering seekSteering, GizmoType type)
        {
            DisplayGizmos(seekSteering);
        }

        [DrawGizmo(GizmoType.Selected)]
        static void DrawGizmo(SeekSteering seekSteering, GizmoType type)
        {
            DisplayGizmos(seekSteering);
        }

        private static void DisplayGizmos(SeekSteering seekSteering)
        {
            if (seekSteering.DrawDebugPath)
            {
                Gizmos.color = Color.red;
                DrawPath(seekSteering.GetDebugPath());
            }

            if (seekSteering.DrawDebugSmoothPath)
            {
                Gizmos.color = Color.blue;
                DrawPath(seekSteering.GetSmoothedPath());
            }
        }

        private static void DrawPath(List<Vector2> path)
        {
            if (path != null && path.Count > 0)
            {
                float caseSize = Map.instance.mapData.Grid.CaseSize / 4;
                Vector3 size = new Vector3(caseSize, caseSize, 0.1f);
                Vector3 position = new Vector3(path[0].x, path[0].y, depth);

                Gizmos.DrawCube(position, size);

                for (int i = 1; i < path.Count; i++)
                {
                    Vector3 startPosition = new Vector3(path[i - 1].x, path[i - 1].y, depth);
                    Vector3 endPosition = new Vector3(path[i].x, path[i].y, depth);

                    Gizmos.DrawLine(startPosition, endPosition);

                    Gizmos.DrawCube(endPosition, size);
                }
            }
        }
    }
}
