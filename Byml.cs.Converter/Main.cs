using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ByamlExt.Byaml;
using SM3DW_Level_Porter.Ext;

namespace Byml.cs.Converter
{
    public partial class Main : Form
    {
        public static string[] Args;

        public Main(string[] args)
        {
            InitializeComponent();
            Args = args;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (Args.Length > 0)
            {
                foreach (var arg in Args)
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
