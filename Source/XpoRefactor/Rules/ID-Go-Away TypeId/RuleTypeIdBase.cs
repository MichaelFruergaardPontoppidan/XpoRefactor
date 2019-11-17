using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XpoRefactor
{
    public abstract class RuleTypeIdBase : Rule
    {
        override public string Grouping()
        {
            return "ID-Go-Away: TypeId";
        }
        override public bool Enabled()
        {
            return false;
        }
    }
}
