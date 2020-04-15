using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FtlcCommonLib;

namespace SnippetUtil
{
    class SuggestUIMngr
    {
        public RichTextBox mRichTextBox { get; }
        public ListBox mListBox { get; }

        public static SuggestUIMngr Current;

        public SuggestUIMngr(RichTextBox ftlRichTextBox, ListBox listBox)
        {
            mRichTextBox = ftlRichTextBox;
            mListBox = listBox;
            mRichTextBox.KeyDown += MRichTextBox_KeyDown;
            Current = this;
        }

        private void MRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (mListBox.Visible)
            {
                HandleSuggestBoxKeys(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.U)
            {
                //listBox1.Visible = true;
                var selPos = mRichTextBox.GetPositionFromCharIndex(mRichTextBox.SelectionStart);

                //var offset = new Point() { X = 5, Y = 50 };
                var offset = new Point() { X = 5, Y = 14 };
                var p1 = Utils.Add(/*this.Location, mRichTextBox.Location,*/ selPos, offset);

                mListBox.Location = p1;
                mListBox.Visible = true;
            }

        }

        internal void SetSuggestions(List<string> currentSuggestions)
        {
            mListBox.Items.Clear();
            currentSuggestions.ForEach(s => { mListBox.Items.Add(s); });
        }

        internal void HandleSuggestBoxKeys(object sender, KeyEventArgs e)
        {
            var ind = mListBox.SelectedIndex;
            var count = mListBox.Items.Count;
            if (e.KeyCode == Keys.Down)
            {
                if (ind == -1) { ind = 0; } else if (ind < count - 1) { ind++; } else { ind = 0; }
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                if (ind == -1) { ind = count; } else if (ind > 0) { ind--; } else { ind = count - 1; }
                e.Handled = true;
            }
            mListBox.SelectedIndex = ind;

            if (e.KeyCode == Keys.Escape)
            {
                mListBox.Visible = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (mListBox.SelectedItem != null)
                {
                    var sel = mListBox.SelectedItem.ToString();
                    System.Diagnostics.Debug.WriteLine(sel);
                    //var pos = mRichTextBox.SelectionStart;
                    //
                    //mRichTextBox.SelectionStart = pos;
                    //mRichTextBox.SelectionLength = 0;
                    //
                    //mRichTextBox.SelectedText = sel;
                    //richTextBox1.Text.Insert(pos, sel);
                    mListBox.Visible = false;
                    FieldManager.Current?.HandleSuggestionTxt(sel);
                    e.Handled = true;
                }
            }
        }

    }
}
