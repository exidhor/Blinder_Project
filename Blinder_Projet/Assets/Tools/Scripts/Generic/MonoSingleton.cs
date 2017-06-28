using UnityEngine;

namespace Tools
{
    /// <summary>
    /// Use to implement Singleton Pattern with Unity MonoBehaviour
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T _instance;

        /// <summary>
        /// Access to the instance.
        /// This may create the object if there is none.
        /// Use <see cref="internalInstance"/> if you want
        /// to access to the instance whithout modify the value
        /// </summary>
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

        /// <summary>
        /// If you want to check instance without modify it
        /// </summary>
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
