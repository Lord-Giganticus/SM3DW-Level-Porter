using System;
using System.IO;
using Byml.cs.lib.Ext;

namespace Byml.cs.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    if (File.Exists(arg))
                    {
                        var Data = new FileInfo(arg);
                        var data = Data.GetBymlFileData();
                        data.byteOrder = data.byteOrder.SwitchByteOrder();
                        data.SaveFile(Data.Name);
                    }
                }
            }
            else
            {
                Console.WriteLine("Enter the path to a byml file here:");
                var path = Console.ReadLine();
                if (File.Exists(path))
                {
                    var Data = new FileInfo(path);
                    var data = Data.GetBymlFileData();
                    data.byteOrder = data.byteOrder.SwitchByteOrder();
                    data.SaveFile(Data.Name);
                }
            }
        }
    }
}
