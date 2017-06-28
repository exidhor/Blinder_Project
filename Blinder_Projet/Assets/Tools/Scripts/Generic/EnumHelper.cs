using System;

namespace Tools
{
    /// <summary>
    /// Gather all the usefull methods around enums
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// A slow method using GetNames(...) which returns the
        /// number of value of a defined type T.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public static int CountOf<T>()
        {
            return Enum.GetNames(typeof(T)).Length;
        }
    }
}