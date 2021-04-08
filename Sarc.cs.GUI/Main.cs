using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Sarc.cs.lib.Classes;
using Sarc.cs.lib.Ext;
using SZS;
using SARCExt;
using Sarc.cs.GUI.Ext;

namespace Sarc.cs.GUI
{
    public partial class Main : Form
    {

        public SarcData Data { get; set; }

        public Files files = new Files();

        public Main()
        {
            InitializeComponent();
            label2.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "szs file (*.szs)|*.szs|All files (*.*)|*.*",
                FilterIndex = 1,
                Multiselect = false,
                Title = "Search for a szs file",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".szs",
                InitialDirectory = Directory.GetCurrentDirectory()
            };

            if (open.ShowDialog() == DialogResult.OK)
            {
                Data = YAZ0.Decompress(open.FileName.ReadBytes()).LoadAsSarcData();
                listBox1.Items.Clear();
                foreach (var item in Data.Files)
                {
                    listBox1.Items.Add(item.Key);
                }
                label2.Text = Data.endianness.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var save = new SaveFileDialog
            {
                FileName = listBox1.SelectedItem.ToString(),
                Filter = "All files (*.*)|*.*",
                FilterIndex = 1,
                Title = "Save the file.",
                CheckPathExists = true,
                InitialDirectory = Directory.GetCurrentDirectory()
            };
            if (save.ShowDialog().IsResult(DialogResult.OK))
            {
                var item = listBox1.SelectedItem;
                var data = new List<byte>();
                foreach (var file in Data.GetSarcFiles())
                {
                    if (file.IsKey(item.ToString()))
                    {
                        foreach (byte b in file.Value)
                        {
                            data.Add(b);
                        }
                        break;
                    }
                }
                File.WriteAllBytes(save.FileName, data.ToArray());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var item = listBox1.SelectedItem.ToString();
            var data = new List<byte>();
            OpenFileDialog open = new OpenFileDialog
            {
                FileName = item,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = true,
                Filter = "All files (*.*)|*.*",
                FilterIndex = 1,
                InitialDirectory = Directory.GetCurrentDirectory(),
                Title = "Select a file to replace."
            };
            if (open.ShowDialog().IsResult(DialogResult.OK))
            {
                foreach (byte b in File.ReadAllBytes(open.FileName))
                {
                    data.Add(b);
                }
                var return_data = new Dictionary<string, byte[]>();
                foreach (var file in Data.GetSarcFiles())
                {
                    
                    if (file.IsKey(item.ToString()))
                    {
                        return_data.Add(file.Key, data.ToArray());
                    } else
                    {
                        return_data.Add(file.Key, file.Value);
                    }
                }
                Data.Files = return_data;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog
            {
                UseDescriptionForTitle = true,
                Description = "Select a folder to extract all files to.",
                RootFolder = Environment.SpecialFolder.Recent,
                ShowNewFolderButton = true,
                SelectedPath = Directory.GetCurrentDirectory()
            };
            if (folder.ShowDialog().IsResult(DialogResult.OK))
            {
                Directory.SetCurrentDirectory(folder.SelectedPath);
                foreach (var item in Data.Files)
                {
                    File.WriteAllBytes(item.Key, item.Value);
                }
            }
        }
    }
}
