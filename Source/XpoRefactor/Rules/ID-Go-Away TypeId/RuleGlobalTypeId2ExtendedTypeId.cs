using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleGlobalTypeId2ExtendedTypeId : RuleTypeIdBase
    {
        public override string Run(string input)
        {
            return Regex.Replace(input, @"typeId2ExtendedTypeId[ ]?[(][ ]?typeid[ ]?[(](.*?)[ ]?[)][ ]?[)]", "extendedTypeNum($1)", RegexOptions.IgnoreCase);
        }
        public override string RuleName()
        {
            return "typeId2ExtendedTypeId(typeId( -> extendedTypeNum(";
        }
        override public bool Enabled()
        {
            return false;
        }

    }
}
