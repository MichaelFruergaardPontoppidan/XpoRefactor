using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    public class XpoMatch
    {
        private StringBuilder builder;
        private Regex regex;

        public Match Match(string input)
        {
            if (regex == null)
            {
                regex = new Regex(this.Expression, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
            return regex.Match(input);
        }

        private string Expression
        {
            get { return builder.ToString(); }
        }
        public XpoMatch()
        {
            builder = new StringBuilder(100);            
        }
        public void AddWhiteSpaceRequired()
        {
            //include this line when running commit
            builder.Append(@"[\s]+");
        }
        public void AddWhiteSpace()
        {
            //include this line when running commit
            builder.Append(@"[\s]*");  
        }
        public void AddLiteral(string literal)
        {
            builder.Append(literal);
        }
        public void AddCapture()
        {
            this.AddWhiteSpace();
            builder.Append(@"([\S]+?)");
        }
        public void AddCaptureAnything()
        {
            this.AddWhiteSpace();
            builder.Append(@"([\S\s]+?)");
        }
        public void AddCaptureOptional()
        {
            this.AddWhiteSpace();
            builder.Append(@"([\S]?)");            
        }
        public void AddCaptureOptional(string literal)
        {
            this.AddWhiteSpace();
            builder.Append("("+literal+")?");            
        }
        public void AddStartBracket()
        {
            this.AddWhiteSpace();
            builder.Append(@"[\[]");
        }
        public void AddEndBracket()
        {
            this.AddWhiteSpace();
            builder.Append(@"[\]]");
        }
        public void AddStartParenthesis()
        {
            this.AddWhiteSpace();
            builder.Append(@"[(]");            
        }
        public void AddEndParenthesis()
        {
            this.AddWhiteSpace();
            builder.Append("[)]");            
        }
        public void AddComma()
        {
            this.AddWhiteSpace();
            builder.Append("[,]");
        }
        public void AddNewLine()
        {
            this.AddWhiteSpace();
            builder.Append("[#]");
            this.AddWhiteSpace();
        }
        public void AddSemicolon()
        {
            this.AddWhiteSpace();
            builder.Append("[;]");
        }
        public void AddDoubleColon()
        {
            this.AddWhiteSpace();
            builder.Append("[:][:]");
        }
        public void AddEqual()
        {
            builder.Append("[=]");
        }
        public void AddDot()
        {
            this.AddWhiteSpace();
            builder.Append("[.]");            
        }
    }
}
