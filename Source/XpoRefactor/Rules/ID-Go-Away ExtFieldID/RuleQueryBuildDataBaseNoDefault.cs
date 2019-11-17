using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    abstract class RuleQueryBuildDataBaseNoDefault : RuleExtFieldIdBase
    {
        abstract protected string methodName();
        
        protected override void buildXpoMatch()
        {
            xpoMatch.AddDot();
            xpoMatch.AddLiteral(this.methodName());
            xpoMatch.AddStartParenthesis();
            xpoMatch.AddCaptureOptional("global[:][:]");
            xpoMatch.AddLiteral("fieldid2ext");
            xpoMatch.AddStartParenthesis();
            xpoMatch.AddLiteral("fieldnum");
            xpoMatch.AddStartParenthesis();
            xpoMatch.AddCapture();
            xpoMatch.AddComma();
            xpoMatch.AddCapture();
            xpoMatch.AddEndParenthesis();
            xpoMatch.AddComma();
            xpoMatch.AddCapture();
            xpoMatch.AddEndParenthesis();
            xpoMatch.AddEndParenthesis();            
        }
        public override string RuleName()
        {
            return "QueryBuildDataSource."+this.methodName()+"(FieldId2Ext(fieldNum(t, f), a)) -> QueryBuildDataSource."+this.methodName()+"(fieldNum(t, f), a)";
        }

        public override string Run(string input)
        {
//            Match match = Regex.Match(input, @"[.]" + this.methodName() + @"[\s]?[(][\s]?(global[:][:])?fieldid2ext[\s]?[(][\s]?fieldnum[\s]?[(][\s]?([\w]+)[\s]?[,][\s]?([\w]+)[)][\s]?[,]([\s\S]+?)[\s]?[)][\s]?[)]", RegexOptions.IgnoreCase);
            Match match = xpoMatch.Match(input);

            if (match.Success)
            {
                string globalUsage = match.Groups[1].Value.Trim();
                string tableName = match.Groups[2].Value.Trim();
                string fieldName = match.Groups[3].Value.Trim();
                string arrayIndex = match.Groups[4].Value.Trim();
                
                string updatedInput = input.Remove(match.Index, match.Length);
                updatedInput = updatedInput.Insert(match.Index, "."+this.methodName()+"(fieldNum(" + tableName + ", " + fieldName+"), "+ arrayIndex + ")");
                
                return this.Run(updatedInput);
            }
             
            return input;
        }      
    }
}
