using System.Windows.Forms;

namespace SM3DW_Level_Porter.Ext
{
    public static class Result
    {
        public static bool IsResult(this DialogResult dialog, DialogResult result)
        {
            return dialog == result;
        }
    }
}
