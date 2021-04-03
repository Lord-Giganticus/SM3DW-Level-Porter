using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents extension methods for operations on <see cref="Stream"/> instances.
    /// </summary>
    public static partial class StreamExtensions
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        [ThreadStatic]
        private static byte[] _buffer;

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        private static byte[] Buffer
        {
            get
            {
                // Instantiate here as inline initialization only runs for the first thread requiring the buffer.
                // Must not be used in async methods as reused threads may overwrite contents before they were parsed.
                if (_buffer == null)
                    _buffer = new byte[sizeof(Decimal)];
                return _buffer;
            }
        }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <include file='doc.xml' path='doc/methods/align/*'/>
        public static T Align<T>(this T stream, long alignment, bool grow = false) where T : Stream
        {
            if (alignment == 0)
                throw new ArgumentOutOfRangeException("Alignment must not be 0.");

            long position = stream.Seek((-stream.Position % alignment + alignment) % alignment, SeekOrigin.Current);

            if (grow && position > stream.Length)
                stream.SetLength(position);

            return stream;
        }

        /// <summary>
        /// Gets a value indicating whether the end of the stream has been reached and no more data can be read.
        /// </summary>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <returns>A value indicating whether the end of the stream has been reached.</returns>
        public static bool IsEndOfStream(this Stream stream) => stream.Position >= stream.Length;

        /// <include file='doc.xml' path='doc/methods/move/*'/>
        public static T Move<T>(this T stream, long offset) where T : Stream
        {
            if (offset == 0)
                return stream;

            if (stream.CanSeek)
            {
                // No need to run any simulation.
                stream.Seek(offset);
            }
            else
            {
                // Impossible to simulate seeking backwards in non-seekable stream.
                if (offset < 0)
                    throw new NotSupportedException("Cannot simulate moving backwards in a non-seekable stream.");

                // Simulate move by reading or writing bytes.
                if (stream.CanRead)
                {
                    stream.ReadBytes((int)offset);
                }
                else if (stream.CanWrite)
                {
                    for (int i = 0; i < offset; i++)
                        stream.WriteByte(0);
                }
            }
            return stream;
        }

        /// <include file='doc.xml' path='doc/methods/move/*'/>
        public async static Task MoveAsync(this Stream stream, long offset,
            CancellationToken ct = default)
        {
            if (offset == 0)
                return;

            if (stream.CanSeek)
            {
                // No need to run any simulation.
                stream.Seek(offset);
            }
            else
            {
                // Impossible to simulate seeking backwards in non-seekable stream.
                if (offset < 0)
                    throw new NotSupportedException("Cannot simulate moving backwards in a non-seekable stream.");

                // Simulate move by reading or writing bytes.
                if (stream.CanRead)
                {
                    await stream.ReadBytesAsync((int)offset, ct);
                }
                else if (stream.CanWrite)
                {
                    for (int i = 0; i < offset; i++)
                        await stream.WriteByteAsync(0, ct);
                }
            }
        }

        /// <include file='doc.xml' path='doc/methods/seek/*'/>
        public static long Seek(this Stream stream, long offset) => stream.Seek(offset, SeekOrigin.Current);

        /// <include file='doc.xml' path='doc/methods/temporary_seek/*'/>
        public static Seek TemporarySeek(this Stream stream, long offset = 0, SeekOrigin origin = SeekOrigin.Current)
            => new Seek(stream, offset, origin);

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static void FillBuffer(Stream stream, int length)
        {
            if (stream.Read(Buffer, 0, length) < length)
                throw new EndOfStreamException($"Could not read {length} byte{(length == 1 ? null : "s")}.");
        }

        private static async Task<byte[]> FillBufferAsync(Stream stream, int length, CancellationToken ct)
        {
            byte[] buffer = new byte[length];
            if (await stream.ReadAsync(buffer, 0, length, ct) < length)
                throw new EndOfStreamException($"Could not read {length} bytes.");
            return buffer;
        }
    }
}
