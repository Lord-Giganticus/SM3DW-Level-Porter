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

        public static BinaryDataWriter WriteFile(this FileInfo file)
        {
            return new BinaryDataWriter(file.ReadFileAsStream());
        }

        public static BinaryDataReader FlipByteOrder(this BinaryDataReader reader)
        {
            return new BinaryDataReader(reader.BaseStream)
            {
                ByteOrder = reader.ByteOrder.FlipByteOrder()
            };
        }

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

        public static BinaryDataReader ToReader(this BinaryDataWriter writer)
        {
            return new BinaryDataReader(writer.BaseStream);
        }

        public static void WriteToFile(this Stream stream, string path)
        {
            using var file = File.Create(path);
            stream.CopyTo(file);
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
