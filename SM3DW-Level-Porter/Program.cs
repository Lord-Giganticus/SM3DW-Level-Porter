using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace SM3DW_Level_Porter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SM3DW-Level-Porter";
            List<FileInfo> files = new List<FileInfo>();
            foreach (string item in args)
            {
                if (File.Exists(item)) {
                    if (item.EndsWith(".szs"))
                    {
                        files.Add(new FileInfo(item));
                    }
                }
            }
            Classes.FilesParse parse = new Classes.FilesParse();
            parse.Ext = ".szs";
            parse.Parse(files);
            Thread.Sleep(8000);
        }
    }
}
