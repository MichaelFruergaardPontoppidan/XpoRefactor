using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class InfologDocu : Rule
    {
        public override string RuleName()
        {
            return "infolog.docu() -> Docu::getInstance()";
        }
        public override bool Enabled()
        {
            return true;
        }
        protected override void buildXpoMatch()
        {
            xpoMatch.AddLiteral("infolog");
            xpoMatch.AddDot();
            xpoMatch.AddLiteral("docu");
            xpoMatch.AddStartParenthesis();
            xpoMatch.AddEndParenthesis();
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
                string updatedInput = input.Remove(match.Index, match.Length);
                updatedInput = updatedInput.Insert(match.Index, string.Format("Docu::getInstance()"));
                return this.Run(updatedInput);
            }

            return input;
        }
    }
}
