using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public static class EnumHelper
    {
        public static int CountOf<T>()
        {
            return Enum.GetNames(typeof(T)).Length;
        }
    }
}