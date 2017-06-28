using UnityEngine;

namespace Tools
{
    /// <summary>
    /// The editor version of the MonoSingleton. See MonoSingleton
    /// for more Information.
    /// </summary>
    /// <typeparam name="T">The child type</typeparam>
    [ExecuteInEditMode]
    public abstract class EditModeSingleton<T> : MonoSingleton<T>
        where T : MonoBehaviour
    {
        // nothing
    }
}
