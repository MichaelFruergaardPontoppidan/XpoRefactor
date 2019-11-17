using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XpoRefactor
{
    public class Scanner 
    {
        private ResultDelegate resultCallback;
        private ProgressDelegate progressCallback;
        private SignalEndDelegate signalEndCallback;
        private bool commit = false;
        private List<Rule> rules;

        public Scanner()
        {
        }

        void scanFolder(string path)
        {
            string[] files = null;
            
            //Skip doc folders
            if (path.Contains("\\System Documentation\\") ||
                path.Contains("\\Application Documentation\\") ||
                path.Contains("\\Application Developer Documentation\\"))
                return;

            if (Directory.Exists(path))
            {
                files = System.IO.Directory.GetFiles(path, "*.xpo");
                foreach (string file in files)
                {
                    scanFile(file);
                }

                string[] folders = System.IO.Directory.GetDirectories(path);
                foreach (string folder in folders)
                {
                    scanFolder(folder);
                }
            }
        }
        
        void scanFile(string filename)
        {
            progressCallback(filename);
            
            if (File.Exists(filename))
            {
                XpoReader SourceFile = new XpoReader(filename);
                
                string fileText = SourceFile.Text();
                string processedText = fileText;
                string skipText = processedText.ToLower();
                foreach (Rule rule in rules)
                {
                    if (rule.skip(skipText))
                        continue;

                    processedText = rule.Run(processedText);
                }
                
                if (fileText != processedText)
                {
                    ResultItem item = new ResultItem();
                    item.filename = filename;
                    item.before = fileText;
                    item.after = processedText;                         
                    resultCallback(item);

                    if (commit)
                    {
                        System.Text.Encoding outEncoding;
                        outEncoding = SourceFile.fileEncoding;

                        SourceFile = null;
                        File.SetAttributes(filename, FileAttributes.Archive);
                        FileStream destinationStream = new FileStream(filename, FileMode.Create);
                        using (StreamWriter destinationFile = new StreamWriter(destinationStream, outEncoding))
                        {
                            destinationFile.Write(processedText);
                        }
                    }
                }                
            }
        }

        public void Run(
            string path, 
            bool commitValue,
            List<Rule> rulesValue,
            ResultDelegate resultDelegate, 
            ProgressDelegate progressDelegate, 
            SignalEndDelegate signalEndDelegate)
        {
            commit = commitValue;
            rules = rulesValue;
            resultCallback = resultDelegate;
            progressCallback = progressDelegate;
            signalEndCallback = signalEndDelegate;
            this.scanFolder(path);
            signalEndCallback();
        }
    }
}
