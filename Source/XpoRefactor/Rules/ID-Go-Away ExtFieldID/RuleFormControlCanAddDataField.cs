using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleFormControlCanAddDataField : RuleArgFieldArr
    {
        protected override string methodName()
        {
            return "canAddDataField";
        }
        public override string RuleName()
        {
            return "FormControl."+base.RuleName();
        }
    }
}
