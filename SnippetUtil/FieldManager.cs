using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnippetUtil
{
    class Rint
    {
        public int val { get; set; }
        public int lim { get; set; }
        public Rint(int v, int m) { val = v; lim = m; }
        public void Inc() { val++; if (val >= lim) val = 0; }
        public void Dec() { val--; if (val <= lim) val = lim - 1; }
    }

    class FieldManager
    {
        private string mContents;

        public static string MStartStr { get; } = "#\\*";
        public static string MEndStr { get; } = "\\*#";

        MatchCollection mMatchCollection;
        private int mStartLen;
        private int mEndLen;
        private RichTextBox mRichTextBox;
        private List<FieldHolder> mHolders;

        public FieldManager()
        {
        }

        private void MRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
        }

        public FieldManager(string contents, RichTextBox richTextBox)
        {
            mRichTextBox = richTextBox;
            mRichTextBox.KeyDown += MRichTextBox_KeyDown;

            this.mContents = contents;
            mStartLen = MStartStr.Length;
            mEndLen = MEndStr.Length;
            Refresh();
        }

        public void Refresh(string newContents)
        {
            this.mContents = newContents;
            Refresh();
        }

        public void Refresh()
        {
            Regex rx = new Regex($"{MStartStr}.*?{MEndStr}");
            mMatchCollection = rx.Matches(this.mContents);
            ProcessContent();
        }

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

        private void ProcessContent()
        {
            StringBuilder sb = new StringBuilder();
            int ind = 0;
            mHolders = new List<FieldHolder>();
            int partStart = 0;
            int lfCount = 0;
            foreach (Match m in mMatchCollection)
            {
                string part = mContents.Substring(partStart, m.Index - partStart);
                sb.Append(part);
                lfCount += part.Where(ch => ch == '\n').Count();
                var fh = new FieldHolder(m, ind, lfCount);
                ind++;
                sb.Append(fh.Name);
                partStart = m.Index + m.Length;
                mHolders.Add(fh);
            }
            string theRest = string.Empty;
            if (mHolders.Count != 0)
            {
                theRest = mContents.Substring(mHolders.Last().End);
            }
            else {
                theRest = mContents;
            }
            sb.Append(theRest);

            mRichTextBox.Text = sb.ToString();
        }

        internal void NextField()
        {
            
        }

        internal void PrevField()
        {
            
        }

        Rint selectedFld;
        public void SelectField(int ind)
        {
            var hldr = this.mHolders[ind];
            mRichTextBox.Select(hldr.RBStart, hldr.RBLen);
        }

        public void HighlightFields()
        {
            foreach (var h in mHolders)
            {
                mRichTextBox.Select(h.RBStart, h.RBLen);
                mRichTextBox.SelectionBackColor = Color.LightSalmon;
            }
            
        }

        public void GetFldRange(int ind, out int start, out int end) {
            if (mMatchCollection.Count < ind) {
                start = -1;
                end = -1;
                return;
            }
            var c = mMatchCollection[ind];
            start = c.Index + mStartLen;
            end = c.Index +  c.Length - mEndLen;
        }
    }
}
