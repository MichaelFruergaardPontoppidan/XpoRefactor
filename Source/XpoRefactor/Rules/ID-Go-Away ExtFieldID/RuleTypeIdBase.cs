using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XpoRefactor
{
    public abstract class RuleExtFieldIdBase : Rule
    {
        override public string Grouping()
        {
            return "ID-Go-Away: ExtFieldId";
        }
        override public bool Enabled()
        {
            return false;
        }
        public override string mustContain()
        {
            return "fieldid2ext";
        }
    }
}
