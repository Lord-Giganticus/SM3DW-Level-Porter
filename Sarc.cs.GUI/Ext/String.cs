using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarc.cs.GUI.Ext
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
