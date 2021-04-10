namespace SM3DW_Level_Porter.Ext
{
    public static class String
    {
        public static string GetLastChars(this string String, int num)
        {
            return String.Substring(String.Length - num);;
        }
    }
}
