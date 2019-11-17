using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleQueryBuildDataSourceAddGroupByField : RuleQueryBuildDataBaseNoDefault
    {
        override protected string methodName()
        {
            return "addGroupByField";
        }
    }
}
