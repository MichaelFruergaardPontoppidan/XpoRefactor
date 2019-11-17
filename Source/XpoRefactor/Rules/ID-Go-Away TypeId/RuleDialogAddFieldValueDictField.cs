using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleDialogAddFieldValueDictField : RuleDialogAddFieldDictField
    {
        protected override string methodName()
        {
            return "addFieldValue";
        }
    }
}
