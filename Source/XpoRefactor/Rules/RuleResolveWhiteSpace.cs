using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleResolveWhiteSpace : Rule
    {
        public override string RuleName()
        {
            return "Resolve WhiteSpace";
        }

        public override bool Enabled()
        {
            return true;
        }

        public override string Grouping()
        {
            return "";
        }
        protected override void buildXpoMatch()
        {
            xpoMatch.AddSymbol(">", 4);
            xpoMatch.AddWhiteSpaceRequired();
            xpoMatch.AddLiteral("ORIGINAL");
            xpoMatch.AddCaptureAnything();
            xpoMatch.AddSymbol("<", 4);
            xpoMatch.AddWhiteSpaceRequired();
            xpoMatch.AddLiteral("END");
            xpoMatch.AddWhiteSpaceRequired();
            xpoMatch.AddSymbol("+",48);

        }
        public override string Run(string input)
        {
            return this.Run(input, 0);
        }
        public string Run(string input, int startAt = 0)
        {
            Match match = xpoMatch.Match(input, startAt);

            if (match.Success)
            {
                string capture = match.Captures[0].ToString();
                string[] sep = new string[3];
                sep[0] = "++++++++++++++++++++++++++++++++++++++++++++++++\r\n";
                sep[1] = "====";
                sep[2] = "<<<<";
                string[] split = capture.Split(sep, StringSplitOptions.None);

                string original = split[1];
                string theirs = split[3];
                string yours = split[5];


                string originalTrim = original.Replace(" ", "").Replace("\r", "").Replace("\n", "").ToLowerInvariant();
                string theirsTrim = theirs.Replace(" ", "").Replace("\r", "").Replace("\n", "").ToLowerInvariant();
                string yoursTrim = yours.Replace(" ", "").Replace("\r", "").Replace("\n", "").ToLowerInvariant();

                string output = "XXX";

                if (originalTrim == theirsTrim)
                    output = yours;
                else if (originalTrim == yoursTrim)
                    output = theirs;
                else if (theirsTrim == yoursTrim)
                    output = yours;

                if (output != "XXX")
	            {
                    string updatedInput = input.Remove(match.Index, match.Length);
                    updatedInput = updatedInput.Insert(match.Index, output);
                    return this.Run(updatedInput, match.Index);
		 
	            }
                return this.Run(input, match.Index+4);

            }

            return input;
        }
    }
}
