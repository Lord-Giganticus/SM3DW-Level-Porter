using System.IO;
using System.Threading.Tasks;
using BYAML;
using Byml.cs.lib.Ext;

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

        public static BymlFileData GetByml(this Stream stream)
        {
            return ByamlFile.LoadN(stream);
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

        public static Task<BymlFileData> GetBymlAsync(this Stream stream)
        {
            return Task.Run(() => ByamlFile.LoadN(stream));
        }
    }
}
