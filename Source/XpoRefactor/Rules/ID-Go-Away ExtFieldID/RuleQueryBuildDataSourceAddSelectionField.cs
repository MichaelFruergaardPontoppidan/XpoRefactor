using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleQueryBuildDataSourceAddSelectionField : RuleQueryBuildDataBaseWithDefaultArg
    {
        override protected string methodName()
        {
            return "addSelectionField";
        }
        protected override string defaultArg()
        {
            return "SelectionField::Database";
        }
    }
}
