using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace XpoRefactor
{
    class FieldDictionaryBase : DictionaryBase
    {
        public string TableName;
        public string FieldName;
        public string TypeName;
        public int ArraySize;
        //public string Type;
        public string Key
        {
            get { return (this.TableName + "." + this.FieldName).ToLower(); }
        }
        static public void readFile(string filename, Hashtable collection)
        {
            if (File.Exists(filename))
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        FieldDictionaryBase d = new FieldDictionaryBase();
                        string[] values = line.Split(';');
                        d.TableName = values[0];
                        d.FieldName = values[1];
                        d.TypeName = values[2];
                        d.ArraySize = Int32.Parse(values[3]);
          //              d.Type = values[4];
                        if (!collection.ContainsKey(d.Key))
                            collection.Add(d.Key, d);
                        line = reader.ReadLine();
                    }
                }
            }
        }

        public static void readFolder(string path, Hashtable collection)
        {
            if (Directory.Exists(path))
            {
                string[] files = System.IO.Directory.GetFiles(path, "*.xpo");
                foreach (string file in files)
                {
                    string name = file.Substring(path.Length + 1, file.Length - path.Length - 5);
                    XpoReader SourceFile = new XpoReader(file);
                    string xpoText = SourceFile.Text();
                    MatchCollection matches = Regex.Matches(xpoText,
                      @"FIELD[ ][#]([\w]+)[\s\S]*?PROPERTIES([\s\S]*?)ENDPROPERTIES", RegexOptions.Compiled);
                    foreach (Match match in matches)
                    {
                        string fieldName = match.Groups[1].Value.Trim();
                        string properties = match.Groups[2].Value.Trim();
                        string typename = SourceFile.GetPropertyValue("ARRAY \r\n", properties);
                        if (typename == String.Empty)
                            typename = SourceFile.GetPropertyValue("EnumType", properties);

                        FieldDictionaryBase d = new FieldDictionaryBase();
                        d.TableName = name;
                        d.FieldName = fieldName;
                        d.TypeName = typename;
                        if (typename == string.Empty)
                            d.ArraySize = 1;
                        else
                            d.ArraySize = XpoRefactor.Dictionary.typeArraySize(typename);

                        if (!collection.ContainsKey(d.Key))
                            collection.Add(d.Key, d);
                    }
                }
            }
        }
    }
}
