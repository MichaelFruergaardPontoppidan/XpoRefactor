using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleTreenodeApplObjectType : Rule
    {
        public override string RuleName()
        {
            return "Treenode.applObjectType() >> Treenode.UtilElement().RecordType()";
        }

        public override bool Enabled()
        {
            return false;
        }

        public override string Grouping()
        {
            return "Treenode";
        }
        protected override void buildXpoMatch()
        {
            xpoMatch.AddDot();
            xpoMatch.AddLiteral("applObjectType");
            xpoMatch.AddStartParenthesis();
            xpoMatch.AddEndParenthesis();
        }

        public override string Run(string input)
        {
            Match match = xpoMatch.Match(input);

            if (match.Success)
            {
                string updatedInput = input.Remove(match.Index, match.Length);
                updatedInput = updatedInput.Insert(match.Index,string.Format(".utilElement().RecordType"));

                return this.Run(updatedInput);
            }

            return input;
        }
    }
}
