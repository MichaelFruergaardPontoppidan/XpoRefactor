using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleTypeIdShiftRight : RuleTypeIdBase
    {
        public override string RuleName()
        {
            return "typeid(?) >> 16 -> enumnum(?)";
        }

        public override string Run(string input)
        {
            Match match = Regex.Match(input, @"typeid[ ]?[(][ ]?([\w]+)[ ]?[)][ ]?[>][>][ ]?[#]?16[ ]?(&[ ]?0xffff)?", RegexOptions.IgnoreCase);

            if (match.Success)
            {
                string typeName = match.Groups[1].Value.Trim();
                
                string updatedInput = input.Remove(match.Index, match.Length);
                bool changed = false;

                if (Dictionary.isEnum(typeName))
                {
                    updatedInput = updatedInput.Insert(match.Index, "enumNum(" + typeName+")");
                    changed = true;
                }
                else if (Dictionary.isType(typeName))
                {
                    updatedInput = updatedInput.Insert(match.Index, "extendedTypeNum(" + typeName + ")");
                    changed = true;
                }
                if (changed)
                {
                    return this.Run(updatedInput);
                }                
            }
            return input;
        }
    }
}
