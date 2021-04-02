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

namespace SM3DW_Level_Porter
{
    public partial class Main : Form
    {
        public Files files;

        public Dictionary<string, byte[]> WiiU_Byml_Files { get; set; }

        public Dictionary<string, byte[]> Switch_Byml_Files { get; set; }

        public SarcData Switch_Data { get; set; }

        public FileInfo Switch_File { get; set; }

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
                foreach (var item in open.FileNames)
                {
                    var data = YAZ0.Decompress(item).LoadAsSarcData();
                    foreach (var items in data.GetSarcFiles())
                    {
                        if (items.Key.EndsWith(".byml"))
                        {
                            listView1.Items.Add(items.Key);
                            dict.Add(items.Key, items.Value);
                        }
                    }
                }
                WiiU_Byml_Files = dict;
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
                Switch_Byml_Files = dict;
                Switch_Data = data;
                Switch_File = new FileInfo(open.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var WiiU_dict = new Dictionary<string, byte[]>();
            foreach (var item in WiiU_Byml_Files)
            {
                File.WriteAllBytes(item.Key, item.Value);
                var Data = new FileInfo(item.Key);
                var data = Data.GetBymlFileData();
                data.byteOrder = data.byteOrder.SwitchByteOrder();
                WiiU_dict.Add(item.Key, data.GetBytes());
                File.Delete(item.Key);
            }
            var Switch_dict = new Dictionary<string, byte[]>();
            foreach (var item in Switch_Byml_Files)
            {
                foreach (var i in WiiU_dict)
                {
                    if (item.Key == i.Key)
                    {
                        Switch_dict.Add(item.Key, i.Value);
                    }
                }
            }
            Switch_Data.endianness = Syroot.BinaryData.ByteOrder.LittleEndian;
            Switch_Data.Files = Switch_dict;
            var pack_data = Switch_Data.PackSarcData().GetBytes();
            var encoded_data = YAZ0.Compress(pack_data);
            SaveFileDialog save = new SaveFileDialog
            {
                InitialDirectory = Path.GetDirectoryName(Switch_File.FullName),
                Filter = "New Converted Stage (*."+Switch_File.Extension+")|"+Switch_File.Extension,
                FilterIndex = 1,
                CheckPathExists = true
            };
            if (save.ShowDialog().IsResult(DialogResult.OK))
            {
                File.WriteAllBytes(save.FileName, encoded_data);
                MessageBox.Show("Complete!");
            }
        }
    }
}
