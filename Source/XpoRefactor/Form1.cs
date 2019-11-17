﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using XpoRefactor.Properties;

namespace XpoRefactor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.populateRules();

            Path.Text = _settings.DirectoryPath;
            ValidatePath();
        }

        private Thread thread;
        private bool commit;
        private bool endSignalReceived = false;
        private List<ResultItem> resultQueue;
        private List<Rule> rules;
        private string currentlyScanning = string.Empty;
        private int scanned;
        private int occurrences;
        private Settings _settings = XpoRefactor.Properties.Settings.Default;

        private bool isTypeRule(Type type)
        {
            if (type.Name == "Rule")
                return true;

            if (type.BaseType != null)
                return isTypeRule(type.BaseType);

            return false;
        }

        private void populateRules()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass &&
                    !type.IsAbstract &&
                    isTypeRule(type))                    
                {
                    Rule rule = (Rule) assembly.CreateInstance(type.FullName);
                    RulesCtrl.Items.Add(rule, rule.Enabled());
                }
            }
        }

        private void Go_Click(object sender, EventArgs e)
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
                thread = null;
                timer1.Stop();
                Go.Text = "Go";
            }
            else
            {
                
                thread = new Thread(threadStart);
                tabControl1.SelectedIndex = 1;
                scanned = 0;
                occurrences = 0;
                commit = CommitCtrl.Text == "Yes";
                endSignalReceived = false;
                Result.Items.Clear();
                rules = new List<Rule>();
                foreach (Rule rule in RulesCtrl.CheckedItems)
                {
                    rule.Init();
                    rules.Add(rule);
                }
                thread.Start();
                timer1.Start();
                Go.Text = "Stop";                
            }
        }

        void threadStart()
        {
            Dictionary.construct(Path.Text);
            Scanner scanner = new Scanner();
            scanner.Run(Path.Text, commit, rules, UpdateResults, UpdateProgress, SignalEnd);
        }

        void UpdateResults(ResultItem item)
        {
            lock (timer1)
            {
                if (resultQueue == null)
                {
                    resultQueue = new List<ResultItem>();
                }
                resultQueue.Add(item);
            }            
        }
        void UpdateProgress(string filename)
        {
            currentlyScanning = filename;
            scanned++;
        }
        void SignalEnd()
        {
            endSignalReceived = true;            
        }

        private int countDeltas(string before, string after)
        {
            DeltaInfo deltaInfo = DeltaInfo.Construct(before, after);
            int deltas = 0;

            while (deltaInfo != null && 
                   deltaInfo.StartPos != deltaInfo.EndPosAfter && 
                   deltaInfo.StartPos != deltaInfo.EndPosBefore)
            {
                deltas++;
                before = before.Remove(deltaInfo.StartPos, deltaInfo.EndPosBefore - deltaInfo.StartPos);
                after = after.Remove(deltaInfo.StartPos, deltaInfo.EndPosAfter - deltaInfo.StartPos);
                deltaInfo = DeltaInfo.Construct(before, after);
            }
            
            return deltas;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (timer1)
            {
                if (resultQueue != null)
                {
                    foreach (ResultItem item in resultQueue)
                    {
                        ListViewItem listItem = new ListViewItem();
                        listItem.Text = item.filename;
                        DeltaInfo deltaInfo = DeltaInfo.Construct(item.before, item.after);
                        if (deltaInfo != null)
                        {
                            if (deltaInfo.EndPosBefore - deltaInfo.StartPos > 0 &&
                                deltaInfo.EndPosAfter - deltaInfo.StartPos > 0)
                            {
                                listItem.SubItems.Add(item.before.Substring(deltaInfo.StartPos, deltaInfo.EndPosBefore - deltaInfo.StartPos).Trim());
                                listItem.SubItems.Add(item.after.Substring(deltaInfo.StartPos, deltaInfo.EndPosAfter - deltaInfo.StartPos).Trim());
                                occurrences += this.countDeltas(item.before, item.after);
                            }
                            else
                            {
                                listItem.Text = "ERROR" + listItem.Text;
                            }
                            Result.Items.Add(listItem);

                        }
                    }
                }
                resultQueue = null;
            }
            if (endSignalReceived)
            {
                StatusText.Text = "Done";
                Go.Text = "Go"; 
                timer1.Stop();
            }
            else
            {
                if (currentlyScanning != "")
                    StatusText.Text = currentlyScanning.Substring(Path.Text.Length);
                else
                    StatusText.Text = "Preparing...";
            }
            ProgressText.Text = String.Format("{0} scanned; {1} found in {2} files. ", scanned, occurrences, Result.Items.Count);
        }

        private void AllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i<RulesCtrl.Items.Count; i++)
            {
                RulesCtrl.SetItemChecked(i, true);
            }
        }

        private void NoneButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < RulesCtrl.Items.Count; i++)
            {
                RulesCtrl.SetItemChecked(i, false);
            }
        }

        private void ValidatePath()
        {
            bool exists = Directory.Exists( Path.Text );
            if( !exists )
            {
                _errorProvider.SetError( Path, string.Format( "Directory: {0} does not exists", Path.Text ) );
            }
            else
            {
                _errorProvider.Clear();
                SaveLastGoodPath();
            }
        }

        private void SaveLastGoodPath()
        {
            _settings.DirectoryPath = Path.Text;
            _settings.Save();
        }

        private void Path_TextChanged( object sender, EventArgs e )
        {
            ValidatePath();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string toClipBoard = "";
            foreach (Rule rule in RulesCtrl.CheckedItems)
            {
                toClipBoard += rule.ToString()+System.Environment.NewLine;
            }

            Clipboard.SetText(toClipBoard);
        }
    }
}
