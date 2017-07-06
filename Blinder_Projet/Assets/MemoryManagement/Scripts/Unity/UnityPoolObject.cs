using UnityEngine;

namespace MemoryManagement
{
    /// <summary>
    /// An object lived in a pool (created, stored, destroyed)
    /// </summary>
    public class UnityPoolObject : MonoBehaviour
    {
        public UnityPool Pool;
        public int IndexInPool;

        void Awake()
        {
            Pool = null;
            IndexInPool = -1;
        }

        /// <summary>
        /// Called when the object exit the pool
        /// </summary>
        protected internal virtual void OnPreUsing()
        {
            // nothing
        }

        /// <summary>
        /// Called when the object come back to the pool
        /// </summary>
        public virtual void OnRelease()
        {
            // nothing
        }

        /// <summary>
        /// Release the object, it comes back to the pool
        /// </summary>
        public void Release()
        {
            if (Pool  != null)
            {
                 Pool.ReleaseResource(this);
            }
        }

        public override string ToString()
        {
            return name + " ( pool : " + Pool.name + " )";
        }
    }
}