using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AI;
using UnityEditor;
using UnityEngine;

namespace AIEditor
{
    [CustomEditor(typeof(SeekSteering))]
    public class SeekSteeringInspector : Editor
    {
        private SeekSteering _targetObject;

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
            List<Vector2> path = seekSteering.GetDebugPath();

            Gizmos.color = Color.red;

            if (path != null)
            {
                for (int i = 1; i < path.Count; i++)
                {
                    Gizmos.DrawLine(path[i-1], path[i]);
                }
            }
        }
    }
}
