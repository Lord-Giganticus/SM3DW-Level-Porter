using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- Read ----

        /// <include file='doc_gen.xml' path='doc/boolean/read/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        public static T Read<T>(this T stream, out Boolean result,
            BooleanCoding coding = BooleanCoding.Byte)
            where T : Stream
        {
            return ReadBoolean(stream, out result, coding);
        }

        /// <include file='doc_gen.xml' path='doc/boolean/read/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        public static Boolean ReadBoolean(this Stream stream,
            BooleanCoding coding = BooleanCoding.Byte)
        {
            ReadBoolean(stream, out Boolean result, coding);
            return result;
        }

        /// <include file='doc_gen.xml' path='doc/boolean/read/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        public static T ReadBoolean<T>(this T stream, out Boolean result,
            BooleanCoding coding = BooleanCoding.Byte)
            where T : Stream
        {
            result = coding switch
            {
                BooleanCoding.Byte => stream.ReadByte() != 0,
                BooleanCoding.Word => ReadInt16(stream) != 0,
                BooleanCoding.Dword => ReadInt32(stream) != 0,
                _ => throw new ArgumentException($"Invalid {nameof(BooleanCoding)}.", nameof(coding)),
            };
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/boolean/read/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task<Boolean> ReadBooleanAsync(this Stream stream,
            BooleanCoding coding = BooleanCoding.Byte,
            CancellationToken ct = default)
        {
            return coding switch
            {
                BooleanCoding.Byte => await ReadByteAsync(stream, ct) != 0,
                BooleanCoding.Word => await ReadInt16Async(stream, ct: ct) != 0,
                BooleanCoding.Dword => await ReadInt32Async(stream, ct: ct) != 0,
                _ => throw new ArgumentException($"Invalid {nameof(BooleanCoding)}.", nameof(coding)),
            };
        }

        // ---- Write ----

        /// <include file='doc_gen.xml' path='doc/boolean/write/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T Write<T>(this T stream, Boolean value,
            BooleanCoding coding = BooleanCoding.Byte, ByteConverter converter = null)
            where T : Stream
        {
            return WriteBoolean(stream, value, coding, converter);
        }

        /// <include file='doc_gen.xml' path='doc/boolean/write/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task WriteAsync(this Stream stream, Boolean value,
            BooleanCoding coding = BooleanCoding.Byte, ByteConverter converter = null,
            CancellationToken ct = default)
        {
            await WriteBooleanAsync(stream, value, coding, converter, ct);
        }

        /// <include file='doc_gen.xml' path='doc/boolean/write/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T WriteBoolean<T>(this T stream, Boolean value,
            BooleanCoding coding = BooleanCoding.Byte, ByteConverter converter = null)
            where T : Stream
        {
            converter ??= ByteConverter.System;
            byte[] buffer;
            switch (coding)
            {
                case BooleanCoding.Byte:
                    stream.WriteByte((Byte)(value ? 1 : 0));
                    break;
                case BooleanCoding.Word:
                    buffer = Buffer;
                    converter.GetBytes((Int16)(value ? 1 : 0), buffer);
                    stream.Write(Buffer, 0, sizeof(Int16));
                    break;
                case BooleanCoding.Dword:
                    buffer = Buffer;
                    converter.GetBytes(value ? 1 : 0, buffer);
                    stream.Write(Buffer, 0, sizeof(Int32));
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(BooleanCoding)}.", nameof(coding));
            }
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/boolean/write/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task WriteBooleanAsync(this Stream stream, Boolean value,
            BooleanCoding coding = BooleanCoding.Byte, ByteConverter converter = null,
            CancellationToken ct = default)
        {
            converter ??= ByteConverter.System;
            byte[] buffer;
            switch (coding)
            {
                case BooleanCoding.Byte:
                    await stream.WriteAsync((Byte)(value ? 1 : 0), ct);
                    break;
                case BooleanCoding.Word:
                    buffer = new byte[sizeof(Int16)];
                    converter.GetBytes((Int16)(value ? 1 : 0), buffer);
                    await stream.WriteAsync(buffer, 0, sizeof(Int16), ct);
                    break;
                case BooleanCoding.Dword:
                    buffer = new byte[sizeof(Int32)];
                    converter.GetBytes(value ? 1 : 0, buffer);
                    await stream.WriteAsync(buffer, 0, sizeof(Int32), ct);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(BooleanCoding)}.", nameof(coding));
            }
        }
    }
}
