using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM3DW_Level_Porter.Ext
{
    public static class LongExt
    {
        public static int ToInt(this long Long)
        {
            return int.Parse(Long.ToString());
        }

        public static Task<int> ToIntAsync(this long Long)
        {
            return Task.Run(() => Long.ToInt());
        }
    }
}
