using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clustering.Common.Extensions
{
    public static class StringToDictionary
    {
        public static Dictionary<int, string> TransformToDictionary (this string str)
        {
            var list = str.Split(';');
            return list.ToDictionary(x => Int32.Parse(x.Split(':')[0]), x => x.Split(':')[1]);
        }
    }
}
