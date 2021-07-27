using System;
using System.Collections.Generic;

namespace SM3DW_Level_Porter.Ext
{
    public static class String
    {
        public static string GetLastChars(this string String, int num)
        {
            List<string> arr()
            {
                var r = new List<string>();
                for (int i = num; i > String.Length; i++)
                {
                    r.Add(String[i].ToString());
                }
                return r;
            }
            return string.Join("", arr().ToArray());
        }
    }
}
