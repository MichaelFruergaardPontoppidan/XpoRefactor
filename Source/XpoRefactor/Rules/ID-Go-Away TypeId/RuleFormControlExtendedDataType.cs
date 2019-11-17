using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleFormControlExtendedDataType : RuleTypeIdBase
    {
        public override string RuleName()
        {
            return "FormControl.extendedDataType(x.extendedTypeId()) -> FormControl.extendedDataType(x.Id())";
        }
        public override bool Enabled()
        {
            return false;
        }
        public override string mustContain()
        {
            return "extendedDataType";
        }
        protected override void buildXpoMatch()
        {
            xpoMatch.AddDot();
            xpoMatch.AddLiteral("extendedDataType");
            xpoMatch.AddStartParenthesis();
            xpoMatch.AddCapture();
            xpoMatch.AddDot();
            xpoMatch.AddLiteral("extendedTypeId");
            xpoMatch.AddStartParenthesis();
            xpoMatch.AddEndParenthesis();
            xpoMatch.AddEndParenthesis();
        }

        public override string Run(string input)
        {
            Match match = xpoMatch.Match(input);            

            if (match.Success)
            {
                string variableName = match.Groups[1].Value.Trim();
                
                string updatedInput = input.Remove(match.Index, match.Length);
                if (variableName.ToLower().Contains("type"))
                {
                    updatedInput = updatedInput.Insert(match.Index, string.Format(".extendedDataType({0}.id())", variableName));
                    return this.Run(updatedInput);
                }
                else if (variableName.ToLower().Contains("field"))
                {
                    updatedInput = updatedInput.Insert(match.Index, string.Format(".extendedDataType({0}.typeId())", variableName));
                    return this.Run(updatedInput);
                }
            }
            return input;
        }
    }
}
