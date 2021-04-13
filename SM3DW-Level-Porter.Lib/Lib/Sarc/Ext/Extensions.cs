using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SARCExt;
using Syroot.BinaryData;
using SM3DW_Level_Porter.Ext;

namespace Sarc.cs.lib.Ext
{
    public static class Extensions
    {
        public static SarcData LoadAsSarcData(this FileInfo file)
        {
            return SARC.UnpackRamN(File.ReadAllBytes(file.FullName));
        }

        public static SarcData LoadAsSarcData(this string path)
        {
            if (File.Exists(path))
            {
                return SARC.UnpackRamN(path.ReadBytes());
            } else
            {
                throw new FileNotFoundException();
            }
        }

        public static SarcData LoadAsSarcData(this byte[] bytes)
        {
            return SARC.UnpackRamN(bytes);
        }

        public static Dictionary<string, byte[]> GetSarcFiles(this SarcData sarcData)
        {
            return sarcData.Files;
        }

        public static Dictionary<string, byte[]> ReplaceFile(this Dictionary<string, byte[]> dict, FileInfo file)
        {
            var new_dict = new Dictionary<string, byte[]>();
            foreach (var item in dict)
            {
                if (item.Key == file.Name)
                {
                    new_dict.Add(item.Key, file.ReadBytes());
                } else
                {
                    new_dict.Add(item.Key, item.Value);
                }
            }
            return new_dict;
        }

        public static Dictionary<string, byte[]> ReplaceFiles(this Dictionary<string, byte[]> dict, DirectoryInfo dir)
        {
            var new_dict = new Dictionary<string, byte[]>();
            var files = dir.GetFiles();
            foreach (var item in dict)
            {
                foreach (var file in files)
                {
                    if (item.Key == file.Name)
                    {
                        new_dict.Add(item.Key, file.ReadBytes());
                        break;
                    } else
                    {
                        new_dict.Add(item.Key, item.Value);
                        break;
                    }
                }
                continue;
            }
            return new_dict;
        }

        public static byte[] ReadBytes(this FileInfo file)
        {
            return File.ReadAllBytes(file.FullName);
        }

        public static byte[] ReadBytes(this string Path)
        {
            if (File.Exists(Path))
            {
                return File.ReadAllBytes(Path);
            } else
            {
                throw new FileNotFoundException();
            }
        }

        public static SarcData SetSarcFiles(this Dictionary<string,byte[]> dict, SarcData data)
        {
            data.Files = dict;
            return data;
        }

        public static ByteOrder FlipByteOrder(this ByteOrder byteOrder)
        {
            if (byteOrder == ByteOrder.BigEndian)
            {
                byteOrder = ByteOrder.LittleEndian;
                return byteOrder;
            } else
            {
                byteOrder = ByteOrder.BigEndian;
                return byteOrder;
            }
        }

        public static Tuple<int, byte[]> PackSarcData(this SarcData data)
        {
            return SARC.PackN(data);
        }

        public static void SaveSarc(this Tuple<int,byte[]> tuple, string file)
        {
            File.WriteAllBytes(file, tuple.GetBytes());
        }

        public static void SaveSarc(this byte[] bytes, string file)
        {
            File.WriteAllBytes(file, bytes);
        }

        public static byte[] GetBytes<T>(this Tuple<T,byte[]> tuple)
        {
            return tuple.Item2;
        }

        public static List<SarcData> Tolist(this SarcData[] datas)
        {
            return datas.ToList();
        }

        public static void ExtractFiles(this Dictionary<string, byte[]> dict)
        {
            foreach (var item in dict)
            {
                File.WriteAllBytes(item.Key, item.Value);
            }
        }

        public static bool IsKey(this KeyValuePair<string, byte[]> pair, string key)
        {
            if (pair.Key == key)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public static KeyValuePair<T1, T2> GetRandPair<T1, T2>(this Dictionary<T1, T2> dict)
        {
            Random rnd = new Random();
            int r = rnd.Next(dict.Count);
            return dict.ElementAt(r);
        }
    }
}
