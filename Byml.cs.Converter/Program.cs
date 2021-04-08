using ByamlExt.Byaml;
using SM3DW_Level_Porter.Ext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Byml.cs.Converter
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
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
                        Directory.SetCurrentDirectory(f.DirectoryName);
                        using var s = new StreamWriter(f.Name + ".yml");
                        s.Write(y);
                        s.Close();
                    }
                    else if (f.Extension == ".yml")
                    {
                        var y = f.FullName.FromYaml();
                        var b = ByamlFile.SaveN(y);
                        Directory.SetCurrentDirectory(f.DirectoryName);
                        File.WriteAllBytes(f.Name + ".byml", b);
                    }
                }
            }
        }
    }
}
