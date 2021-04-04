using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public static BinaryDataReader ReadStream(this Stream stream)
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
        [Obsolete("Likely will cause a error.")]
        public static BinaryDataReader ToReader(this BinaryDataWriter writer)
        {
            return new BinaryDataReader(writer.BaseStream);
        }

        public static void WriteToFile(this Stream stream, string path)
        {
            var data = stream.ReadBytes(int.Parse(stream.Length.ToString()));
            stream.Dispose();
            File.WriteAllBytes(path, data);
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
    }
}
