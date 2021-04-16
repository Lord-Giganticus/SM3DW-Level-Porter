using System;
using System.IO;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace SM3DW_Level_Porter.Ext
{
    public static class BinaryDataExt
    {
        public static ByteOrder FlipByteOrder(this ByteOrder order)
        {
            if (order == ByteOrder.BigEndian)
            {
                return ByteOrder.LittleEndian;
            } else
            {
                return ByteOrder.BigEndian;
            }
        }

        public static BinaryDataReader ReadFile(this FileInfo file)
        {
            return new BinaryDataReader(file.ReadFileAsStream(),false);
        }

        public static FileStream ReadFileAsStream(this FileInfo file)
        {
            return File.OpenRead(file.FullName);
        }

        public static BinaryDataReader ReadStream<T>(this T stream) where T : Stream
        {
            return new BinaryDataReader(stream);
        }

        public static BinaryDataReader FlipByteOrder(this BinaryDataReader reader)
        {
            return new BinaryDataReader(reader.BaseStream)
            {
                ByteOrder = reader.ByteOrder.FlipByteOrder()
            };
        }
        [Obsolete("This WILL cause a error!")]
        public static BinaryDataWriter ToWriter(this BinaryDataReader reader)
        {
            return new BinaryDataWriter(reader.BaseStream);
        }

        public static BinaryDataWriter FlipByteOrder(this BinaryDataWriter writer)
        {
            return new BinaryDataWriter(writer.BaseStream)
            {
                ByteOrder = writer.ByteOrder.FlipByteOrder()
            };
        }
        [Obsolete("This WILL cause a error!")]
        public static BinaryDataReader ToReader(this BinaryDataWriter writer)
        {
            return new BinaryDataReader(writer.BaseStream);
        }

        public static bool IsByteOrder(this ByteOrder order, ByteOrder byteOrder)
        {
            if (order == byteOrder)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public static ByteOrder ToByteOrder(this FileInfo info)
        {
            return info.ReadFileAsStream().ReadStream().ByteOrder;
        }

        public static ByteOrder ToByteOrder<T>(this T stream) where T : Stream
        {
            return stream.ReadStream().ByteOrder;
        }

        public static byte[] GetBytes<T>(this T data) where T : BinaryReader
        {
            return data.BaseStream.GetBytes();
        }

        public async static Task<byte[]> GetBytesAsync<T>(this T data) where T : BinaryReader
        {
            return await data.BaseStream.GetBytesAsync();
        }
    }
}
