using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XpoRefactor
{
    class RuleDialogAddField : RuleTypeIdBase
    {
        protected virtual string methodName()
        {
            return "addField";
        }
        public override string RuleName()
        {
            return "dialog.addField(typeId( -> dialog.addField(enumStr(";
        }

        public override string Run(string input)
        {
            string classQualifier = "";

            Match match = Regex.Match(input, classQualifier + "." + this.methodName() + @"[ ]?[(][ ]?typeid[ ]?[(]([\w]+)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string typeName = match.Groups[1].Value.Trim();
                
                string updatedInput = input.Remove(match.Index, match.Length);
                bool changed = false;

                if (Dictionary.isEnum(typeName))
                {
                    updatedInput = updatedInput.Insert(match.Index, classQualifier + "." + this.methodName() + "(enumStr(" + typeName);
                    changed = true;
                }
                else if (Dictionary.isType(typeName))
                {
                    updatedInput = updatedInput.Insert(match.Index, classQualifier + "." + this.methodName() + "(extendedTypeStr(" + typeName);
                    changed = true;
                }
                if (changed)
                {
                    return this.Run(updatedInput);
                }                
            }
            return input;
        }
    }
}
