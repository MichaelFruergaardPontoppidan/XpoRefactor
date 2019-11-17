using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleDialogAddFieldValue : RuleDialogAddField
    {
        protected override string methodName()
        {
            return "addFieldValue";
        }
        public override string RuleName()
        {
            return "dialog.addFieldValue(typeId( -> dialog.addFieldValue(enumStr(";
        }
    }
}
