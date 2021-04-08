using System.IO;
using Sarc.cs.lib.Ext;
using SM3DW_Level_Porter.Ext;
using System.Collections.Generic;
using Syroot.BinaryData;
using SARCExt;

namespace Sarc.cs.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    if (File.Exists(arg))
                    {
                        var f = new FileInfo(arg);
                        var d = f.FullName.ReadBytes().DecompressToSarcData();
                        var b = d.endianness.ToString();
                        var h = d.HashOnly.ToString();
                        Directory.SetCurrentDirectory(f.Directory.FullName);
                        foreach (var i in d.Files)
                        {
                            File.WriteAllBytes(i.Key, i.Value);
                        }
                        using var s = new StreamWriter("endianess.txt");
                        s.Write(b);
                        s.Close();
                        using var s2 = new StreamWriter("hash.txt");
                        s2.Write(h);
                        s2.Close();
                    } if (Directory.Exists(arg))
                    {
                        var Dict = new Dictionary<string, byte[]>();
                        var d = new DirectoryInfo(arg);
                        foreach (var f in d.GetFiles())
                        {
                            Dict.Add(f.Name, f.FullName.ReadBytes());
                        }
                        using var r = new StreamReader("endianess.txt");
                        var l = r.ReadToEnd();
                        r.Close();
                        using var r1 = new StreamReader("hash.txt");
                        var h = r1.ReadToEnd();
                        r1.Close();
                        bool h1;
                        if (h.Contains("True"))
                        {
                            h1 = true;
                        } else
                        {
                            h1 = false;
                        }
                        SarcData SD;
                        if (l.Contains(ByteOrder.BigEndian.ToString()))
                        {
                            SD = new SarcData
                            {
                                endianness = ByteOrder.BigEndian,
                                Files = Dict,
                                HashOnly = h1
                            };
                        } else
                        {
                            SD = new SarcData
                            {
                                endianness = ByteOrder.LittleEndian,
                                Files = Dict,
                                HashOnly = h1
                            };
                        }
                        var src = SD.PackSarc().Compress();
                        Directory.SetCurrentDirectory(Directory.GetParent(d.FullName).FullName);
                        File.WriteAllBytes(d.Name + ".szs", src);
                    }
                }
            }
        }
    }
}
