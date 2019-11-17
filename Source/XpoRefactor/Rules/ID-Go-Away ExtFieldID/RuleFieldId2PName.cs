using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleFieldId2PName : RuleArgFieldArr
    {
        protected override string methodName()
        {
            return "fieldId2PName";
        }
    }
}
