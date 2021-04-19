using SZS;
using Sarc.cs.lib.Ext;
using SARCExt;
using System.Threading.Tasks;

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
            return Byte.Decompress().LoadAsSarcData();
        }

        public static byte[] Compress(this byte[] Byte)
        {
            return YAZ0.Compress(Byte);
        }

        public static byte[] Compress(this SarcData data)
        {
            return YAZ0.Compress(data.PackSarc());
        }

        public static Task<byte[]> DecompressAsync(this byte[] Byte)
        {
            return Task.Run(() => Byte.Decompress());
        }

        public static Task<SarcData> DecompressToSarcDataAsync(this byte[] Byte)
        {
            return Task.Run(() => Byte.DecompressToSarcData());
        }

        public static Task<byte[]> CompressAsync(this byte[] Byte)
        {
            return Task.Run(() => Byte.Compress());
        }

        public static Task<byte[]> CompressAsync(this SarcData data)
        {
            return Task.Run(() => data.Compress());
        }
    }
}
