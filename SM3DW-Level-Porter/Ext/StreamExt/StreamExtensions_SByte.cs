using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Read ----

        /// <include file='doc.xml' path='doc/sbyte/read/*'/>
        public static T Read<T>(this T stream, out SByte result) where T : Stream
            => ReadSByte(stream, out result);

        /// <include file='doc.xml' path='doc/sbyte/read/*'/>
        public static SByte ReadSByte(this Stream stream)
        {
            FillBuffer(stream, sizeof(SByte));
            return (SByte)Buffer[0];
        }

        /// <include file='doc.xml' path='doc/sbyte/read/*'/>
        public static T ReadSByte<T>(this T stream, out SByte result) where T : Stream
        {
            FillBuffer(stream, sizeof(SByte));
            result = (SByte)Buffer[0];
            return stream;
        }

        /// <include file='doc.xml' path='doc/sbyte/read/*'/>
        public static async Task<SByte> ReadSByteAsync(this Stream stream, CancellationToken ct = default)
            => (SByte)await stream.ReadByteAsync(ct);

        // ---- Write ----

        /// <include file='doc.xml' path='doc/sbyte/write/*'/>
        public static T Write<T>(this T stream, SByte value) where T : Stream
            => WriteSByte(stream, value);

        /// <include file='doc.xml' path='doc/sbyte/write/*'/>
        public static async Task WriteAsync(this Stream stream, SByte value, CancellationToken ct = default)
            => await WriteSByteAsync(stream, value, ct);

        /// <include file='doc.xml' path='doc/sbyte/write/*'/>
        public static T WriteSByte<T>(this T stream, SByte value) where T : Stream
        {
            byte[] buffer = Buffer;
            buffer[0] = (byte)value;
            stream.Write(buffer, 0, sizeof(SByte));
            return stream;
        }

        /// <include file='doc.xml' path='doc/sbyte/write/*'/>
        public static async Task WriteSByteAsync(this Stream stream, SByte value, CancellationToken ct = default)
            => await stream.WriteAsync(new[] { (byte)value }, 0, sizeof(SByte), ct);
    }
}
