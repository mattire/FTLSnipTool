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

    partial class FieldManager
    {
        private string mContents;
        private SuggestionMngr mSuggestMngr;

        public static string MStartStrRE { get; } = "#\\*";
        public static string MEndStrRE { get; } = "\\*#";
        public static string MStartStr { get; } = "#*";
        public static string MEndStr { get; } = "*#";
        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

        public List<string> CurrentSuggestions {
            get => _currentSuggestions;
            set {
                _currentSuggestions = value;
                SuggestUIMngr.Current?.SetSuggestions(_currentSuggestions);
            }
        }

        public RichTextBox MRichTextBox
        {
            get { return mRichTextBox; }
            set
            {
                mRichTextBox = value;
                mRichTextBox.KeyDown += MRichTextBox_KeyDown;
                ((FTLRichTextBox)mRichTextBox).TabForward += FieldManager_TabForward;
                ((FTLRichTextBox)mRichTextBox).TabBackward += FieldManager_TabBackward;
                ((FTLRichTextBox)mRichTextBox).RtbKeyDown += FieldManager_KeyDown;
            }
        }

        internal List<FieldHolder> MHolders { get => mHolders; set => mHolders = value; }
        internal SuggestionMngr MSuggestMngr { get => mSuggestMngr; set => mSuggestMngr = value; }

        MatchCollection mMatchCollection;
        private int mStartLen;
        private int mEndLen;
        private RichTextBox mRichTextBox;
        private List<FieldHolder> mHolders;

        public static FieldManager Current;
        public FieldManager()
        {
            Current = this;
        }

        private void MRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
        }

        //public FieldManager(string contents, RichTextBox richTextBox) { }

        public void UpdateContents(string contents, string snipPath)
        {
            mInputHandler = new InputHandler(this);

            //mRichTextBox = richTextBox;
            //mRichTextBox.KeyDown += MRichTextBox_KeyDown;
            //((FTLRichTextBox)mRichTextBox).TabForward  += FieldManager_TabForward;
            //((FTLRichTextBox)mRichTextBox).TabBackward += FieldManager_TabBackward;
            //((FTLRichTextBox)mRichTextBox).RtbKeyDown  += FieldManager_KeyDown;

            this.mContents = contents;
            mSuggestMngr = new SuggestionMngr(snipPath);
            mStartLen = MStartStrRE.Length;
            mEndLen = MEndStrRE.Length;
            Refresh();
            mSuggestMngr.SetOrigFlds(mMatchCollection);
        }

        //int sessionStrLen=0;
        //StringBuilder fldSessionBldr = new StringBuilder();
        InputHandler mInputHandler;
        private void FieldManager_KeyDown(object sender, EventArgs e)
        {
            var kea = (KeyEventArgs)e;
            mInputHandler.Handle(kea);
        }

        internal void HandleSuggestionTxt(string txt)
        {
            var hldr = this.mHolders[selectedFld.val];
            mRichTextBox.Focus();
            mRichTextBox.Select(hldr.RBStart, hldr.RBLen);
            mRichTextBox.SelectedText = txt;
            mInputHandler.Handle(txt);
        }
        private void FieldManager_TabBackward(object sender, EventArgs e)
        {
            SessionEnd();
            PrevField();
        }

        private void FieldManager_TabForward(object sender, EventArgs e)
        {
            SessionEnd();
            HighlightFields();
            NextField();
        }

        private void SessionEnd()
        {
            if (mInputHandler.Changed)
            {
                //var newFldLen = mInputHandler.FldLen;
                var newFldLen = mInputHandler.Len;
                var currentInt = selectedFld.val;
                var hldr = mHolders[currentInt];
                var newFldContent = mRichTextBox.Text.Substring(hldr.RBStart, newFldLen);
                newFldContent = newFldContent.Replace("\n", "");

                mSuggestMngr.AddSuggestion(selectedFld.val, newFldContent);
                var newFldCntntWSE = MStartStr + newFldContent + MEndStr;
                // construct new content
                var fstPart = mContents.Substring(0, hldr.Start);
                var mdlPart = newFldCntntWSE;
                var lstPart = mContents.Substring(hldr.End, mContents.Length - hldr.End);
                var newContent = fstPart + mdlPart + lstPart;
                System.Diagnostics.Debug.WriteLine(newContent);

                Refresh(newContent);
                mInputHandler.Len = 0;
            }
        }

        public void AddCurrentFldToSuggestion()
        {
            var hldr = mHolders[selectedFld.val];
            var newFldLen = mInputHandler.Len;
            mSuggestMngr.AddSuggestion(
                selectedFld.val, 
                mRichTextBox.Text.Substring(hldr.RBStart, newFldLen)); 
        }

        public void Refresh(string newContents)
        {
            this.mContents = newContents;
            Refresh();
        }

        public void Refresh()
        {
            var oldSel = selectedFld?.val;
            Regex rx = new Regex($"{MStartStrRE}.*?{MEndStrRE}");
            mMatchCollection = rx.Matches(this.mContents);
            ProcessContent(oldSel);
            mInputHandler.Changed = false;
            mSuggestMngr.UpdCurrentNamesMap(mMatchCollection);
        }

        private void ProcessContent(int? oldSel)
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
            else
            {
                theRest = mContents;
            }
            sb.Append(theRest);

            mRichTextBox.Text = sb.ToString();
            SelectField(oldSel);
        }

        internal void NextField()
        {
            selectedFld.Inc();
            SetSelected();
        }


        internal void PrevField()
        {
            selectedFld.Dec();
            SetSelected();
        }

        private void SetSelected()
        {
            try
            {

                if (mHolders.Count != 0)
                {
                    var hldr = this.mHolders[selectedFld.val];
                    mRichTextBox.Focus();
                    mRichTextBox.Select(hldr.RBStart, hldr.RBLen);
                    mRichTextBox.SelectionBackColor = Color.Yellow;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        Rint selectedFld;
        private List<string> _currentSuggestions;

        public void SelectField(int? ind = null)
        {
            ind = ind == null ? 0 : ind;
            if (ind < mHolders.Count)
            {
                selectedFld = new Rint((int)ind, mHolders.Count);
            }
            else { selectedFld = new Rint(0, mHolders.Count); }
            //mRichTextBox.SelectAll();
            SetSelected();
        }

        public void HighlightFields()
        {
            foreach (var h in mHolders)
            {
                mRichTextBox.Select(h.RBStart, h.RBLen);
                mRichTextBox.SelectionBackColor = Color.LightSalmon;
            }
            if (mHolders.Count != 0)
            {
                var fst = mHolders.First();
                mRichTextBox.Select(fst.RBStart, fst.RBLen);
            }

            CurrentSuggestions = mSuggestMngr.GetSuggestions(selectedFld.val);
        }

        //public void GetFldRange(int ind, out int start, out int end) {
        //    if (mMatchCollection.Count < ind) {
        //        start = -1;
        //        end = -1;
        //        return;
        //    }
        //    var c = mMatchCollection[ind];
        //    start = c.Index + mStartLen;
        //    end = c.Index +  c.Length - mEndLen;
        //}
    }
}
