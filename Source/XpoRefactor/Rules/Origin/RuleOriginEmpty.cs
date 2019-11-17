using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleOriginEmpty : Rule
    {
        public override string RuleName()
        {
            return "Origin #{00000000-0000-0000-0000-000000000000} >> newGuid()";
        }

        public override bool Enabled()
        {
            return false;
        }

        public override string Grouping()
        {
            return "Origin";
        }
        protected override void buildXpoMatch()
        {
            xpoMatch.AddLiteral("Origin");
            xpoMatch.AddWhiteSpace();
            xpoMatch.AddLiteral("#{");
            xpoMatch.AddCaptureAnything();
            xpoMatch.AddLiteral("}");            
        }

        public override string Run(string input)
        {
            Match match = xpoMatch.Match(input);

            if (match.Success)
            {
                string zeroOrigin = match.Groups[1].Value.Trim();
                if (zeroOrigin == "00000000-0000-0000-0000-000000000000")
                {
                    Guid g = Guid.NewGuid();
                    

                    string updatedInput = input.Remove(match.Groups[1].Index, match.Groups[1].Length);
                    updatedInput = updatedInput.Insert(match.Groups[1].Index, g.ToString().ToUpper());

                    return this.Run(updatedInput);
                }
            }

            return input;
        }
    }
}
