using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- Read ----

        /// <include file='doc_gen.xml' path='doc/byte/read/*'/>
        public static T Read<T>(this T stream, out Byte result) where T : Stream
            => ReadByte(stream, out result);

        /// <include file='doc_gen.xml' path='doc/byte/read/*'/>
        /// <remarks>Special method name as it otherwise conflicts with <see cref="Stream.ReadByte"/>.</remarks>
        public static Byte Read1Byte(this Stream stream)
        {
            ReadByte(stream, out Byte result);
            return result;
        }

        /// <include file='doc_gen.xml' path='doc/byte/read/*'/>
        public static T ReadByte<T>(this T stream, out Byte result) where T : Stream
        {
            FillBuffer(stream, sizeof(Byte));
            result = Buffer[0];
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/byte/read/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task<Byte> ReadByteAsync(this Stream stream, CancellationToken ct = default)
            => (await FillBufferAsync(stream, sizeof(Byte), ct))[0];

        // ---- Write ----

        /// <include file='doc_gen.xml' path='doc/byte/write/*'/>
        public static T Write<T>(this T stream, Byte value) where T : Stream
        {
            stream.WriteByte(value); // use System.IO.Stream implementation directly.
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/byte/write/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task WriteAsync(this Stream stream, Byte value, CancellationToken ct = default)
            => await WriteByteAsync(stream, value, ct);

        /// <include file='doc_gen.xml' path='doc/byte/write/*'/>
        /// <remarks>Special method name as it otherwise conflicts with <see cref="Stream.WriteByte(Byte)"/>.</remarks>
        public static T Write1Byte<T>(this T stream, Byte value) where T : Stream
        {
            stream.WriteByte(value); // use System.IO.Stream implementation directly.
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/byte/write/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task WriteByteAsync(this Stream stream, Byte value, CancellationToken ct = default)
            => await stream.WriteAsync(new[] { value }, 0, sizeof(Byte), ct);
    }
}
