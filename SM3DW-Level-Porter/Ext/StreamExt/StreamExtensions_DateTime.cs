using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Syroot.BinaryData.Core;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- Read ----

        /// <include file='doc_gen.xml' path='doc/datetime/read/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T Read<T>(this T stream, out DateTime result,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null)
            where T : Stream
            => ReadDateTime(stream, out result, coding, converter);

        /// <include file='doc_gen.xml' path='doc/datetime/read/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static DateTime ReadDateTime(this Stream stream,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null)
        {
            ReadDateTime(stream, out DateTime result, coding, converter);
            return result;
        }

        /// <include file='doc_gen.xml' path='doc/datetime/read/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T ReadDateTime<T>(this T stream, out DateTime result,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null)
            where T : Stream
        {
            result = coding switch
            {
                DateTimeCoding.NetTicks => new DateTime(ReadInt64(stream, converter)),
                DateTimeCoding.CTime => CTimeTools.GetDateTime(ReadUInt32(stream, converter)),
                DateTimeCoding.CTime64 => CTimeTools.GetDateTime(ReadUInt64(stream, converter)),
                _ => throw new ArgumentException($"Invalid {nameof(DateTimeCoding)}.", nameof(coding)),
            };
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/datetime/read/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task<DateTime> ReadDateTimeAsync(this Stream stream,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null,
            CancellationToken ct = default)
        {
            return coding switch
            {
                DateTimeCoding.NetTicks => new DateTime(await ReadInt64Async(stream, converter, ct)),
                DateTimeCoding.CTime => CTimeTools.GetDateTime(await ReadUInt32Async(stream, converter, ct)),
                DateTimeCoding.CTime64 => CTimeTools.GetDateTime(await ReadUInt64Async(stream, converter, ct)),
                _ => throw new ArgumentException($"Invalid {nameof(DateTimeCoding)}.", nameof(coding)),
            };
        }

        // ---- Write ----

        /// <include file='doc_gen.xml' path='doc/datetime/write/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T Write<T>(this T stream, DateTime value,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null)
            where T : Stream
            => WriteDateTime(stream, value, coding, converter);

        /// <include file='doc_gen.xml' path='doc/datetime/write/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task WriteAsync(this Stream stream, DateTime value,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null,
            CancellationToken ct = default)
            => await WriteAsync(stream, value, coding, converter, ct);

        /// <include file='doc_gen.xml' path='doc/datetime/write/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T WriteDateTime<T>(this T stream, DateTime value,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null)
            where T : Stream
        {
            converter ??= ByteConverter.System;
            switch (coding)
            {
                case DateTimeCoding.NetTicks:
                    Write(stream, value.Ticks, converter);
                    break;
                case DateTimeCoding.CTime:
                    Write(stream, (UInt32)CTimeTools.GetSeconds(value), converter);
                    break;
                case DateTimeCoding.CTime64:
                    Write(stream, (UInt64)CTimeTools.GetSeconds(value), converter);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(DateTimeCoding)}.", nameof(coding));
            }
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/datetime/write/*'/>
        /// <include file='doc.xml' path='doc/any/coding/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task WriteDateTimeAsync(this Stream stream, DateTime value,
            DateTimeCoding coding = DateTimeCoding.NetTicks, ByteConverter converter = null,
            CancellationToken ct = default)
        {
            converter ??= ByteConverter.System;
            switch (coding)
            {
                case DateTimeCoding.NetTicks:
                    await WriteAsync(stream, value.Ticks, converter, ct);
                    break;
                case DateTimeCoding.CTime:
                    await WriteAsync(stream, (UInt32)CTimeTools.GetSeconds(value), converter, ct);
                    break;
                case DateTimeCoding.CTime64:
                    await WriteAsync(stream, (UInt64)CTimeTools.GetSeconds(value), converter, ct);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(DateTimeCoding)}.", nameof(coding));
            }
        }
    }
}
