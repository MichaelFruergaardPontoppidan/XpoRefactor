using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

namespace XpoRefactor
{
    public class DeltaInfo
    {
        public int StartPos;
        public int EndPosAfter;
        public int EndPosBefore;

        public static DeltaInfo Construct(string before, string after)
        {
            if (before == after)
                return null;

            char[] beforeArray = before.ToCharArray();
            char[] afterArray = after.ToCharArray();
            bool deltaFound = false;
            int startPos = 0;
            int endPosBefore = 0;
            int endPosAfter = 0;
            int minLength = Math.Min(beforeArray.Length, afterArray.Length);
            for (int pos = 0; pos < minLength; pos++)
            {
                if (!deltaFound && beforeArray[pos] != afterArray[pos])
                {
                    deltaFound = true;
                    startPos = pos;
                }
                if (endPosBefore == 0 && deltaFound && (beforeArray[pos] == ';' || beforeArray[pos] == '\n'))
                {
                    endPosBefore = pos;
                }
                if (endPosBefore == 0 && deltaFound && (beforeArray[pos] == '#'))
                {
                    endPosBefore = pos - 11;
                }
                if (endPosAfter == 0 && deltaFound && (afterArray[pos] == ';' || afterArray[pos] == '\n'))
                {
                    endPosAfter = pos;
                }
                if (endPosAfter == 0 && deltaFound && (afterArray[pos] == '#'))
                {
                    endPosAfter = pos - 11;
                }

                if (endPosBefore != 0 && endPosAfter != 0)
                {
                    break;
                }
            }
            for (; startPos > 0; startPos--)
            {
                if (afterArray[startPos] == '#')
                    break;
            }
            startPos++;
            endPosBefore++;
            endPosAfter++;
            DeltaInfo deltaInfo = new DeltaInfo();
            deltaInfo.StartPos = startPos;
            deltaInfo.EndPosAfter = endPosAfter;
            deltaInfo.EndPosBefore = endPosBefore;

            return deltaInfo;
        }
    }
}