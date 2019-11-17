using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleStringEditPerfTypeLookup : RuleTypeIdBase
    {
        public override string Run(string input)
        {
            return Regex.Replace(input, ".performTypeLookup[ ]?[(][ ]?typeid[ ]?[(]", ".performTypeLookup(extendedTypeNum(", RegexOptions.IgnoreCase);
        }
        public override string RuleName()
        {
            return "stringEdit.PerformTypeLookup(typeId( -> stringEdit.PerformTypeLookup(extendedTypeNum(";
        }
    }
}