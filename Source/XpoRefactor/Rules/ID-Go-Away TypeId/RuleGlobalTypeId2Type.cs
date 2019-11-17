using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleGlobalTypeId2Type : RuleTypeIdBase
    {
        public override string Run(string input)
        {
            return Regex.Replace(input, "typeId2Type[ ]?[(][ ]?typeid[ ]?[(]", "typeName2Type(extendedTypeStr(", RegexOptions.IgnoreCase);
        }
        public override string RuleName()
        {
            return "typeId2Type(typeId( -> typeId2Name(extendedTypeStr(";
        }
        override public bool Enabled()
        {
            return false;            
        }

    }
}
