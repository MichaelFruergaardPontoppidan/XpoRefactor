using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleSysDictFieldExtendedTypeId : RuleTypeIdBase
    {
        public override string Run(string input)
        {            
            //SysDictType also has a method name extendedTypeId - replacing this will cause compilation errors.
            if (input.Contains("SysDictType"))
                return input;

            return Regex.Replace(input, ".extendedTypeId[ ]?[(][ ]?[)][ ]?==[ ]?typeid[ ]?[(]", ".typeId() == extendedTypeNum(", RegexOptions.IgnoreCase);
        }
        public override string RuleName()
        {
            return "SysDictField.extendedTypeId() == typeId( -> SysDictField.typeId() == extendedTypeNum(";
        }
    }
}