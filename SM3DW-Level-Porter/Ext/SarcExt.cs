using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SARCExt;
using Sarc.cs.lib.Ext;
using System.IO;

namespace SM3DW_Level_Porter.Ext
{
    public static class SarcExt
    {
        public static SarcData SwitchEndianness(this SarcData data)
        {
            return new SarcData
            {
                Files = data.Files,
                endianness = data.endianness.FlipByteOrder(),
                HashOnly = data.HashOnly
            };
        }

        public static SarcData SetFiles(this SarcData data, Dictionary<string, byte[]> dict)
        {
            return new SarcData
            {
                Files = dict,
                endianness = data.endianness,
                HashOnly = data.HashOnly
            };
        }

        public static SarcData ReplaceFile(this SarcData data, FileInfo file)
        {
            var dict = new Dictionary<string, byte[]>();
            foreach (var item in data.Files)
            {
                if (item.Key == file.Name)
                {
                    if (file.Exists)
                    {
                        dict.Add(item.Key, file.FullName.ReadBytes());
                    }
                    else
                    {
                        dict.Add(item.Key, item.Value);
                    }
                } else
                {
                    dict.Add(item.Key, item.Value);
                }
            }
            return new SarcData
            {
                endianness = data.endianness,
                Files = dict,
                HashOnly = data.HashOnly
            };
        }

        public static byte[] PackSarc(this SarcData data)
        {
            return data.PackSarcData().GetBytes();
        }
    }
}
