using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEditor;
using UnityEngine;

namespace ToolsEditor
{
    [CustomEditor(typeof(CameraFollow))]
    class CameraFollowInspector : Editor
    {
        [DrawGizmo(GizmoType.Selected)]
        static void DrawGizmo(CameraFollow cameraFollow, GizmoType type)
        {
            DrawBounds(cameraFollow);
        }

        private static void DrawBounds(CameraFollow cameraFollow)
        {
            Handles.color = Color.blue;

            Handles.DrawWireDisc(cameraFollow.transform.position,
                Vector3.forward, cameraFollow.Bounds);
        }
    }
}
