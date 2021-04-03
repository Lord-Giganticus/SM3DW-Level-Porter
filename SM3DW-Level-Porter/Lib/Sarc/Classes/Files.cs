using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using SARCExt;
using Syroot.BinaryData;
using Sarc.cs.lib.Ext;

namespace Sarc.cs.lib.Classes
{
    public class Files
    {
        /// <summary>
        /// Loads the SarcData from a FileInfo
        /// </summary>
        /// <param name="file">The file to load as SarcData</param>
        /// <returns>A new SarcData.</returns>
        public SarcData GetSarcData(FileInfo file)
        {
            if (file.Extension == ".szs")
            {
                return file.LoadAsSarcData();
            } else
            {
                throw new Exception("File extension is not correct.");
            }
        }
        /// <summary>
        /// Loads the SarcData from a string
        /// </summary>
        /// <param name="file">The file to load as SarcData</param>
        /// <returns>A new SarcData.</returns>
        public SarcData GetSarcData(string file)
        {
            return GetSarcData(new FileInfo(file));
        }
        /// <summary>
        /// Loads the SarcData from a byte array.
        /// </summary>
        /// <param name="src">The byte array to load as SarcData</param>
        /// <returns>A new SarcData.</returns>
        public SarcData GetSarcData(byte[] src)
        {
            return src.LoadAsSarcData();
        }
        /// <summary>
        /// Loads a array of SarcData from a List of FileInfo.
        /// </summary>
        /// <param name="files">The List of FileInfo</param>
        /// <returns>A array of SarcData.</returns>
        public SarcData[] GetSarcDatas(List<FileInfo> files)
        {
            var sarcdata = new List<SarcData>();
            foreach (var item in files)
            {
                if (item.Extension == ".szs")
                {
                    sarcdata.Add(item.LoadAsSarcData());
                }
            }
            return sarcdata.ToArray();
        }
        /// <summary>
        /// Loads a array of SarcData from a DirectoryInfo.
        /// </summary>
        /// <param name="directory">The DirectoryInfo to parse.</param>
        /// <returns>A array of SarcData</returns>
        public SarcData[] GetSarcDatas(DirectoryInfo directory)
        {
            var sarcdata = new List<SarcData>();
            foreach (var item in directory.GetFiles())
            {
                if (item.Extension == ".szs")
                {
                    sarcdata.Add(item.LoadAsSarcData());
                }
            }
            return sarcdata.ToArray();
        }
        /// <summary>
        /// Loads a array of SarcData from a List of byte[]/
        /// </summary>
        /// <param name="srcs">The List of byte[] to parse.</param>
        /// <returns>A array of SarcData.</returns>
        public SarcData[] GetSarcDatas(List<byte[]> srcs)
        {
            var sarcdata = new List<SarcData>();
            foreach (var item in srcs)
            {
                sarcdata.Add(item.LoadAsSarcData());
            }
            return sarcdata.ToArray();
        }
        /// <summary>
        /// Packs the SarcData.
        /// </summary>
        /// <param name="data">The SarcData to pack</param>
        /// <returns>A byte[] array of the packed data.</returns>
        public byte[] PackSarc(SarcData data)
        {
            return data.PackSarcData().GetBytes();
        }
        /// <summary>
        /// Saves the Sarc.
        /// </summary>
        /// <param name="src">The byte array of the packed data.</param>
        /// <param name="file">The file to save to.</param>
        public void SaveSarc(byte[] src, string file)
        {
            src.SaveSarc(file);
        }
        /// <summary>
        /// Packs multiple Sarcs
        /// </summary>
        /// <param name="datas">A SarcData array of SarcData.</param>
        /// <returns>A List of byte[] containing the new code.</returns>
        public List<byte[]> PackSarcs(SarcData[] datas)
        {
            var list = new List<byte[]>();
            foreach (var data in datas)
            {
                list.Add(data.PackSarcData().GetBytes());
            }
            return list;
        }
    }
}
