﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleTreenodeAOTObjectNode : Rule
    {
        public override string RuleName()
        {
            return "Treenode.AOTObjectNode() >> Treenode.TreeNodeType().isRootElement()";
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
            xpoMatch.AddLiteral("AOTObjectNode");
            xpoMatch.AddStartParenthesis();
            xpoMatch.AddEndParenthesis();
        }

        public override string Run(string input)
        {
            Match match = xpoMatch.Match(input);

            if (match.Success)
            {
                string updatedInput = input.Remove(match.Index, match.Length);
                updatedInput = updatedInput.Insert(match.Index,string.Format(".TreeNodeType().isRootElement()"));

                return this.Run(updatedInput);
            }

            return input;
        }
    }
}
