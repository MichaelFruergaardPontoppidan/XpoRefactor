using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace XpoRefactor
{
    public class Dictionary
    {
        static Dictionary dictionary;
        private Hashtable types;
        private Hashtable fields;
        private Hashtable enums;

        private void readFile(string filename, HashSet<DictionaryBase> collection)
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
                        if (values.Length > 1) 
                            d.ArraySize = Int32.Parse(values[1]);
                        collection.Add(d);
                        line = reader.ReadLine();
                    } 
                }
            }
        }
        private void readFolder(string path, HashSet<DictionaryBase> collection)
        {
            if (Directory.Exists(path))
            {
                string[] files = System.IO.Directory.GetFiles(path, "*.xpo");
                foreach (string file in files)
                {
                    string name = file.Substring(path.Length + 1, file.Length - path.Length - 5);
                    TypeDictionaryBase d = new TypeDictionaryBase();
                    d.Name = name;                 
                    collection.Add(d);
                }
            }
        }
        private void init(string path)
        {
            types  = new Hashtable();
            enums  = new Hashtable();
            fields = new Hashtable();
            TypeDictionaryBase.readFile("..\\..\\Dictionary\\types.txt", types);
            EnumDictionaryBase.readFile("..\\..\\Dictionary\\enums.txt", enums);
            TypeDictionaryBase.readFile("Dictionary\\types.txt", types);
            EnumDictionaryBase.readFile("Dictionary\\enums.txt", enums);
            FieldDictionaryBase.readFile("Dictionary\\fields.txt", fields);

            TypeDictionaryBase.readFolder(path + "\\Data Dictionary\\Extended Data Types", types);
            EnumDictionaryBase.readFolder(path + "\\Data Dictionary\\Base Enums", enums);
            FieldDictionaryBase.readFolder(path + "\\Data Dictionary\\Tables", fields);
        }

        static public bool isType(string name)
        {
            if (dictionary.types.ContainsKey(name.ToLower()))
                return true;
            return false;
        }
        static public bool isEnum(string name)
        {
            if (dictionary.enums.ContainsKey(name.ToLower())) 
                return true;
            return false;
        }
        static public int typeArraySize(string name)
        {
            if (isType(name))
            {
                TypeDictionaryBase tdb = (TypeDictionaryBase)dictionary.types[name.ToLower()];
                return tdb.ArraySize;
            }
            return 1;
        }

        static public void construct(string path)
        {
            if (dictionary == null)
                dictionary = new Dictionary();
            dictionary.init(path);
        }
    }
}
