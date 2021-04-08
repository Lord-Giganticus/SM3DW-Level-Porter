using System.IO;
using ByamlExt.Byaml;
using FirstPlugin;

namespace SM3DW_Level_Porter.Ext
{
    public static class BymlExt
    {
        public static ByamlFile ByamlFile;

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
