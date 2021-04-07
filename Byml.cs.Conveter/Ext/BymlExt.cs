using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByamlExt.Byaml;
using Byml.cs.lib.Ext;
using FirstPlugin;

namespace SM3DW_Level_Porter.Ext
{
    public static class BymlExt
    {
        public static ByamlFile ByamlFile;

        public static BymlFileData SwitchEndianness(this BymlFileData data)
        {
            return new BymlFileData
            {
                byteOrder = data.byteOrder.SwitchByteOrder(),
                RootNode = data.RootNode,
                SupportPaths = data.SupportPaths,
                Version = data.Version
            };
        }

        public static BymlFileData GetByml(this Stream stream)
        {
            return ByamlFile.LoadN(stream);
        }

        public static string ToYaml(this BymlFileData data)
        {
            return YamlByamlConverter.ToYaml(data);
        }

        public static BymlFileData FromYaml(this string String)
        {
            return YamlByamlConverter.FromYaml(String);
        }
    }
}
