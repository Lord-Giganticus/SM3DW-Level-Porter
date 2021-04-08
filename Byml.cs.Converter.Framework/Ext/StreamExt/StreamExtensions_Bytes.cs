using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- Read ----

        /// <summary>
        /// Reads <see cref="Byte"/> values into <paramref name="result"/> in one call.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="result">The read values.</param>
        /// <returns>The current stream instance.</returns>
        public static T ReadBytes<T>(this T stream, byte[] result) where T : Stream
        {
            if (stream.Read(result, 0, result.Length) < result.Length)
                throw new EndOfStreamException($"Could not read {result.Length} bytes.");
            return stream;
        }
        /// <summary>
        /// Returns <see cref="Byte"/> values read from the stream in one call.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The read values.</returns>
        public static byte[] ReadBytes<T>(this T stream, int count) where T : Stream
        {
            byte[] result = new byte[count];
            stream.ReadBytes(result);
            return result;
        }

        // ---- Read Async ----

        /// <summary>
        /// Returns <see cref="Byte"/> values read asynchronously from the stream in one call.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        /// <returns>The read values.</returns>
        public static async Task<byte[]> ReadBytesAsync<T>(this T stream, int count, CancellationToken ct = default)
            where T : Stream
        {
            byte[] result = new byte[count];
            await stream.ReadAsync(result, 0, result.Length, ct);
            return result;
        }

        // ---- Write ----

        /// <summary>
        /// Writes <see cref="Byte"/> values to the stream in one call.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <returns>The current stream instance.</returns>
        public static T WriteBytes<T>(this T stream, byte[] values) where T : Stream
        {
            stream.Write(values, 0, values.Length);
            return stream;
        }
        // ---- Write Async ----

        /// <summary>
        /// Writes <see cref="Byte"/> values asynchronously to the stream in one call.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        /// <returns>The current stream instance.</returns>
        public static async Task<T> WriteBytesAsync<T>(this T stream, byte[] values, CancellationToken ct = default)
            where T : Stream
        {
            await stream.WriteAsync(values, 0, values.Length, ct);
            return stream;
        }

        /// <summary>
        /// Writes <see cref="Byte"/> values asynchronously to the stream in one call.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        /// <returns>The current stream instance.</returns>
        public static async Task<T> WriteBytesAsync<T>(this T stream, ArraySegment<byte> values, CancellationToken ct = default)
            where T : Stream
        {
            await stream.WriteAsync(values.Array, 0, values.Count, ct);
            return stream;
        }
    }
}
