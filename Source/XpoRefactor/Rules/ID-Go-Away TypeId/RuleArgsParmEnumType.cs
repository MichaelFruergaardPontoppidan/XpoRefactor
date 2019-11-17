using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleArgsParmEnumType : RuleTypeIdBase
    {
        public override string Run(string input)
        {
            return Regex.Replace(input, ".parmEnumType[ ]?[(][ ]?typeid[ ]?[(]", ".parmEnumType(enumNum(", RegexOptions.IgnoreCase);
        }
        public override string RuleName()
        {
            return "args.parmEnumType(typeId( -> args.parmEnumType(enumNum(";
        }
    }
}