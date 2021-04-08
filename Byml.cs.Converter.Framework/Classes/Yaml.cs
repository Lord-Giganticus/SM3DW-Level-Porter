using ByamlExt.Byaml;
using SharpYaml.Serialization;
using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Library.Security.Cryptography;

namespace Byml.cs.Converter.Framework.Classes
{
    public class Yaml
    {
        private static Dictionary<dynamic, YamlNode> NodePaths = new Dictionary<dynamic, YamlNode>();

        private static Dictionary<string, dynamic> ReferenceNodes = new Dictionary<string, dynamic>();

        //id to keep track of reference nodes
        static int refNodeId = 0;

        public static BymlFileData FromYaml(string text)
        {
            NodePaths.Clear();
            ReferenceNodes.Clear();

            var data = new BymlFileData();
            var yaml = new YamlStream();
            yaml.Load(File.OpenText(text));
            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

            foreach (var child in mapping.Children)
            {
                var key = ((YamlScalarNode)child.Key).Value;
                var value = child.Value.ToString();

                if (key == "Version")
                    data.Version = ushort.Parse(value);
                if (key == "IsBigEndian")
                    data.byteOrder = bool.Parse(value) ? ByteOrder.BigEndian : ByteOrder.LittleEndian;
                if (key == "SupportPaths")
                    data.SupportPaths = bool.Parse(value);

                if (child.Value is YamlMappingNode)
                    data.RootNode = ParseNode(child.Value);
                if (child.Value is YamlSequenceNode)
                    data.RootNode = ParseNode(child.Value);
            }

            ReferenceNodes.Clear();
            NodePaths.Clear();

            return data;
        }

        static dynamic ParseNode(YamlNode node)
        {
            if (node is YamlMappingNode)
            {
                var values = new Dictionary<string, dynamic>();
                if (IsValidReference(node))
                    ReferenceNodes.Add(node.Tag, values);

                foreach (var child in ((YamlMappingNode)node).Children)
                {
                    var key = ((YamlScalarNode)child.Key).Value;
                    var tag = ((YamlScalarNode)child.Key).Tag;
                    if (tag == "!h")
                        key = Crc32.Compute(key).ToString("x");

                    values.Add(key, ParseNode(child.Value));
                }
                return values;
            }
            else if (node is YamlSequenceNode)
            {
                var values = new List<dynamic>();
                if (IsValidReference(node))
                    ReferenceNodes.Add(node.Tag, values);

                foreach (var child in ((YamlSequenceNode)node).Children)
                    values.Add(ParseNode(child));
                return values;
            } //Reference node
            else if (node is YamlScalarNode && ((YamlScalarNode)node).Value.Contains("!refTag="))
            {
                string tag = ((YamlScalarNode)node).Value.Replace("!refTag=", string.Empty);
                Console.WriteLine($"refNode {tag} {ReferenceNodes.ContainsKey(tag)}");
                if (ReferenceNodes.ContainsKey(tag))
                    return ReferenceNodes[tag];
                else
                {
                    Console.WriteLine("Failed to find reference node! " + tag);
                    return null;
                }
            }
            else
            {
                return ConvertValue(((YamlScalarNode)node).Value, ((YamlScalarNode)node).Tag);
            }
        }

        static bool IsValidReference(YamlNode node)
        {
            return node.Tag != null && node.Tag.Contains("!ref") && !ReferenceNodes.ContainsKey(node.Tag);
        }

        static dynamic ConvertValue(string value, string tag)
        {
            if (tag == null)
                tag = "";

            if (value == "null")
                return null;
            else if (value == "true" || value == "True")
                return true;
            else if (value == "false" || value == "False")
                return false;
            else if (tag == "!u")
                return UInt32.Parse(value, CultureInfo.InvariantCulture);
            else if (tag == "!l")
                return Int32.Parse(value, CultureInfo.InvariantCulture);
            else if (tag == "!d")
                return Double.Parse(value, CultureInfo.InvariantCulture);
            else if (tag == "!ul")
                return UInt64.Parse(value, CultureInfo.InvariantCulture);
            else if (tag == "!ll")
                return Int64.Parse(value, CultureInfo.InvariantCulture);
            else if (tag == "!h")
                return Crc32.Compute(value).ToString("x");
            else if (tag == "!p")
                return new ByamlPathIndex() { Index = Int32.Parse(value, CultureInfo.InvariantCulture) };
            else
            {
                float floatValue = 0;
                bool isFloat = float.TryParse(value, out floatValue);
                if (isFloat)
                    return floatValue;
                return value;
            }
        }
    }
}
