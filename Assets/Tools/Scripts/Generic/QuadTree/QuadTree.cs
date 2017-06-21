using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Tools
{
    /*
     * \brief   A class which provide a first traitment
     *          to accelerate collision computing in 2D.
     */
    [Serializable]
    public class QuadTree<T> : ISerializationCallbackReceiver
    {
        public int MAX_OBJECTS = 30;
        public int MAX_LEVELS = 5;

        private QuadTreeNode<T> _root;

        public QuadTree(Rect bounds)
        {
            _root = new QuadTreeNode<T>(0, bounds);
        }

        /*!
         * \brief   clears the quadtree by recursively 
         *          clearing all objects from all nodes.
        */
        public void Clear()
        {
            if (_root != null)
            {
                _root.Clear();
            }
        }

        public void Insert(T obj, Rect rect)
        {
            if (_root != null)
            {
                _root.Insert(obj, rect);
            }
        }

        public List<T> Retrieve(Rect rect)
        {
            List<T> returnObject = new List<T>();

            if (_root != null)
            {
                List<QTObject<T>> collidedObject = new List<QTObject<T>>();

                _root.Retrieve(collidedObject, rect);

                for (int i = 0; i < collidedObject.Count; i++)
                {
                    returnObject.Add(collidedObject[i].obj);
                }    
            }

            return returnObject;
        }

        public void OnBeforeSerialize()
        {
            // todo
        }

        public void OnAfterDeserialize()
        {
            // todo
        }
    }
}