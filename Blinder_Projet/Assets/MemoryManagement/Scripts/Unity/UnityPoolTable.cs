using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace MemoryManagement
{
    /// <summary>
    /// The pool manager, used to find a pool, or create
    /// some new pools
    /// </summary>
    public sealed class UnityPoolTable : MonoSingleton<UnityPoolTable>
    {
        private Dictionary<int, UnityPool> _table;

        private void Awake()
        {
            _table = new Dictionary<int, UnityPool>();
        }

        /// <summary>
        /// Create a new pool for the model
        /// The model has to be new, its instanceId will
        /// be used to create a dictionary entry
        /// </summary>
        /// <param name="model">the model of the pool</param>
        /// <param name="poolSize">The number of element in the pool</param>
        /// <param name="expandSize">By how many the pool has to increase</param>
        /// <returns>The new pool</returns>
        public UnityPool AddPool(UnityPoolObject model, uint poolSize, uint expandSize = 1)
        {
            UnityPool newPool = InstanciatePool(model, expandSize);
            newPool.SetSize(poolSize);

            _table.Add(model.GetInstanceID(), newPool);

            return newPool;
        }

        /// <summary>
        /// Create a new pool for the model
        /// The model has to be new, its instanceId will
        /// be used to create a dictionary entry
        /// </summary>
        /// <param name="poolEntry"></param>
        /// <returns></returns>
        public UnityPool AddPool(UnityPoolEntry poolEntry)
        {
            return AddPool(poolEntry.Prefab,
                poolEntry.PoolSize,
                poolEntry.ExpandPoolSize);
        }

        /// <summary>
        /// Check if there is already a pool with this model
        /// </summary>
        /// <param name="model"></param>
        /// <returns>True if there is one pool with this model, false otherwise</returns>
        public bool Contains(UnityPoolObject model)
        {
            return _table.ContainsKey(model.GetInstanceID());
        }

        /// <summary>
        /// Get the pool from the instanceID of the model
        /// </summary>
        /// <param name="instanceID">The instanceID of the pool model</param>
        /// <returns>The pool</returns>
        public UnityPool GetPool(int instanceID)
        {
            return _table[instanceID];
        }

        /// <summary>
        /// Instanciate a new pool
        /// </summary>
        /// <param name="model">The model of the new pool</param>
        /// <param name="expandSize">By how many the pool has to increase</param>
        /// <returns></returns>
        private UnityPool InstanciatePool(UnityPoolObject model, uint expandSize)
        {
            GameObject poolGameObject = new GameObject();
            poolGameObject.transform.parent = gameObject.transform;
            UnityPool pool = poolGameObject.AddComponent<UnityPool>();

            pool.transform.parent = gameObject.transform;

            pool.Construct(model, expandSize);

            return pool;
        }
    }
}