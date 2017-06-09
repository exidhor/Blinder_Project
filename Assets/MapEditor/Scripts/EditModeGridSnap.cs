﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MapEditor
{
    [ExecuteInEditMode]
    public class EditModeGridSnap : MonoBehaviour
    {
        public float depth = 0;

        private void Awake()
        {
            Debug.Log("b Awake " + GetInstanceID());
        }

        private void OnEnable()
        {
            Debug.Log("b OnEnable " + GetInstanceID());
        }

        private void OnDisable()
        {
            Debug.Log("b OnDisable " + GetInstanceID());
        }

        private void OnDestroy()
        {
            Debug.Log("b OnDestroy " + GetInstanceID());
        }

        void Update()
        {
            float snapValue = MapEditorModel.Instance.data.CaseSize;

            if (snapValue == 0)
            {
                return;
            }

            float snapInverse = 1 / snapValue;

            float x, y, z;

            // if snapValue = .5, x = 1.45 -> snapInverse = 2 -> x*2 => 2.90 -> round 2.90 => 3 -> 3/2 => 1.5
            // so 1.45 to nearest .5 is 1.5
            x = Mathf.Round(transform.position.x * snapInverse) / snapInverse;
            y = Mathf.Round(transform.position.y * snapInverse) / snapInverse;
            z = depth;  // depth from camera

            transform.position = new Vector3(x, y, z);
        }
    }
}