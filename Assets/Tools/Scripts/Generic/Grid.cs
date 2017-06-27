using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tools
{
    /// <summary>
    /// This is a basic grid, with no size, which can be store
    /// in an asset.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Grid<T> : ScriptableObject, ISerializationCallbackReceiver
    {
        /// <summary>
        /// also the line size or the number of culumns
        /// </summary>
        public int width
        {
            get { return _cases.Count; }
        }

        /// <summary>
        /// also the culum size or the number of line
        /// </summary>
        public int height
        {
            get
            {
                if (_cases.Count == 0)
                {
                    return 0;
                }

                return _cases[0].Count;
            }
        }

        /// <summary>
        /// The number of cases in the grid
        /// </summary>
        public int count
        {
            get { return width*height; }
        }

        public List<T> this[int i]
        {
            get { return _cases[i]; }
            set { _cases[i] = value; }
        }

        protected List<List<T>> _cases = new List<List<T>>();

        // serializable data, used to display grid content in inspector
        // and store it into assets
        [SerializeField] protected int _serializableLine;
        [SerializeField] protected int _serializableCulumn;
        [SerializeField] protected List<T> _serializableCases = new List<T>();
        [SerializeField] protected List<bool> _serializableNullArray = new List<bool>(); // to handle null ref serialization

        /// <summary>
        /// Clear all the cases with the default value
        /// </summary>
        /// <param name="resetValue">The value to set to every case</param>
        public void Clear(T resetValue = default(T))
        {
            for (int i = 0; i < _cases.Count; i++)
            {
                for (int j = 0; j < _cases[i].Count; j++)
                {
                    _cases[i][j] = resetValue;
                }
            }
        }

        /// <summary>
        /// Resize this grid, from the size of the other grid
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="otherGrid">The other grid</param>
        public virtual void ResizeFrom<U>(Grid<U> otherGrid)
        {
            Resize(otherGrid.width, otherGrid.height);
        }

        /// <summary>
        /// Resize the grid, removing or adding the smallest
        /// amount of case
        /// </summary>
        /// <param name="width">The new width</param>
        /// <param name="height">The new height</param>
        /// <param name="defaultValue">The value for the new case</param>
        public void Resize(int width, int height, T defaultValue = default(T))
        {
            bool removeWidth = this.width > width;
            bool removeHeight = this.height > height;

            if (removeWidth)
            {
                int delta = this.width - width;
                _cases.RemoveRange(width, delta);
            }

            if (removeHeight)
            {
                int delta = this.height - height;

                for (int i = 0; i < _cases.Count; i++)
                {
                    _cases[i].RemoveRange(height, delta);
                }
            }

            if (!removeWidth)
            {
                int countToAdd = width - this.width;

                for (int i = 0; i < countToAdd; i++)
                {
                    _cases.Add(new List<T>(height));

                    if (removeHeight)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            _cases.Last().Add(defaultValue);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < this.height; j++)
                        {
                            _cases.Last().Add(defaultValue);
                        }
                    }
                }
            }

            if (!removeHeight)
            {
                int countToAdd = height - this.height;

                for (int i = 0; i < _cases.Count; i++)
                {
                    for (int j = 0; j < countToAdd; j++)
                    {
                        _cases[i].Add(defaultValue);
                    }
                }
            }
        }

        /// <summary>
        /// Callback method from Unity to ensure our own serialization.
        /// [Unity can't handle nested list]
        /// We store all the data on a single list before the serialization,
        /// and set a flag on another list (to handle null ref)
        /// </summary>
        public virtual void OnBeforeSerialize()
        {
            _serializableLine = width;
            _serializableCulumn = height;

            _serializableCases = new List<T>();
            _serializableNullArray = new List<bool>();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _serializableCases.Add(_cases[i][j]);

                    _serializableNullArray.Add(EqualityComparer<T>.Default.Equals(_cases[i][j], default(T)));
                }
            }
        }


        /// <summary>
        /// Callback method from Unity to ensure our own serialization.
        /// [Unity can't handle nested list]
        /// After the serialization, we put the data from the single list into
        /// the nested list.
        /// </summary>
        public virtual void OnAfterDeserialize()
        {
            _cases = new List<List<T>>();

            for (int i = 0; i < _serializableLine; i++)
            {
                _cases.Add(new List<T>());

                for (int j = 0; j < _serializableCulumn; j++)
                {
                    int index = i*_serializableLine + j;

                    if (_serializableNullArray[index])
                    {
                        _cases[i].Add(default(T));
                    }
                    else
                    {
                        _cases[i].Add(_serializableCases[index]);
                    }
                }
            }

            _serializableCases = null;
        }

        /// <summary>
        /// Is the coord is inside the grid
        /// </summary>
        /// <param name="coord">The coord to verify</param>
        /// <returns>True if the coord is correct, false otherwise</returns>
        public bool IsValidCoord(Vector2i coord)
        {
            return IsValidCoord(coord.x, coord.y);
        }

        /// <summary>
        /// Is the coord is inside the grid
        /// </summary>
        /// <param name="x">x to verify</param>
        /// <param name="y">y to verify</param>
        /// <returns>True if the coord is correct, false otherwise</returns>
        public bool IsValidCoord(int x, int y)
        {
            return 0 <= x && x < width
                   && 0 <= y && y < height;
        }

        /// <summary>
        /// Return the case at the given coord
        /// </summary>
        /// <param name="coord">The case coord</param>
        /// <returns></returns>
        public T GetCaseAt(Vector2i coord)
        {
            return GetCaseAt(coord.x, coord.y);
        }

        /// <summary>
        /// Return the case at the given coord
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public T GetCaseAt(int x, int y)
        {


#if UNITY_EDITOR // debug info

            if (x < 0 || x >= _cases.Count
                || y < 0 || y >= _cases[x].Count)
            {
                Debug.LogError("invalid indices : x(" + x + "), y(" + y + "); Width = " + width + ", height = " + height);
            }
#endif

            return this[x][y];
        }
    }
}