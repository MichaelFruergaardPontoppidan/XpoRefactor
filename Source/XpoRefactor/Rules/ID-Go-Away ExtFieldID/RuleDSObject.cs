using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleDSObject : RuleExtFieldIdBase
    {
        protected override void buildXpoMatch()
        {
            xpoMatch.AddLiteral("_ds");
            xpoMatch.AddDot();
            xpoMatch.AddLiteral("object");
            xpoMatch.AddStartParenthesis();
            xpoMatch.AddCaptureOptional("global[:][:]");
            xpoMatch.AddLiteral("fieldid2ext");
            xpoMatch.AddStartParenthesis();
            xpoMatch.AddCapture();
            xpoMatch.AddComma();
            xpoMatch.AddLiteral("1");
            xpoMatch.AddEndParenthesis();
            xpoMatch.AddEndParenthesis();
        }

        public override string RuleName()
        {
            return "datasource_ds.object(fieldid2ext(f, 1)) -> datasource_ds.object(f)";
        }

        public override string Run(string input)
        {
            Match match = xpoMatch.Match(input);

            if (match.Success)
            {
                string globalUsage = match.Groups[1].Value.Trim();
                string fieldId = match.Groups[2].Value.Trim();                                
                    
                string updatedInput = input.Remove(match.Index, match.Length);
                updatedInput = updatedInput.Insert(match.Index,
                    string.Format("_ds.object({0})", fieldId));

                return this.Run(updatedInput);
            }

            return input;
        }      
    }
}
