using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using SM3DW_Level_Porter.Ext;
using Sarc.cs.lib.Classes;
using SZS;
using Sarc.cs.lib.Ext;
using SARCExt;
using Byml.cs.lib.Ext;
using Syroot.BinaryData;
using SM3DW_Level_Porter.Classes;
using BYAML;

namespace SM3DW_Level_Porter
{
    public partial class Main : Form
    {
        public Files files;

        public Dictionary<string, byte[]> WiiU_Files { get; set; }

        public Dictionary<string, byte[]> Switch_Files { get; set; }

        public List<bool> Hashs { get; set; }

        public bool Hash { get; set; }

        public FileInfo Switch_File { get; set; }

        public FileInfo WiiU_File { get; set; }

        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "Wii U State Format (Map1.szs;Design1.szs;Sound1.szs)|*1.szs",
                InitialDirectory = Directory.GetCurrentDirectory(),
                FilterIndex = 1,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = true,
                DefaultExt = ".szs",
                Title = "Select a Wii U Stage."
            };
            if (open.ShowDialog().IsResult(DialogResult.OK))
            {
                var dict = new Dictionary<string, byte[]>();
                var hashes = new List<bool>();
                foreach (var item in open.FileNames)
                {
                    var data = YAZ0.Decompress(item).LoadAsSarcData();
                    foreach (var items in data.GetSarcFiles())
                    {
                        listView1.Items.Add(items.Key);
                        dict.Add(items.Key, items.Value);
                    }
                    hashes.Add(data.HashOnly);
                }
                WiiU_File = new FileInfo(open.FileNames[0]);
                WiiU_Files = dict;
                Hashs = hashes;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "SZS files (*.szs)|*.szs",
                FilterIndex = 1,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                Title = "Search for a Switch Stage.",
                DefaultExt = ".szs",
                InitialDirectory = Directory.GetCurrentDirectory()
            };
            if (open.ShowDialog().IsResult(DialogResult.OK))
            {
                var dict = new Dictionary<string, byte[]>();
                var data = YAZ0.Decompress(open.FileName).LoadAsSarcData();
                foreach (var item in data.GetSarcFiles())
                {
                    dict.Add(item.Key, item.Value);
                    listView2.Items.Add(item.Key);
                }
                Switch_Files = dict;
                Hash = data.HashOnly;
                Switch_File = new FileInfo(open.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var new_dict = new Dictionary<string, byte[]>();
            foreach (var item in WiiU_Files)
            {
                if (!item.Key.EndsWith(".byml"))
                {
                    var m = new MemoryStream();
                    m.Write(item.Value, 0, item.Value.Length);
                    var b = m.ReadStream();
                    b.ByteOrder = ByteOrder.LittleEndian;
                    var data = b.ReadBytes(item.Value.Length);
                    b.Dispose();
                    m.Dispose();
                    new_dict.Add(item.Key, data);
                } else
                {
                    var m = new MemoryStream(item.Value);
                    var d = m.GetByml();
                    d.byteOrder = ByteOrder.LittleEndian;
                    var data = d.GetBytes();
                    m.Dispose();
                    new_dict.Add(item.Key, data);
                }
            }
            var Data = new SarcData
            {
                HashOnly = Hashs[0],
                Files = new_dict,
                endianness = ByteOrder.LittleEndian
            };
            var name = WiiU_File.Name.Substring(0, WiiU_File.Name.Length - 5);
            var src = YAZ0.Compress(Data.PackSarcData().GetBytes());
            SaveFileDialog save = new SaveFileDialog
            {
                Filter = "Szs files (*.szs)|*.szs|All files (*.*)|*.*",
                FilterIndex = 1,
                Title = "Save the new Stage.",
                DefaultExt = ".szs",
                CheckPathExists = true,
                InitialDirectory = Directory.GetCurrentDirectory(),
                OverwritePrompt = true,
                FileName = name
            };
            if (save.ShowDialog().IsResult(DialogResult.OK))
            {
                File.WriteAllBytes(save.FileName, src);
                MessageBox.Show("Complete!");
            }
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (BackColor == SystemColors.Control)
            {
                BackColor = SystemColors.ControlDarkDark;
                toolStrip1.BackColor = BackColor;
                toolStripButton1.Text = "Light Mode";
            } else
            {
                BackColor = SystemColors.Control;
                toolStrip1.BackColor = BackColor;
                toolStripButton1.Text = "Dark Mode";
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Colors colors = new Colors();
            BackColor = colors.GetRandomColor();
            toolStrip1.BackColor = BackColor;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var Map_Data = new Dictionary<string, byte[]>();
            var Sound_Data = new Dictionary<string, byte[]>();
            var new_dict = new Dictionary<string, byte[]>();
            foreach (var item in Switch_Files)
            {
                if (!item.Key.EndsWith(".byml"))
                {
                    var m = new MemoryStream();
                    m.Write(item.Value, 0, item.Value.Length);
                    var b = m.ReadStream();
                    b.ByteOrder = ByteOrder.BigEndian;
                    var data = b.ReadBytes(item.Value.Length);
                    b.Dispose();
                    m.Dispose();
                    new_dict.Add(item.Key, data);
                }
                else
                {
                    var f = new FileInfo(item.Key);
                    var m = new MemoryStream(item.Value);
                    var d = m.GetByml();
                    d.byteOrder = ByteOrder.BigEndian;
                    var data = d.GetBytes();
                    m.Dispose();
                    if (f.Name.Contains(Names.Camera) || f.Name.Contains(Names.Map))
                    {
                        Map_Data.Add(item.Key, data);
                    } else if (f.Name.Contains(Names.Sound))
                    {
                        Sound_Data.Add(item.Key, data);
                    } else
                    {
                        new_dict.Add(item.Key, data);
                    }
                }
            }
            var Design = new SarcData
            {
                HashOnly = Hash,
                Files = new_dict,
                endianness = ByteOrder.BigEndian
            };
            var Sound = new SarcData
            {
                HashOnly = Hash,
                Files = Sound_Data,
                endianness = ByteOrder.BigEndian
            };
            var Map = new SarcData
            {
                HashOnly = Hash,
                Files = Map_Data,
                endianness = ByteOrder.BigEndian
            };
            var Name = Switch_File.Name.Substring(0, Switch_File.Name.Length - 4);
            FolderBrowserDialog folder = new FolderBrowserDialog
            {
                UseDescriptionForTitle = true,
                Description = "Select a folder to save the new Stage to.",
                RootFolder = Environment.SpecialFolder.Recent,
                ShowNewFolderButton = true
            };
            if (folder.ShowDialog().IsResult(DialogResult.OK))
            {
                Directory.SetCurrentDirectory(folder.SelectedPath);
                File.WriteAllBytes(Name + "Design1.szs", YAZ0.Compress(Design.PackSarcData().GetBytes()));
                File.WriteAllBytes(Name + "Sound1.szs", YAZ0.Compress(Sound.PackSarcData().GetBytes()));
                File.WriteAllBytes(Name + "Map1.szs", YAZ0.Compress(Map.PackSarcData().GetBytes()));
                MessageBox.Show("Complete!");
            }
        }
    }
}
