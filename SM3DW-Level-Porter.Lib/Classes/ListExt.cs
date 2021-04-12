using System;
using System.Collections.Generic;

namespace SM3DW_Level_Porter.Classes
{
    public static class ListExt
    {
        public static T RandItem<T>(this List<T> list) where T : notnull
        {
            Random rnd = new Random();
            int r = rnd.Next(list.Count);
            return list[r];
        }
    }
}
