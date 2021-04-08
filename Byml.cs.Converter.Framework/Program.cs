using ByamlExt.Byaml;
using SM3DW_Level_Porter.Ext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Byml.cs.Converter.Framework
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
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
                        var y = byml.ToYaml();
                        Directory.SetCurrentDirectory(f.Directory.FullName);
                        var n = Path.GetFileNameWithoutExtension(f.FullName);
                        var s = new StreamWriter(n + ".yml");
                        s.Write(y);
                        s.Close();
                    }
                    else if (f.Extension == ".yml")
                    {
                        var y = f.FullName.FromYaml();
                        var b = ByamlFile.SaveN(y);
                        Directory.SetCurrentDirectory(f.Directory.FullName);
                        var n = Path.GetFileNameWithoutExtension(f.FullName);
                        File.WriteAllBytes(n + ".byml", b);
                    }
                }
            } else
            {
                Console.WriteLine("Enter a file here.");
                var R = Console.ReadLine();
                var f = new FileInfo(R);
                if (f.Exists)
                {
                    if (f.Extension == ".byml")
                    {
                        var b = File.ReadAllBytes(f.FullName);
                        var m = new MemoryStream(b);
                        var byml = m.GetByml();
                        var y = byml.ToYaml();
                        Directory.SetCurrentDirectory(f.Directory.FullName);
                        var n = Path.GetFileNameWithoutExtension(f.FullName);
                        var s = new StreamWriter(n + ".yml");
                        s.Write(y);
                        s.Close();
                    } else if (f.Extension == ".yml")
                    {
                        var y = f.FullName.FromYaml();
                        var b = ByamlFile.SaveN(y);
                        Directory.SetCurrentDirectory(f.Directory.FullName);
                        var n = Path.GetFileNameWithoutExtension(f.FullName);
                        File.WriteAllBytes(n + ".byml", b);
                    }
                }
            }
        }
    }
}
