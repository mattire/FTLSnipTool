using System;
using System.Text.RegularExpressions;

namespace SnippetUtil
{
    partial class FieldManager
    {
        public class FieldHolder {
            public FieldHolder(Match m, int order, int lfCount)
            {
                //var stLen = FieldManager.MStartStr.Length;
                //var enLen = FieldManager.MEndStr.Length;
                var stLen = 2;
                var enLen = 2;
                int len = m.Value.Length - (stLen + enLen);
                Name = m.Value.Substring(stLen,len);
                OrigStr = m.Value;
                Start = m.Index;
                End = m.Index + m.Length;
                RBStart = Start - order * (stLen + enLen) - lfCount;
                RBEnd = End - (order +1) * (stLen + enLen) - lfCount;
                RBLen = RBEnd - RBStart;
            }   

            public String OrigStr { get; set; }
            public String Name { get; set; }
            public int Start { get; set; }
            public int End { get; set; }
            public int RBStart    { get; set; }
            public int RBEnd    { get; set; }
            public int RBLen { get; set; }
        }
    }
}
