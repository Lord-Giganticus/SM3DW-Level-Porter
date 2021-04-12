using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM3DW_Level_Porter.Ext
{
    public static class CollectionExt
    {
        public static Tuple<T1,T2> ToTuple<T1,T2>(this KeyValuePair<T1,T2> pair)
        {
            return Tuple.Create(pair.Key, pair.Value);
        }

        public static List<Tuple<T1,T2>> ToList<T1,T2>(this Dictionary<T1,T2> dict)
        {
            var l = new List<Tuple<T1, T2>>();
            foreach (var d in dict)
            {
                l.Add(d.ToTuple());
            }
            return l;
        }

        public static List<T> ToList<T>(this T[] array)
        {
            var l = new List<T>();
            foreach (var a in array)
            {
                l.Add(a);
            }
            return l;
        }

        public static bool IsType<T>(this List<T> list, Type type)
        {
            var a = list[0];
            if (a.GetType() == type)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
