using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tools
{
    /*
 * \brief   It is a container to store an object
 *          and it's collision bounds (a rect).
 */
    [Serializable]
    public struct QTObject<T>
    {
        public T obj;
        public Rect rect;

        public QTObject(T obj, Rect rect)
        {
            this.obj = obj;
            this.rect = rect;
        }

        public override string ToString()
        {
            return "QTObject : " + obj.ToString();
        }
    }
}
