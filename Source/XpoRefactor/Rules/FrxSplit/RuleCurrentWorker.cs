using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleCurrentWorker : Rule
    {
        public override string RuleName()
        {
            return "currentWorker()-> HcmWorker::CurrentWorker()";
        }
        public override bool Enabled()
        {
            return false;
        }
        protected override void buildXpoMatch()
        {
            xpoMatch.AddEqual();
            xpoMatch.AddWhiteSpace();
            xpoMatch.AddLiteral("currentWorker");
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
            XpoMatch match2;
            
            if (match.Success)
            {
                string updatedInput = input.Remove(match.Index, match.Length);
                updatedInput = updatedInput.Insert(match.Index, string.Format("= HcmWorker::currentWorker()"));
                return this.Run(updatedInput);
            }

            match2 = new XpoMatch();
            
            
            match2.AddLiteral("global");
            match2.AddDoubleColon();
            match2.AddLiteral("currentWorker");
            match2.AddStartParenthesis();
            match2.AddEndParenthesis();

            match = match2.Match(input);
            if (match.Success)
            {
                string updatedInput = input.Remove(match.Index, match.Length);
                updatedInput = updatedInput.Insert(match.Index, string.Format("HcmWorker::currentWorker()"));
                return this.Run(updatedInput);
            }
            
            match2 = new XpoMatch();
            match2.AddStartParenthesis();
            match2.AddWhiteSpace();
            match2.AddLiteral("currentWorker");
            match2.AddStartParenthesis();
            match2.AddEndParenthesis();
            match2.AddEndParenthesis();

            match = match2.Match(input);
            if (match.Success)
            {
                string updatedInput = input.Remove(match.Index, match.Length);
                updatedInput = updatedInput.Insert(match.Index, string.Format("(HcmWorker::currentWorker())"));
                return this.Run(updatedInput);
            }

            match2 = new XpoMatch();
            match2.AddComma();
            match2.AddWhiteSpace();
            match2.AddLiteral("currentWorker");
            match2.AddStartParenthesis();
            match2.AddEndParenthesis();
            match2.AddEndParenthesis();

            match = match2.Match(input);
            if (match.Success)
            {
                string updatedInput = input.Remove(match.Index, match.Length);
                updatedInput = updatedInput.Insert(match.Index, string.Format(", HcmWorker::currentWorker()"));
                return this.Run(updatedInput);
            }

            return input;
        }
    }
}
