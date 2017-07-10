using System;

namespace Tools
{
    // source : http://allanrbo.blogspot.fr/2011/12/simple-heap-implementation-priority.html
    public class MinHeap<T> where T : new()
    {
        // add (node, priority)    -- C5 O(log(n))
        // Count
        // DeleteMinPriority ()    -- C5 O(log(n)) (FindMin take O(1) time)
        // Update (node, priority) -- C5 O(log(n)) 

        // Reset ()

        // + NO ALLOC

        public int count
        {
            get { return _count; }
        }

        private MinHeapNode<T>[] _array;

        private int _count;
        private float _increaseSizeFactor;

        public MinHeap(int capacity, float increaseSizeFactor)
        {
            _array = new MinHeapNode<T>[capacity];

            InitRange(0, capacity);

            _count = 0;
            _increaseSizeFactor = increaseSizeFactor;
        }

        private void InitRange(int from, int to)
        {
            for (int i = from; i < to; i++)
            {
                _array[i] = new MinHeapNode<T>();
            }
        }

        public void Insert(T o)
        {
            _array[_count].Data = o;

            IncreaseSize();

            int i = _count - 1;
            while (i > 0)
            {
                int j = (i + 1) / 2 - 1;

                // Check if the invariant holds for the element in data[i]
                MinHeapNode<T> v = _array[j];
                if (v.Priority <= _array[i].Priority)
                {
                    break;
                }

                // Swap the elements
                Swap(i, j);

                i = j;
            }
        }

        public T ExtractMin()
        {
            if (_count < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            MinHeapNode<T> min = _array[0];
            _array[0] = _array[_count - 1];

            RemoveLast();

            MinHeapify(0);

            return min.Data;
        }

        public void DecreaseKeyForMin(float newValue)
        {
            _array[0].Priority = newValue;

            int i = 0;

            while (i != 0 && _array[GetParentIndex(i)].Priority > _array[i].Priority)
            {
                int indexParent = GetParentIndex(i);

                Swap(i, indexParent);
                i = indexParent;
            }
        }

        private void Swap(int firstIndex, int secondIndex)
        {
            MinHeapNode<T> tmp = _array[firstIndex];
            _array[firstIndex] = _array[secondIndex];
            _array[secondIndex] = tmp;
        }

        private int GetParentIndex(int childIndex)
        {
            return (childIndex - 1) / 2;
        }

        public T Peek()
        {
            return _array[0].Data;
        }

        private void IncreaseSize()
        {
            _count++;

            if (_array.Length >= _count)
            {
                Allocate();
            }
        }

        private void Allocate()
        {
            int newCapacity = (int) (_array.Length * _increaseSizeFactor);

            MinHeapNode<T>[] newArray = new MinHeapNode<T>[newCapacity];

            Array.Copy(_array, newArray, _array.Length);

            int lastCapacity = _array.Length;

            _array = newArray;

            InitRange(lastCapacity, _array.Length);
        }

        private void RemoveLast()
        {
            _count--;
        }

        private void MinHeapify(int i)
        {
            int smallest;
            int l = 2 * (i + 1) - 1;
            int r = 2 * (i + 1); // - 1 + 1

            if (l < _count && _array[l].Priority < _array[i].Priority)
            {
                smallest = l;
            }
            else
            {
                smallest = i;
            }

            if (r < _count && _array[r].Priority < _array[smallest].Priority)
            {
                smallest = r;
            }

            if (smallest != i)
            {
                MinHeapNode<T> tmp = _array[i];
                _array[i] = _array[smallest];
                _array[smallest] = tmp;

                MinHeapify(smallest);
            }
        }
    }
}