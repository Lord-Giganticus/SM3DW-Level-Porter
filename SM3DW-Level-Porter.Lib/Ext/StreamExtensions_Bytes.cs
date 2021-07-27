using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SM3DW_Level_Porter.Ext;

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
        /// Reads <see cref="Byte"/> values into <paramref name="result"/> in one call.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="result">The read values.</param>
        /// <returns>The current stream instance.</returns>
        public static T ReadBytes<T>(this T stream, ArraySegment<byte> result) where T : Stream
        {
#if NETSTANDARD2_0
            if (stream.Read(result.Array, 0, result.Count) < result.Count)
                throw new EndOfStreamException($"Could not read {result.Count} bytes.");
#else
            if (stream.Read(result) < result.Count)
                throw new EndOfStreamException($"Could not read {result.Count} bytes.");
#endif
            return stream;
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Reads <see cref="Byte"/> values into <paramref name="result"/> in one call.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="result">The read values.</param>
        /// <returns>The current stream instance.</returns>
        public static T ReadBytes<T>(this T stream, Span<byte> result) where T : Stream
        {
            if (stream.Read(result) < result.Length)
                throw new EndOfStreamException($"Could not read {result.Length} bytes.");
            return stream;
        }
#endif
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

        /// <summary>
        /// Returns all <see cref="Byte"/> values read from the stream in one call.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <returns>The read values.</returns>
        public static byte[] ReadAllBytes<T>(this T stream) where T : Stream
        {
            return stream.ReadBytes(stream.Length.ToInt());
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

        /// <summary>
        /// Returns <see cref="byte"/> values read asynchronously from the stream in one call.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <returns>The read values.</returns>
        public static async Task<byte[]> ReadAllBytesAsync<T>(this T stream) where T : Stream
        { 
            var result = await stream.ReadBytesAsync(int.Parse(stream.Length.ToString()));
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

        /// <summary>
        /// Writes <see cref="Byte"/> values to the stream in one call.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <returns>The current stream instance.</returns>
        public static T WriteBytes<T>(this T stream, ArraySegment<byte> values) where T : Stream
        {
#if NETSTANDARD2_0
            stream.Write(values.Array, 0, values.Count);
#else
            stream.Write(values);
#endif
            return stream;
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Writes <see cref="Byte"/> values to the stream in one call.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <returns>The current stream instance.</returns>
        public static T WriteBytes<T>(this T stream, ReadOnlySpan<byte> values) where T : Stream
        {
            stream.Write(values);
            return stream;
        }
#endif
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

#if !NETSTANDARD2_0
        /// <summary>
        /// Writes <see cref="Byte"/> values asynchronously to the stream in one call.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        /// <returns>The current stream instance.</returns>
        public static async Task<T> WriteBytesAsync<T>(this T stream, ReadOnlyMemory<byte> values, CancellationToken ct = default)
            where T : Stream
        {
            await stream.WriteAsync(values, ct);
            return stream;
        }
#endif
    }
}
