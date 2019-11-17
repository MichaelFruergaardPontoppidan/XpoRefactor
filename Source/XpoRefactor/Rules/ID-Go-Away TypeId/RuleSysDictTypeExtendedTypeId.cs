using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleSysDictTypeExtendedTypeId : RuleTypeIdBase
    {
        public override string Run(string input)
        {            
            //SysDictField also has a method name extendedTypeId - replacing this will cause compilation errors.
            if (input.Contains("SysDictField"))
                return input;

            return Regex.Replace(input, ".extendedTypeId[ ]?[(][ ]?[)][ ]?==[ ]?typeid[ ]?[(]", ".id() == extendedTypeNum(", RegexOptions.IgnoreCase);
        }
        public override string RuleName()
        {
            return "SysDictType.extendedTypeId() == typeId( -> SysDictType.id() == extendedTypeNum(";
        }
    }
}