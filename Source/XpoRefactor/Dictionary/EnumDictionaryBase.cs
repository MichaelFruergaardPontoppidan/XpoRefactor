using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Text;

namespace XpoRefactor
{
    class EnumDictionaryBase : DictionaryBase
    {
        public string Name;
        public string Key
        {
            get { return (this.Name).ToLower(); }
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
                        EnumDictionaryBase d = new EnumDictionaryBase();
                        string[] values = line.Split(';');
                        d.Name = values[0];
                        if (!collection.ContainsKey(d.Key))
                            collection.Add(d.Key, d);
                        line = reader.ReadLine();
                    }
                }
            }
        }
        static public void readFolder(string path, Hashtable collection)
        {
            if (Directory.Exists(path))
            {
                string[] files = System.IO.Directory.GetFiles(path, "*.xpo");
                foreach (string file in files)
                {
                    string name = file.Substring(path.Length + 1, file.Length - path.Length - 5);
                    EnumDictionaryBase d = new EnumDictionaryBase();
                    d.Name = name;
                    if (!collection.ContainsKey(d.Key))
                        collection.Add(d.Key, d);
                }
            }
        }

    }
}
/*
static void CreateTypeTxt(Args _args)
{
    SetEnumerator enum = SysDictionary::construct().enums().getEnumerator();
    SysDictEnum de;
    textbuffer tb = new textbuffer();
    
    while (enum.moveNext())
    {
        de = enum.current();
        tb.appendText(strfmt("%1\n", de.name());
    }
    tb.toClipboard();
}
 
*/