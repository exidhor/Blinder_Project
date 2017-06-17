using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tools
{
    [ExecuteInEditMode]
    public abstract class EditModeSingleton<T> : MonoSingleton<T>
        where T : MonoBehaviour
    {
        // nothing
    }
}
