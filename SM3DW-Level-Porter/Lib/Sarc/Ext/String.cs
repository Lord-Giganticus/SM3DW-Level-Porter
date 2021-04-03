using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM3DW_Level_Porter.Ext
{
    public static class String
    {
        public static string GetLastChars(this string String, int num)
        {
            string new_string = String.Substring(String.Length - num);
            return new_string;
        }
    }
}
