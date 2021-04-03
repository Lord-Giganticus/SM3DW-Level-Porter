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

namespace SM3DW_Level_Porter
{
    public partial class Main : Form
    {
        public Files files;

        public Dictionary<string, byte[]> WiiU_Files { get; set; }

        public Dictionary<string, byte[]> Switch_Files { get; set; }

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
                        listView1.Items.Add(items.Key);
                        dict.Add(items.Key, items.Value);
                    }
                }
                WiiU_Files = dict;
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
                Switch_Data = data;
                Switch_File = new FileInfo(open.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("In Development! ;)");
            return;
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
            MessageBox.Show("In Development! ;)");
            return;
        }
    }
}
