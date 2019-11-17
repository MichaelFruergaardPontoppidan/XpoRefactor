using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleTypeIsTypeTime : RuleTypeIdBase
    {
        public override string RuleName()
        {
            return "isTypeTime(x.extendedTypeId())-> isTypeTime(x.typeName())";
        }
        public override bool Enabled()
        {
            return false;
        }
        protected override void buildXpoMatch()
        {
            xpoMatch.AddLiteral("isTypeTime");
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
                    updatedInput = updatedInput.Insert(match.Index, string.Format("isTypeTime({0}.name())", variableName));
                    return this.Run(updatedInput);
                }
                else if (variableName.ToLower().Contains("field"))
                {
                    updatedInput = updatedInput.Insert(match.Index, string.Format("isTypeTime({0}.typeName())", variableName));
                    return this.Run(updatedInput);
                }
            }
            return input;
        }
    }
}
