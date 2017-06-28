using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tools
{
    public class QuadTreeNode<T>
    {
        public int MAX_OBJECTS = 30;
        public int MAX_LEVELS = 5;

        [SerializeField] private int _level;
        [SerializeField] private List<QTObject<T>> _objects;
        [SerializeField] private Rect _bounds;
        [SerializeField] private QuadTreeNode<T>[] _nodes;

        [SerializeField]
        private List<T> _serializableObjectList;
        [SerializeField]
        private List<Rect> _serializableRectList;

        public QuadTreeNode(int level, Rect bounds)
        {
            _level = level;
            _objects = new List<QTObject<T>>();
            this._bounds = bounds;
            _nodes = new QuadTreeNode<T>[4];
        }

        /*!
         * \brief   clears the quadtree by recursively 
         *          clearing all objects from all nodes.
        */
        public void Clear()
        {
            _objects.Clear();

            for (int i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i] != null)
                {
                    _nodes[i].Clear();
                    _nodes[i] = null;
                }
            }
        }

        /*!
         * \brief   splits the node into four subnodes by dividing 
         *          the node into four equal parts and initializing 
         *          the four subnodes with the new bounds.
         */
        private void Split()
        {
            int subWidth = (int)(_bounds.width / 2);
            int subHeight = (int)(_bounds.height / 2);
            int x = (int)_bounds.x;
            int y = (int)_bounds.y;

            _nodes[0] = new QuadTreeNode<T>(_level + 1,
                new Rect(x + subWidth, y, subWidth, subHeight));
            _nodes[1] = new QuadTreeNode<T>(_level + 1,
                new Rect(x, y, subWidth, subHeight));
            _nodes[2] = new QuadTreeNode<T>(_level + 1,
                new Rect(x, y + subHeight, subWidth, subHeight));
            _nodes[3] = new QuadTreeNode<T>(_level + 1,
                new Rect(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        /*
         * \brief   Determine which node the object belongs to.
         * \param   a_Rect The hitbox of the object.
         * \return  The index of the QuadTree which the node belong to, or -1
         *          if there is no completely matching
         */
        private int GetIndex(Rect rect)
        {
            int index = -1;
            double verticalMidPoint = _bounds.x + (_bounds.width / 2);
            double horizontalMidPoint = _bounds.y + (_bounds.height / 2);

            // Object can completely fit within the top quadrants
            bool topQuadrant = (rect.y < horizontalMidPoint
                && rect.y + rect.height < horizontalMidPoint);

            // Object can completely fit within the bottom quadrants
            bool botQuadrant = (rect.y > horizontalMidPoint);

            if (rect.x < verticalMidPoint && rect.x + rect.width < verticalMidPoint)
            {
                if (topQuadrant)
                    index = 1;

                else if (botQuadrant)
                    index = 2;
            }

            // Object can completely fit within the right quadrants
            else if (rect.x > verticalMidPoint)
            {
                if (topQuadrant)
                    index = 0;

                else if (botQuadrant)
                    index = 3;
            }

            return index;
        }

        public void Insert(T obj, Rect rect)
        {
            Insert(new QTObject<T>(obj, rect));
        }

        /*
         * \brief   Insert the object into the quadtree. If the node
         *          exceeds the capacity, it will split and add all
         *          objects to their corresponding nodes.
         * \param   a_Object The object to insert
         */
        private void Insert(QTObject<T> obj)
        {
            if (_nodes[0] != null)
            {
                int index = GetIndex(obj.rect);

                if (index != -1)
                {
                    _nodes[index].Insert(obj);
                    return;
                }
            }

            _objects.Add(obj);

            if (_objects.Count > MAX_OBJECTS && _level < MAX_LEVELS)
            {
                if (_nodes[0] == null)
                    Split();

                int i = 0;

                while (i < _objects.Count)
                {
                    int index = GetIndex(_objects[i].rect);
                    if (index != -1)
                    {
                        QTObject<T> qtObject = _objects[i];
                        _objects.RemoveAt(i);
                        _nodes[index].Insert(qtObject);
                    }
                    else
                    {
                        i++;
                    }

                }
            }
        }

        /*
         * \brief   Find all potential collisions for specific bounds
         * \param   a_ReturnObject the list of QTObject in collision
         *          with the specific bounds.
         * \param   a_Rect the specific bounds (i.e. the bounds of the
         *          object we want to test)
         * \return  the list of collided objects (use for recursive operation),
         *          which be the same as a_ReturnObjects at the end.
         */
        public List<QTObject<T>> Retrieve(List<QTObject<T>> returnObjects, Rect rect)
        {
            if (_nodes[0] != null)
            {
                int index = GetIndex(rect);

                if (index != -1)
                {
                    _nodes[index].Retrieve(returnObjects, rect);
                }
                else
                {
                    for (int i = 0; i < _nodes.Length; i++)
                    {
                        _nodes[i].Retrieve(returnObjects, rect);
                    }
                }
            }

            returnObjects.AddRange(_objects);

            return returnObjects;
        }
    }
}
