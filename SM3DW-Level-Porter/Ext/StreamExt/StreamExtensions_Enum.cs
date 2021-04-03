using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Syroot.BinaryData.Core;

namespace Syroot.BinaryData
{
    public static partial class StreamExtensions
    {
        // ---- Read Type ----

        /// <include file='doc_gen.xml' path='doc/enum/read/*'/>
        /// <include file='doc.xml' path='doc/enum/type/*'/>
        /// <include file='doc.xml' path='doc/enum/strict/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static object ReadEnum(this Stream stream, Type type,
            bool strict = false, ByteConverter converter = null)
        {
            ReadEnum(stream, out object result, type, strict, converter);
            return result;
        }

        /// <include file='doc_gen.xml' path='doc/enum/read/*'/>
        /// <include file='doc.xml' path='doc/enum/type/*'/>
        /// <include file='doc.xml' path='doc/enum/strict/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static void ReadEnum(this Stream stream, out object result, Type type,
            bool strict = false, ByteConverter converter = null)
        {
            converter ??= ByteConverter.System;

            // Read enough bytes to form an enum value.
            Type valueType = Enum.GetUnderlyingType(type);
            if (valueType == typeof(Byte))
            {
                FillBuffer(stream, sizeof(Byte));
                result = Buffer[0];
            }
            else if (valueType == typeof(SByte))
            {
                FillBuffer(stream, sizeof(SByte));
                result = (SByte)Buffer[0];
            }
            else if (valueType == typeof(Int16))
            {
                FillBuffer(stream, sizeof(Int16));
                result = converter.ToInt16(Buffer);
            }
            else if (valueType == typeof(Int32))
            {
                FillBuffer(stream, sizeof(Int32));
                result = converter.ToInt32(Buffer);
            }
            else if (valueType == typeof(Int64))
            {
                FillBuffer(stream, sizeof(Int64));
                result = converter.ToInt64(Buffer);
            }
            else if (valueType == typeof(UInt16))
            {
                FillBuffer(stream, sizeof(UInt16));
                result = converter.ToUInt16(Buffer);
            }
            else if (valueType == typeof(UInt32))
            {
                FillBuffer(stream, sizeof(UInt32));
                result = converter.ToUInt32(Buffer);
            }
            else if (valueType == typeof(UInt64))
            {
                FillBuffer(stream, sizeof(UInt64));
                result = converter.ToUInt64(Buffer);
            }
            else
            {
                throw new NotImplementedException($"Unsupported enum type {valueType}.");
            }

            // Check if the value is defined in the enumeration, if requested.
            if (strict)
                ValidateEnumValue(type, result);
        }

        /// <include file='doc_gen.xml' path='doc/enum/read/*'/>
        /// <include file='doc.xml' path='doc/enum/type/*'/>
        /// <include file='doc.xml' path='doc/enum/strict/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task<object> ReadEnumAsync(this Stream stream, Type type,
            bool strict = false, ByteConverter converter = null,
            CancellationToken ct = default)
        {
            converter ??= ByteConverter.System;

            // Read enough bytes to form an enum value.
            Type valueType = Enum.GetUnderlyingType(type);
            object value;
            if (valueType == typeof(Byte))
                value = (await FillBufferAsync(stream, sizeof(Byte), ct))[0];
            else if (valueType == typeof(SByte))
                value = (SByte)(await FillBufferAsync(stream, sizeof(SByte), ct))[0];
            else if (valueType == typeof(Int16))
                value = converter.ToInt16(await FillBufferAsync(stream, sizeof(Int16), ct));
            else if (valueType == typeof(Int32))
                value = converter.ToInt32(await FillBufferAsync(stream, sizeof(Int32), ct));
            else if (valueType == typeof(Int64))
                value = converter.ToInt64(await FillBufferAsync(stream, sizeof(Int64), ct));
            else if (valueType == typeof(UInt16))
                value = converter.ToUInt16(await FillBufferAsync(stream, sizeof(UInt16), ct));
            else if (valueType == typeof(UInt32))
                value = converter.ToUInt32(await FillBufferAsync(stream, sizeof(UInt32), ct));
            else if (valueType == typeof(UInt64))
                value = converter.ToUInt64(await FillBufferAsync(stream, sizeof(UInt64), ct));
            else
                throw new NotImplementedException($"Unsupported enum type {valueType}.");

            // Check if the value is defined in the enumeration, if requested.
            if (strict)
                ValidateEnumValue(type, value);
            return value;
        }

        // ---- Read Generic ----

        /// <include file='doc_gen.xml' path='doc/enum/read/*'/>
        /// <include file='doc.xml' path='doc/enum/strict/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T Read<T, TEnum>(this T stream, out TEnum result,
            bool strict = false, ByteConverter converter = null)
            where T : Stream where TEnum : Enum
            => ReadEnum(stream, out result, strict, converter);

        /// <include file='doc_gen.xml' path='doc/enum/read/*'/>
        /// <include file='doc.xml' path='doc/enum/strict/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static TEnum ReadEnum<TEnum>(this Stream stream,
            bool strict = false, ByteConverter converter = null)
            where TEnum : Enum
            => (TEnum)ReadEnum(stream, typeof(TEnum), strict, converter);

        /// <include file='doc_gen.xml' path='doc/enum/read/*'/>
        /// <include file='doc.xml' path='doc/enum/strict/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T ReadEnum<T, TEnum>(this T stream, out TEnum result,
            bool strict = false, ByteConverter converter = null)
            where T : Stream where TEnum : Enum
        {
            ReadEnum(stream, out object resultObject, typeof(TEnum), strict, converter);
            result = (TEnum)resultObject;
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/enum/read/*'/>
        /// <include file='doc.xml' path='doc/enum/strict/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task<TEnum> ReadEnumAsync<TEnum>(this Stream stream,
            bool strict = false, ByteConverter converter = null,
            CancellationToken ct = default)
            where TEnum : Enum
            => (TEnum)await ReadEnumAsync(stream, typeof(TEnum), strict, converter, ct);

        // ---- Write Type ----

        /// <include file='doc_gen.xml' path='doc/enum/write/*'/>
        /// <include file='doc.xml' path='doc/enum/type/*'/>
        /// <include file='doc.xml' path='doc/enum/strict/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T WriteEnum<T>(this T stream, Type type, object value,
            bool strict = false, ByteConverter converter = null)
            where T : Stream
        {
            int size = GetEnumBytes(type, value, strict, converter, Buffer);
            stream.Write(Buffer, 0, size);
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/enum/write/*'/>
        /// <include file='doc.xml' path='doc/enum/type/*'/>
        /// <include file='doc.xml' path='doc/enum/strict/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task WriteEnumAsync(this Stream stream, Type type, object value,
            bool strict = false, ByteConverter converter = null,
            CancellationToken ct = default)
        {
            byte[] buffer = new byte[sizeof(UInt64)];
            int size = GetEnumBytes(type, value, strict, converter ?? ByteConverter.System, buffer);
            await stream.WriteAsync(buffer, 0, size, ct);
        }

        // ---- Write Generic ---

        /// <include file='doc_gen.xml' path='doc/enum/write/*'/>
        /// <include file='doc.xml' path='doc/enum/strict/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T Write<T, TEnum>(this T stream, TEnum value,
            bool strict = false, ByteConverter converter = null)
            where T : Stream where TEnum : Enum
        {
            WriteEnum(stream, typeof(TEnum), value, strict, converter);
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/enum/write/*'/>
        /// <include file='doc.xml' path='doc/enum/strict/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task WriteAsync<T>(this Stream stream, T value,
            bool strict = false, ByteConverter converter = null,
            CancellationToken ct = default) where T : Enum
        {
            await WriteEnumAsync(stream, typeof(T), value, strict, converter, ct);
        }

        /// <include file='doc_gen.xml' path='doc/enum/write/*'/>
        /// <include file='doc.xml' path='doc/enum/strict/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        public static T WriteEnum<T, TEnum>(this T stream, TEnum value,
            bool strict = false, ByteConverter converter = null)
            where T : Stream where TEnum : Enum
        {
            WriteEnum(stream, typeof(TEnum), value, strict, converter);
            return stream;
        }

        /// <include file='doc_gen.xml' path='doc/enum/write/*'/>
        /// <include file='doc.xml' path='doc/enum/strict/*'/>
        /// <include file='doc.xml' path='doc/any/converter/*'/>
        /// <include file='doc.xml' path='doc/any/ct/*'/>
        public static async Task WriteEnumAsync<T>(this Stream stream, T value,
            bool strict = false, ByteConverter converter = null,
            CancellationToken ct = default) where T : Enum
        {
            await WriteEnumAsync(stream, typeof(T), value, strict, converter, ct);
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static int GetEnumBytes(Type type, object value, bool strict, ByteConverter converter, Span<byte> buffer)
        {
            // Check if the value is defined in the enumeration, if requested.
            if (strict)
                ValidateEnumValue(type, value);

            // Get the bytes of the enum value and return the size of it.
            converter ??= ByteConverter.System;
            Type valueType = Enum.GetUnderlyingType(type);
            if (valueType == typeof(Byte))
            {
                buffer[0] = (Byte)value;
                return sizeof(Byte);
            }
            else if (valueType == typeof(SByte))
            {
                buffer[0] = (Byte)(SByte)value;
                return sizeof(SByte);
            }
            else if (valueType == typeof(Int16))
            {
                converter.GetBytes((Int16)value, buffer);
                return sizeof(Int16);
            }
            else if (valueType == typeof(Int32))
            {
                converter.GetBytes((Int32)value, buffer);
                return sizeof(Int32);
            }
            else if (valueType == typeof(Int64))
            {
                converter.GetBytes((Int64)value, buffer);
                return sizeof(Int64);
            }
            else if (valueType == typeof(UInt16))
            {
                converter.GetBytes((UInt16)value, buffer);
                return sizeof(UInt16);
            }
            else if (valueType == typeof(UInt32))
            {
                converter.GetBytes((UInt32)value, buffer);
                return sizeof(UInt32);
            }
            else if (valueType == typeof(UInt64))
            {
                converter.GetBytes((UInt64)value, buffer);
                return sizeof(UInt64);
            }
            else
            {
                throw new NotImplementedException($"Unsupported enum type {valueType}.");
            }
        }

        private static void ValidateEnumValue(Type enumType, object value)
        {
            if (!EnumTools.Validate(enumType, value))
                throw new InvalidDataException($"Read value {value} is not defined in the enum type {enumType}.");
        }
    }
}
