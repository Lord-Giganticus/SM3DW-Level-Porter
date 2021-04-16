using System.Threading.Tasks;
using System.IO;

namespace SM3DW_Level_Porter.Ext
{
    public static class StreamExt
    {
        public static byte[] GetBytes<T>(this T stream) where T : Stream
        {
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Dispose();
            return ms.ToArray();
        }

        public async static Task<byte[]> GetBytesAsync<T>(this T stream) where T : Stream
        {
            var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            await ms.DisposeAsync();
            return ms.ToArray();
        }
    }
}
