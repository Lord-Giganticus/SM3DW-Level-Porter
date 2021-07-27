using System.IO;
using ByamlExt.Byaml;
using FirstPlugin;
using Byml.cs.Converter.Framework.Classes;

namespace SM3DW_Level_Porter.Ext
{
    public static class BymlExt
    {

        public static BymlFileData GetByml(this Stream stream)
        {
            return ByamlFile.LoadN(stream, byteOrder: stream.GetByteOrder());
        }

        public static string ToYaml(this BymlFileData data)
        {
            return YamlByamlConverter.ToYaml(data);
        }

        public static BymlFileData FromYaml(this string String)
        {
            return Yaml.FromYaml(String);
        }
    }
}
