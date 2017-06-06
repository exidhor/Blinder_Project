using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MemoryManagement.Intern
{
    public abstract class MonoSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<T>();
                    _instance.name = typeof(T).Name;
                }

                return _instance;
            }
        }

        public static T InternalInstance
        {
            get
            {
                return _instance;
            }
        }

        protected virtual void OnEnable()
        {
            // we register this instance if it is created in the editor
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
            }
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}
