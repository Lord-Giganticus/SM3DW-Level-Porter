using System.IO;
using BYAML;
using Byml.cs.lib.Ext;

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
    }
}
