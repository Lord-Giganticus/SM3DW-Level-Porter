using SZS;
using Sarc.cs.lib.Ext;
using SARCExt;


namespace SM3DW_Level_Porter.Ext
{
    public static class Yaz0Ext
    {
        public static byte[] Decompress(this byte[] Byte)
        {
            return YAZ0.Decompress(Byte);
        }

        public static SarcData DecompressToSarcData(this byte[] Byte)
        {
            return YAZ0.Decompress(Byte).LoadAsSarcData();
        }

        public static byte[] Compress(this byte[] Byte)
        {
            return YAZ0.Compress(Byte);
        }

        public static byte[] Compress(this SarcData data)
        {
            return YAZ0.Compress(data.PackSarc());
        }
    }
}
