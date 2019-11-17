using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleQueryBuildDataSourceAddSortField : RuleQueryBuildDataBaseWithDefaultArg
    {
        override protected string methodName()
        {
            return "addSortField";
        }
        protected override string defaultArg()
        {
            return "SortOrder::Ascending";
        }
    }
}
