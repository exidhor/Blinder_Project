using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public class Grid<T> : ScriptableObject, ISerializationCallbackReceiver
    {
        public int width
        {
            get { return _cases.Count; }
        }

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

        public int count
        {
            get { return width*height; }
        }

        // source : https://stackoverflow.com/questions/287928/how-do-i-overload-the-square-bracket-operator-in-c
        public List<T> this[int i]
        {
            get { return _cases[i]; }
            set { _cases[i] = value; }
        }

        protected List<List<T>> _cases = new List<List<T>>();

        // serializable data
        [SerializeField] protected int _serializableLine;
        [SerializeField] protected int _serializableCulumn;
        [SerializeField] protected List<T> _serializableCases = new List<T>();
        [SerializeField] protected List<bool> _serializableNullArray = new List<bool>(); // to handle null ref serialization

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

        public virtual void Copy<U>(Grid<U> otherGrid)
        {
            Resize(otherGrid.width, otherGrid.height);
        }

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

        public bool IsValidCoord(Vector2i coord)
        {
            return IsValidCoord(coord.x, coord.y);
        }

        public bool IsValidCoord(int x, int y)
        {
            return 0 <= x && x < width
                   && 0 <= y && y < height;
        }

        public T GetCaseAt(Vector2i coord)
        {
            return this[coord.x][coord.y];
        }

        public T GetCaseAt(int x, int y)
        {
            return this[x][y];
        }
    }
}