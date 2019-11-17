using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleReportSectionAddControl : RuleExtFieldIdBase
    {
        protected override void buildXpoMatch()
        {
            xpoMatch.AddDot();
            xpoMatch.AddLiteral("addrealcontrol");
            xpoMatch.AddStartParenthesis();
            xpoMatch.AddCapture();
            xpoMatch.AddComma();
            xpoMatch.AddCaptureOptional("global[:][:]");
            xpoMatch.AddLiteral("fieldid2ext");
            xpoMatch.AddStartParenthesis();
            xpoMatch.AddCapture();
            xpoMatch.AddComma();
            xpoMatch.AddCapture();
            xpoMatch.AddEndParenthesis();
            xpoMatch.AddEndParenthesis();
            xpoMatch.AddSemicolon();
        }

        public override string RuleName()
        {
            return "ReportSection.addControl(t, FieldId2Ext(f, a)) -> ReportSection.addControl(t, f, a)";
        }

        public override string Run(string input)
        {
            Match match = xpoMatch.Match(input);

            if (match.Success)
            {
                string tableName  = match.Groups[1].Value.Trim();
                string globalUsage = match.Groups[2].Value.Trim();
                string fieldName = match.Groups[3].Value.Trim();
                string arrayIndex = match.Groups[4].Value.Trim();
    
                string updatedInput = input.Remove(match.Index, match.Length);
                updatedInput = updatedInput.Insert(match.Index, ".addControl(" + tableName+","+fieldName+", "+arrayIndex+");");

                return this.Run(updatedInput);
            }

            return input;
        }      
    }
}
