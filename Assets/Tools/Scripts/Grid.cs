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

        private List<List<T>> _cases = new List<List<T>>();

        // serializable data
        [SerializeField] private int _serializableLine;
        [SerializeField] private int _serializableCulumn;
        [SerializeField] private List<T> _serializableCases = new List<T>();

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

        public void OnBeforeSerialize()
        {
            _serializableLine = width;
            _serializableCulumn = height;

            _serializableCases = new List<T>();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _serializableCases.Add(_cases[i][j]);
                }
            }
        }

        public void OnAfterDeserialize()
        {
            _cases = new List<List<T>>();

            for (int i = 0; i < _serializableLine; i++)
            {
                _cases.Add(new List<T>());

                for (int j = 0; j < _serializableCulumn; j++)
                {
                    int index = i*_serializableLine + j;

                    _cases[i].Add(_serializableCases[index]);
                }
            }

            _serializableCases = null;
        }
    }
}