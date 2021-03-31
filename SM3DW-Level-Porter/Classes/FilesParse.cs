using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM3DW_Level_Porter.Classes
{
    class FilesParse
    {
        public void Parse(List<FileInfo> files, string Ext)
        {
            if (Ext == ".szs")
            {
                foreach (var item in files)
                {
                    if (item.Extension == Ext)
                    {
                        if (item.Name.Contains("Map1") || item.Name.Contains("Design1") || item.Name.Contains("Sound1"))
                        {
                            Console.Title = Console.Title + " : WII U MODE";
                        }
                        else
                        {
                            Console.Title = Console.Title + " : SWITCH MODE";
                        }
                        
                    }
                }
            } else if (Ext == ".byml")
            {
                List<FileInfo> byml_files = new List<FileInfo>();
                foreach (var item in files)
                {
                    if (item.Extension == Ext)
                    {
                        byml_files.Add(item);
                    }
                }
            }
        }
    }
}
