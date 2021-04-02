using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sarc.cs.GUI.Ext
{
    public static class Result
    {
        public static bool IsResult(this DialogResult dialog, DialogResult result)
        {
            if (dialog == result)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
