using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tools
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T _instance;

        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    // we first try to find if there is already an GameObject in the scene 
                    _instance = GameObject.FindObjectOfType<T>();

                    if(_instance == null)
                    {
                        // if not, we create one
                        GameObject go = new GameObject();
                        _instance = go.AddComponent<T>();
                        _instance.name = typeof(T).Name;
                    }
                }

                return _instance;
            }
        }

        public static T internalInstance
        {
            get
            {
                return _instance;
            }
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}
