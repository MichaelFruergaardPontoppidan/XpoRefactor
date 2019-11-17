using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    abstract class RuleArgFieldArr : RuleExtFieldIdBase
    {
        public override string RuleName()
        {
            return this.methodName()+"(t, FieldId2Ext(f, a)) -> "+this.methodName()+"(t, f, a)";
        }
        abstract protected string methodName();
        
        protected override void buildXpoMatch()
        {
            xpoMatch.AddLiteral(this.methodName());
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
        }

        public override string Run(string input)
        {
//            Match match = Regex.Match(input, @"fieldid2name[\s]?[(][\s]?([\S]+)[\s]?[,][\s]?(global[:][:])?fieldid2ext[\s]?[(][\s]?([\S]+)[\s]?[,][ ]?([\S]+)[)][\s]?[)]", RegexOptions.IgnoreCase);
            Match match = xpoMatch.Match(input);

            if (match.Success)
            {
                string tableName  = match.Groups[1].Value.Trim();
                string globalUsage = match.Groups[2].Value.Trim();
                string fieldName = match.Groups[3].Value.Trim();
                string arrayIndex = match.Groups[4].Value.Trim();
    
                string updatedInput = input.Remove(match.Index, match.Length);
                updatedInput = updatedInput.Insert(match.Index, this.methodName()+"(" + tableName+", "+fieldName+", "+arrayIndex+")");

                return this.Run(updatedInput);
            }

            return input;
        }      
    }
}
