using System.IO;
using System.Threading.Tasks;
using ByamlExt.Byaml;
using FirstPlugin;
using Byml.cs.lib.Ext;
using Byml.cs.Converter.Framework.Classes;

namespace SM3DW_Level_Porter.Ext
{
    public static class BymlExt
    {

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

        public static BymlFileData GetByml<T>(this T stream) where T: Stream
        {
            return ByamlFile.LoadN(stream, byteOrder: stream.ToByteOrder());
        }

        public static Task<BymlFileData> SwitchEndiannessAsync(this BymlFileData data)
        {
            return Task.Run(() => new BymlFileData
            {
                byteOrder = data.byteOrder.FlipByteOrder(),
                RootNode = data.RootNode,
                SupportPaths = data.SupportPaths,
                Version = data.Version
            });
        }

        public static Task<BymlFileData> GetBymlAsync<T>(this T stream) where T : Stream
        {
            return Task.Run(() => stream.GetByml());
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
