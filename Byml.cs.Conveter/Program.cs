using System;
using System.IO;
using ByamlExt.Byaml;
using SM3DW_Level_Porter.Ext;
using FirstPlugin;

namespace Byml.cs.Conveter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    var f = new FileInfo(arg);
                    if (f.Extension == ".byml")
                    {
                        var b = File.ReadAllBytes(f.FullName);
                        var m = new MemoryStream(b);
                        var byml = m.GetByml();
                        var y = YamlByamlConverter.ToYaml(byml);
                        Directory.SetCurrentDirectory(f.DirectoryName);
                        using var s = new StreamWriter(f.Name + ".yml");
                        s.Write(y);
                        s.Close();
                    } else if (f.Extension == ".yml")
                    {
                        var y = YamlByamlConverter.FromYaml(f.FullName);
                        var b = ByamlFile.SaveN(y);
                        Directory.SetCurrentDirectory(f.DirectoryName);
                        File.WriteAllBytes(f.Name + ".byml" , b);
                    }
                }
            } else
            {
                Console.WriteLine("Enter the path to a byml or yml file:");
                var p = Console.ReadLine();
                var f = new FileInfo(p);
                if (f.Exists)
                {
                    if (f.Extension == ".byml")
                    {
                        var b = File.ReadAllBytes(f.FullName);
                        var m = new MemoryStream(b);
                        var byml = m.GetByml();
                        var y = YamlByamlConverter.ToYaml(byml);
                        Directory.SetCurrentDirectory(f.DirectoryName);
                        using var s = new StreamWriter(f.Name + ".yml");
                        s.Write(y);
                        s.Close();
                    }
                    else if (f.Extension == ".yml")
                    {
                        var y = YamlByamlConverter.FromYaml(f.FullName);
                        var b = ByamlFile.SaveN(y);
                        Directory.SetCurrentDirectory(f.DirectoryName);
                        File.WriteAllBytes(f.Name + ".byml", b);
                    }
                } else
                {
                    throw new Exception("File does not exist!");
                }
            }
        }
    }
}
