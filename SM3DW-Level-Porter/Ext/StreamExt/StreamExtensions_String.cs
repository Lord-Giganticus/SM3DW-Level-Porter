using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        [ThreadStatic]
        private static char[] _charBuffer;

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        private static char[] CharBuffer
        {
            get
            {
                // Instantiate here as inline initialization only runs for the first thread requiring the buffer.
                // Must not be used in async methods as reused threads may overwrite contents before they were parsed.
                if (_charBuffer == null)
                    _charBuffer = new char[16];
                return _charBuffer;
            }
        }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Read with StringCoding ----

        /// <include file='doc_gen.xml' path='doc/string/read/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/string/encoding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T Read<T>(this T stream, out String result,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null, ByteConverter converter = null)
            where T : Stream
            => ReadString(stream, out result, coding, encoding, converter);

        /// <include file='doc_gen.xml' path='doc/string/read/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/string/encoding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static String ReadString(this Stream stream,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null, ByteConverter converter = null)
        {
            ReadString(stream, out String result, coding, encoding, converter);
            return result;
        }

        /// <include file='doc_gen.xml' path='doc/string/read/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/string/encoding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T ReadString<T>(this T stream, out String result,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null, ByteConverter converter = null)
            where T : Stream
        {
            encoding ??= Encoding.UTF8;
            converter ??= ByteConverter.System;
            result = coding switch
            {
                StringCoding.VariableByteCount => ReadStringWithLength(stream, stream.Read7BitInt32(), false, encoding),
                StringCoding.ByteCharCount => ReadStringWithLength(stream, stream.ReadByte(), true, encoding),
                StringCoding.Int16CharCount => ReadStringWithLength(stream, ReadInt16(stream, converter), true, encoding),
                StringCoding.Int32CharCount => ReadStringWithLength(stream, ReadInt32(stream, converter), true, encoding),
                StringCoding.ZeroTerminated => ReadStringZeroPostfix(stream, encoding),
                _ => throw new ArgumentException($"Invalid {nameof(StringCoding)}.", nameof(coding)),
            };
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/string/read/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/string/encoding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task<String> ReadStringAsync(this Stream stream,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null, ByteConverter converter = null,
            CancellationToken ct = default)
        {
            encoding ??= Encoding.UTF8;
            converter ??= ByteConverter.System;
            return coding switch
            {
                StringCoding.VariableByteCount => await ReadStringWithLengthAsync(stream, stream.Read7BitInt32(), false, encoding, ct),
                StringCoding.ByteCharCount => await ReadStringWithLengthAsync(stream, stream.ReadByte(), true, encoding, ct),
                StringCoding.Int16CharCount => await ReadStringWithLengthAsync(stream, ReadInt16(stream, converter), true, encoding, ct),
                StringCoding.Int32CharCount => await ReadStringWithLengthAsync(stream, ReadInt32(stream, converter), true, encoding, ct),
                StringCoding.ZeroTerminated => await ReadStringZeroPostfixAsync(stream, encoding, ct),
                _ => throw new ArgumentException($"Invalid {nameof(StringCoding)}.", nameof(coding)),
            };
        }

        // ---- Read with length ----

        /// <include file='doc_gen.xml' path='doc/string/read/*'/>
        /// <include file='doc.xml' path='doc/string/length/*'/>
        /// <include file='doc.xml' path='doc/string/encoding/*'/>
        public static T Read<T>(this T stream, out String result,
            int length, Encoding encoding = null)
            where T : Stream
            => ReadString(stream, out result, length, encoding);

        /// <include file='doc_gen.xml' path='doc/string/read/*'/>
        /// <include file='doc.xml' path='doc/string/length/*'/>
        /// <include file='doc.xml' path='doc/string/encoding/*'/>
        public static String ReadString(this Stream stream,
            int length, Encoding encoding = null)
        {
            ReadString(stream, out String result, length, encoding);
            return result;
        }

        /// <include file='doc_gen.xml' path='doc/string/read/*'/>
        /// <include file='doc.xml' path='doc/string/length/*'/>
        /// <include file='doc.xml' path='doc/string/encoding/*'/>
        public static T ReadString<T>(this T stream, out String result,
            int length, Encoding encoding = null)
            where T : Stream
        {
            result = ReadStringWithLength(stream, length, true, encoding ?? Encoding.UTF8);
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/string/read/*'/>
        /// <include file='doc.xml' path='doc/string/length/*'/>
        /// <include file='doc.xml' path='doc/string/encoding/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task<String> ReadStringAsync(this Stream stream,
            int length, Encoding encoding = null,
            CancellationToken ct = default)
        {
            return await ReadStringWithLengthAsync(stream, length, true, encoding ?? Encoding.UTF8, ct);
        }

        // ---- Write ----

        /// <include file='doc_gen.xml' path='doc/string/write/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/string/encoding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static T Write<T>(this T stream, String value,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null, ByteConverter converter = null)
            where T : Stream
            => WriteString(stream, value, coding, encoding, converter);

        /// <include file='doc_gen.xml' path='doc/string/write/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/string/encoding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task WriteAsync(this Stream stream, String value,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null, ByteConverter converter = null,
            CancellationToken ct = default)
            => await WriteStringAsync(stream, value, coding, encoding, converter, ct);

        /// <include file='doc_gen.xml' path='doc/string/write/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/string/encoding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T WriteString<T>(this T stream, String value,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null, ByteConverter converter = null)
            where T : Stream
        {
            encoding ??= Encoding.UTF8;
            converter ??= ByteConverter.System;
            byte[] textBuffer = encoding.GetBytes(value);
            switch (coding)
            {
                case StringCoding.VariableByteCount:
                    Write7BitInt32(stream, textBuffer.Length);
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                case StringCoding.ByteCharCount:
                    stream.WriteByte((byte)value.Length);
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                case StringCoding.Int16CharCount:
                    converter.GetBytes((Int16)value.Length, Buffer);
                    stream.Write(Buffer, 0, sizeof(Int16));
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                case StringCoding.Int32CharCount:
                    converter.GetBytes(value.Length, Buffer);
                    stream.Write(Buffer, 0, sizeof(Int32));
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                case StringCoding.ZeroTerminated:
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    switch (encoding.GetByteCount("A"))
                    {
                        case sizeof(Byte):
                            stream.WriteByte(0);
                            break;
                        case sizeof(Int16):
                            stream.WriteByte(0);
                            stream.WriteByte(0);
                            break;
                    }
                    break;
                case StringCoding.Raw:
                    stream.Write(textBuffer, 0, textBuffer.Length);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(StringCoding)}.", nameof(coding));
            }
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/string/write/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/string/encoding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task WriteStringAsync(this Stream stream, String value,
            StringCoding coding = StringCoding.VariableByteCount, Encoding encoding = null, ByteConverter converter = null,
            CancellationToken ct = default)
        {
            encoding ??= Encoding.UTF8;
            converter ??= ByteConverter.System;
            byte[] lengthBuffer;
            byte[] textBuffer = encoding.GetBytes(value);
            switch (coding)
            {
                case StringCoding.VariableByteCount:
                    await Write7BitInt32Async(stream, textBuffer.Length, ct);
                    await stream.WriteAsync(textBuffer, 0, textBuffer.Length, ct);
                    break;
                case StringCoding.ByteCharCount:
                    await stream.WriteByteAsync((byte)value.Length, ct);
                    await stream.WriteAsync(textBuffer, 0, textBuffer.Length, ct);
                    break;
                case StringCoding.Int16CharCount:
                    lengthBuffer = new byte[sizeof(Int16)];
                    converter.GetBytes((Int16)value.Length, lengthBuffer);
                    await stream.WriteAsync(lengthBuffer, 0, sizeof(Int16), ct);
                    await stream.WriteAsync(textBuffer, 0, textBuffer.Length, ct);
                    break;
                case StringCoding.Int32CharCount:
                    lengthBuffer = new byte[sizeof(Int32)];
                    converter.GetBytes(value.Length, lengthBuffer);
                    await stream.WriteAsync(lengthBuffer, 0, sizeof(Int32), ct);
                    await stream.WriteAsync(textBuffer, 0, textBuffer.Length, ct);
                    break;
                case StringCoding.ZeroTerminated:
                    await stream.WriteAsync(textBuffer, 0, textBuffer.Length, ct);
                    switch (encoding.GetByteCount("A"))
                    {
                        case sizeof(Byte):
                            await stream.WriteByteAsync(0, ct);
                            break;
                        case sizeof(Int16):
                            await stream.WriteByteAsync(0, ct);
                            await stream.WriteByteAsync(0, ct);
                            break;
                    }
                    break;
                case StringCoding.Raw:
                    await stream.WriteAsync(textBuffer, 0, textBuffer.Length, ct);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(StringCoding)}.", nameof(coding));
            }
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static string ReadStringWithLength(Stream stream, int length, bool lengthInChars, Encoding encoding)
        {
            if (length == 0)
                return String.Empty;

            Decoder decoder = encoding.GetDecoder();
            StringBuilder builder = new StringBuilder(length);
            int totalBytesRead = 0;
            lock (stream)
            {
                byte[] buffer = Buffer;
                char[] charBuffer = CharBuffer;
                do
                {
                    int bufferOffset = 0;
                    int charsDecoded = 0;
                    while (charsDecoded == 0)
                    {
                        // Read raw bytes from the stream.
                        int bytesRead = stream.Read(buffer, bufferOffset++, 1);
                        if (bytesRead == 0)
                            throw new EndOfStreamException("Incomplete string data, missing requested length.");
                        totalBytesRead += bytesRead;
                        // Convert the bytes to chars and append them to the string being built.
                        charsDecoded = decoder.GetCharCount(buffer, 0, bufferOffset);
                        if (charsDecoded > 0)
                        {
                            decoder.GetChars(buffer, 0, bufferOffset, charBuffer, 0);
                            builder.Append(charBuffer, 0, charsDecoded);
                        }
                    }
                } while (lengthInChars && builder.Length < length || !lengthInChars && totalBytesRead < length);
            }
            return builder.ToString();
        }

        private static async Task<string> ReadStringWithLengthAsync(Stream stream, int length, bool lengthInChars,
            Encoding encoding, CancellationToken ct)
        {
            if (length == 0)
                return String.Empty;

            Decoder decoder = encoding.GetDecoder();
            StringBuilder builder = new StringBuilder(length);
            int totalBytesRead = 0;
            byte[] buffer = new byte[1];
            char[] charBuffer = new char[16];
            do
            {
                int bufferOffset = 0;
                int charsDecoded = 0;
                while (charsDecoded == 0)
                {
                    // Read raw bytes from the stream.
                    int bytesRead = await stream.ReadAsync(buffer, bufferOffset++, 1, ct);
                    if (bytesRead == 0)
                        throw new EndOfStreamException("Incomplete string data, missing requested length.");
                    totalBytesRead += bytesRead;
                    // Convert the bytes to chars and append them to the string being built.
                    charsDecoded = decoder.GetCharCount(buffer, 0, bufferOffset);
                    if (charsDecoded > 0)
                    {
                        decoder.GetChars(buffer, 0, bufferOffset, charBuffer, 0);
                        builder.Append(charBuffer, 0, charsDecoded);
                    }
                }
            } while (lengthInChars && builder.Length < length || !lengthInChars && totalBytesRead < length);
            return builder.ToString();
        }

        private static string ReadStringZeroPostfix(Stream stream, Encoding encoding)
        {
            // Read byte or word values until a 0 value is found (no encoding's char surrogate should consist of 0).
            // Endianness depends on encoding, not the actual values.
            List<byte> bytes = new List<byte>();
            bool isChar = true;
            byte[] buffer = Buffer;
            lock (stream)
            {
                switch (encoding.GetByteCount("A"))
                {
                    case sizeof(Byte):
                        // Read single bytes.
                        while (isChar)
                        {
                            FillBuffer(stream, sizeof(Byte));
                            if (isChar = buffer[0] != 0)
                                bytes.Add(buffer[0]);
                        }
                        break;
                    case sizeof(Int16):
                        // Read word values of 2 bytes width.
                        while (isChar)
                        {
                            FillBuffer(stream, sizeof(Int16));
                            if (isChar = buffer[0] != 0 || buffer[1] != 0)
                            {
                                bytes.Add(buffer[0]);
                                bytes.Add(buffer[1]);
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException(
                            "Unhandled character byte count. Only 1 or 2 byte terminators are supported at the moment.");
                }
            }
            // Convert to string.
            return encoding.GetString(bytes.ToArray());
        }

        private static async Task<string> ReadStringZeroPostfixAsync(Stream stream, Encoding encoding,
            CancellationToken ct)
        {
            // Read byte or word values until a 0 value is found (no encoding's char surrogate should consist of 0).
            // Endianness depends on encoding, not the actual values.
            List<byte> bytes = new List<byte>();
            bool isChar = true;
            byte[] buffer;
            switch (encoding.GetByteCount("A"))
            {
                case sizeof(Byte):
                    // Read single bytes.
                    buffer = new byte[sizeof(Byte)];
                    while (isChar)
                    {
                        await FillBufferAsync(stream, sizeof(Byte), ct);
                        if (isChar = buffer[0] != 0)
                            bytes.Add(buffer[0]);
                    }
                    break;
                case sizeof(Int16):
                    // Read word values of 2 bytes width.
                    buffer = new byte[sizeof(Int16)];
                    while (isChar)
                    {
                        await FillBufferAsync(stream, sizeof(Int16), ct);
                        if (isChar = buffer[0] != 0 || buffer[1] != 0)
                        {
                            bytes.Add(buffer[0]);
                            bytes.Add(buffer[1]);
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException(
                        "Unhandled character byte count. Only 1- or 2-byte encodings are support at the moment.");
            }
            // Convert to string.
            return encoding.GetString(bytes.ToArray());
        }
    }
}
