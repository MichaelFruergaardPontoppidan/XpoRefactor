using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleQueryBuildDataSourceAddOrderByField : RuleQueryBuildDataBaseWithDefaultArg
    {
        override protected string methodName()
        {
            return "addOrderByField";
        }
        protected override string defaultArg()
        {
            return "SortOrder::Ascending";
        }
    }
}
