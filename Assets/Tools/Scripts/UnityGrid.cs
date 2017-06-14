using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public class UnityGrid<T> : Grid<T>
    {
        [SerializeField] private bool _drawGrid;

        [SerializeField] private Transform _parentTransform;
        [SerializeField] private Vector2 _size;
        [SerializeField] private float _caseSize;

        [SerializeField] private Color _gridColor;
    }
}
