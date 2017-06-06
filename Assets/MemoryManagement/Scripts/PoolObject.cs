﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MemoryManagement
{
    /// <summary>
    /// An object lived in a pool (created, stored, destroyed)
    /// </summary>
    public class PoolObject : MonoBehaviour
    {
        public Pool Pool;
        public int IndexInPool;

        void Awake()
        {
            Pool = null;
            IndexInPool = -1;
        }

        /// <summary>
        /// Called when the object exit the pool
        /// </summary>
        public virtual void OnPoolExit()
        {
            // nothing
        }

        /// <summary>
        /// Called when the object come back to the pool
        /// </summary>
        public virtual void OnPoolEnter()
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