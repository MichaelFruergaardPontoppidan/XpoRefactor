using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class AxMapPolicy : Rule
    {
        public override string RuleName()
        {
            return "useMapPolicy = {0}; -> this.enableMapPolicy()";
        }
        public override bool Enabled()
        {
            return false;
        }
        protected override void buildXpoMatch()
        {
            xpoMatch.AddLiteral("useMapPolicy");
            xpoMatch.AddWhiteSpace();
            xpoMatch.AddEqual();
            xpoMatch.AddWhiteSpace();
            xpoMatch.AddCapture();
            xpoMatch.AddWhiteSpace();
            xpoMatch.AddSemicolon();
        }
        override public string Grouping()
        {
            return "FrxSplit";
        }
        public override string Run(string input)
        {
            Match match = xpoMatch.Match(input);
            
            if (match.Success)
            {
                string value = match.Groups[1].Value.Trim();
                string updatedInput = input.Remove(match.Index, match.Length);
                if (value.ToLower() == "true")
                    updatedInput = updatedInput.Insert(match.Index, string.Format("this.enableMapPolicy();"));
                else
                    updatedInput = updatedInput.Insert(match.Index, string.Format("this.disableMapPolicy();"));
                return this.Run(updatedInput);
            }

            return input;
        }
    }
}
