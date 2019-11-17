using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XpoRefactor
{
    public abstract class Rule
    {
        private XpoMatch priv_xpoMatch;
        protected XpoMatch xpoMatch
        {
            get
            {
                if (priv_xpoMatch == null)
                {
                    priv_xpoMatch = new XpoMatch();
                    this.buildXpoMatch();
                }
                return priv_xpoMatch;
            }
        }
        
        protected virtual void buildXpoMatch()
        {
        }
        
        virtual public bool skip(string input)
        {
            string mustContain = this.mustContain().ToLower();
            if (mustContain != String.Empty)
            {
                return !input.ToLower().Contains(mustContain);
            }
            return false;
        }

        virtual public void Init()
        {
        }

        virtual public string mustContain()
        {
            return String.Empty;
        }

        abstract public string Run(string input);
        override sealed public string ToString()
        {
            return this.Grouping() + "." + this.RuleName();
        }
        abstract public string RuleName();
        virtual public string Grouping()
        {
            return "";
        }
        virtual public bool Enabled()
        {
            return false;
        }
    }
}
