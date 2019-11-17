using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleDialogAddFieldDictField : RuleTypeIdBase
    {
        public override string RuleName()
        {
            return String.Format("dialog.{0}(x.extendedTypeId() -> dialog.{0}(x.typeName()", this.methodName());
        }
        public override bool Enabled()
        {
            return false;
        }
        public override string mustContain()
        {
            return "extendedTypeId";
        }
        protected virtual string methodName()
        {
            return "addField";
        }
        protected override void buildXpoMatch()
        {
            xpoMatch.AddDot();
            xpoMatch.AddLiteral(this.methodName());
            xpoMatch.AddStartParenthesis();
            xpoMatch.AddCapture();
            xpoMatch.AddDot();
            xpoMatch.AddLiteral("extendedTypeId");
            xpoMatch.AddStartParenthesis();
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
                    updatedInput = updatedInput.Insert(match.Index, string.Format(".{0}({1}.name()", this.methodName(), variableName));
                    return this.Run(updatedInput);
                }
                else if (variableName.ToLower().Contains("field"))
                {
                    updatedInput = updatedInput.Insert(match.Index, string.Format(".{0}({1}.typeName()", this.methodName(), variableName));
                    return this.Run(updatedInput);
                }
            }
            return input;
        }
    }
}
