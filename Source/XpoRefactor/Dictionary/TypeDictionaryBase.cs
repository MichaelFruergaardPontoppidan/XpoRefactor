using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Text;

namespace XpoRefactor
{
    class TypeDictionaryBase : DictionaryBase
    {
        public string Name;
        public int ArraySize;
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
                        TypeDictionaryBase d = new TypeDictionaryBase();
                        string[] values = line.Split(';');
                        d.Name = values[0];                        
                        d.ArraySize = Int32.Parse(values[1]);
                        if (!collection.ContainsKey(d.Key))
                            collection.Add(d.Name.ToLower(), d);
                        line = reader.ReadLine();
                    }
                }
            }
        }
        static private int readArrayLength(string FileName)
        {
            XpoReader SourceFile = new XpoReader(FileName);
            string ArraySizeProperty = SourceFile.GetPropertyValue("ArraySize");
            if (ArraySizeProperty != String.Empty)
                return Int32.Parse(ArraySizeProperty);
            return 1;
        }

        public static void readFolder(string path, Hashtable collection)
        {
            if (Directory.Exists(path))
            {
                string[] files = System.IO.Directory.GetFiles(path, "*.xpo");
                foreach (string file in files)
                {
                    string name = file.Substring(path.Length + 1, file.Length - path.Length - 5);
                    TypeDictionaryBase d = new TypeDictionaryBase();
                    d.Name = name;
                    if (!collection.ContainsKey(d.Key))
                    {
                        d.ArraySize = TypeDictionaryBase.readArrayLength(file);
                        collection.Add(name.ToLower(), d);
                    }
                }
            }
        }

    }
}
/*
static void CreateTypeTxt(Args _args)
{
    SetEnumerator enum = SysDictionary::construct().types().getEnumerator();
    SysDictType dt;
    textbuffer tb = new textbuffer();
    
    while (enum.moveNext())
    {
        dt = enum.current();
        tb.appendText(strfmt("%1;%2\n", dt.name(), dt.arraySize()));
    }
    tb.toClipboard();
}
 
*/