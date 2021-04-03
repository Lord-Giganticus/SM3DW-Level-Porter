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

        /// <include file='doc_gen.xml' path='doc/_7bitin32/read/*'/>
        public static Int32 Read7BitInt32(this Stream stream)
        {
            Read7BitInt32(stream, out Int32 result);
            return result;
        }


        /// <include file='doc_gen.xml' path='doc/_7bitin32/read/*'/>
        public static T Read7BitInt32<T>(this T stream, out Int32 result) where T : Stream
        {
            // Endianness does not matter, as this value is stored byte by byte.
            // While the highest bit is set, the integer requires another of a maximum of 5 bytes.
            result = 0;
            for (int i = 0; i < sizeof(Int32) + 1; i++)
            {
                byte readByte = stream.Read1Byte();
                result |= (readByte & 0b01111111) << i * 7;
                if ((readByte & 0b10000000) == 0)
                    return stream;
            }
            throw new InvalidDataException("Invalid 7-bit encoded Int32.");
        }


        /// <include file='doc_gen.xml' path='doc/_7bitin32/read/*'/>
        public static async Task<Int32> Read7BitInt32Async(this Stream stream, CancellationToken ct = default)
        {
            // Endianness should not matter, as this value is stored byte by byte.
            // While the highest bit is set, the integer requires another of a maximum of 5 bytes.
            int value = 0;
            for (int i = 0; i < sizeof(Int32) + 1; i++)
            {
                byte readByte = await stream.ReadByteAsync(ct);
                value |= (readByte & 0b01111111) << i * 7;
                if ((readByte & 0b10000000) == 0)
                    return value;
            }
            throw new InvalidDataException("Invalid 7-bit encoded Int32.");
        }

        // ---- Write ----


        /// <include file='doc_gen.xml' path='doc/_7bitin32/write/*'/>
        public static T Write7BitInt32<T>(this T stream, Int32 value) where T : Stream
        {
            byte[] buffer = Buffer;
            int size = Get7BitInt32Bytes(value, buffer);
            stream.Write(buffer, 0, size);
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/_7bitin32/write/*'/>
        public static async Task Write7BitInt32Async(this Stream stream, Int32 value, CancellationToken ct = default)
        {
            byte[] buffer = new byte[sizeof(Int32) + 1];
            int size = Get7BitInt32Bytes(value, buffer);
            await stream.WriteAsync(buffer, 0, size, ct);
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static int Get7BitInt32Bytes(Int32 value, byte[] buffer)
        {
            // The highest bit determines whether to continue writing more bytes to form the Int32 value.
            UInt32 unsigned = (UInt32)value;
            int i = 0;
            while (unsigned >= 0b10000000)
            {
                buffer[i++] = (byte)(unsigned | 0b10000000);
                unsigned >>= 7;
            }
            buffer[i] = (byte)unsigned;
            return i + 1;
        }
    }
}
