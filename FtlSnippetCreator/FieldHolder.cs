using FtlcCommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FtlSnippetCreator
{
    class FieldHolder
    {
        private int selStart;
        public string SelTxt { get; set; }
        public string FldTxt { get; set; }

        public FieldHolder(int selStart, string selTxt, string fldTxt)
        {
            this.SelStart = selStart;
            this.SelEnd   = selStart + selTxt.Length;
            this.SelTxt = selTxt;
            this.FldTxt = fldTxt;
        }

        public int SelStart { get => selStart; set => selStart = value; }
        public int SelEnd { get; }

        public int LenDiff => FldTxt.Length - SelTxt.Length;

        //public List<int> Occurrences { get; private set; }

        public static List<FieldHolder> Occurrences2Holders(string plainText, string searchStr)
        {
            var fldTxt = $"{Constants.MStartStr}{searchStr}{Constants.MEndStr}";
            List<int> occurrences = CheckOccurrences(plainText, searchStr);
            return occurrences.Select(o => new FieldHolder(o, searchStr, fldTxt)).ToList();
        }

        private static List<int> CheckOccurrences(string plainText, string searchStr)
        {
            //Regex rx = new Regex(SelTxt);
            //rx.Matches(plainText);
            List<int> inds = new List<int>();
            int ind = 0;
            while (ind != -1) {
                int searchInd = inds.Count() == 0 ? 0 : inds.Last() + searchStr.Length;
                var searchTxt = plainText.Substring(searchInd);
                System.Diagnostics.Debug.WriteLine(searchTxt);
                var foundInd = searchTxt.IndexOf(searchStr);
                if (foundInd == -1) {
                    break;
                }
                foundInd = searchInd + foundInd;
                inds.Add(foundInd);
                ind = foundInd;
            }
            return inds;
        }
    }
}
