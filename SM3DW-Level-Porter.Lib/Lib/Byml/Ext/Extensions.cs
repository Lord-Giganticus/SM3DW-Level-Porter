using System.IO;
using BYAML;
using Syroot.BinaryData;

namespace Byml.cs.lib.Ext
{
    public static class Extensions
    {
        public static BymlFileData GetBymlFileData(this FileInfo file)
        {
            return ByamlFile.LoadN(file.FullName);
        }

        public static byte[] GetBytes(this BymlFileData data)
        {
            return ByamlFile.SaveN(data);
        }

        public static void SaveFile(this byte[] data, string file)
        {
            File.WriteAllBytes(file, data);
        }

        public static void SaveFile(this BymlFileData data, string file)
        {
            File.WriteAllBytes(file, ByamlFile.SaveN(data));
        }

        public static ByteOrder SwitchByteOrder(this ByteOrder order)
        {
            if (order == ByteOrder.BigEndian)
            {
                return ByteOrder.LittleEndian;
            } else
            {
                return ByteOrder.BigEndian;
            }
        }
    }
}
