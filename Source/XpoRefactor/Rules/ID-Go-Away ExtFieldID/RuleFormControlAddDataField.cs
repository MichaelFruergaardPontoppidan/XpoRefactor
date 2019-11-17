using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleFormControlAddDataField : RuleExtFieldIdBase
    {
        public override string RuleName()
        {
            return "FormControl.addDataField(ds, FieldId2Ext(f, a)) -> FormControl.addDataField(ds, f, fc.controlNum(fc.controlCount()), a)";
        }
                
        protected override void buildXpoMatch()
        {            
            xpoMatch.AddCapture();
            xpoMatch.AddDot();
            xpoMatch.AddLiteral("addDataField");
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

        public override string Run(string input)
        {
            if (!input.ToLower().Contains("adddatafield"))
                return input;

            Match match = xpoMatch.Match(input);

            if (match.Success)
            {
                string controlName = match.Groups[1].Value.Trim();
                string datasourceName = match.Groups[2].Value.Trim();
                string globalUsage = match.Groups[3].Value.Trim();
                string fieldName = match.Groups[4].Value.Trim();
                string arrayIndex = match.Groups[5].Value.Trim();
    
                string updatedInput = input.Remove(match.Index, match.Length);
                updatedInput = updatedInput.Insert(match.Index, 
                    string.Format(" {0}.addDataField({1}, {2}, {3});",
                        controlName , datasourceName, fieldName, arrayIndex));

                return this.Run(updatedInput);
            }

            return input;
        }      
    }
}
