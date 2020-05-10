using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapboard
{
    public class Highlighter
    {
        private Rewrite mRewrite;

        public Highlighter(Rewrite rewrite)
        {
            this.mRewrite = rewrite;
        }

        internal void Highlight(Rewrite.FieldPlace fpl)
        {
            var selCol = mRewrite.RichTextBox.SelectionColor;
            var selStart = mRewrite.RichTextBox.SelectionStart;
            var selLen   = mRewrite.RichTextBox.SelectionLength;

            mRewrite.RichTextBox.SelectionColor = Color.LightGray;

            mRewrite.FieldPlaces.Where(fp1 => fp1.FldName==fpl.FldName).ToList().ForEach(fp =>
            {
                int len = fp.OutPutTextEnd - fp.OutPutTextStart;
                mRewrite.RichTextBox.Select(fp.OutPutTextStart, len);
                //mRewrite.RichTextBox.SelectionBackColor = Color.LightSalmon;
                //mRewrite.RichTextBox.SelectionBackColor = Color.LightGoldenrodYellow;
                mRewrite.RichTextBox.SelectionBackColor = Color.LightGray;
            });
            //mRewrite.RichTextBox

            mRewrite.RichTextBox.SelectionStart  = selStart;
            mRewrite.RichTextBox.SelectionLength = selLen;
            mRewrite.RichTextBox.SelectionColor = selCol;
        }

        internal void Unhighlight()
        {
            var selCol = mRewrite.RichTextBox.SelectionColor;
            var selStart = mRewrite.RichTextBox.SelectionStart;
            var selLen = mRewrite.RichTextBox.SelectionLength;

            mRewrite.RichTextBox.SelectionColor = Color.White;

            mRewrite.FieldPlaces.ForEach(fp =>
            {
                int len = fp.OutPutTextEnd - fp.OutPutTextStart;
                mRewrite.RichTextBox.Select(fp.OutPutTextStart, len);
                //mRewrite.RichTextBox.SelectionBackColor = Color.LightSalmon;
                //mRewrite.RichTextBox.SelectionBackColor = Color.LightGoldenrodYellow;
                mRewrite.RichTextBox.SelectionBackColor = Color.White;
            });
            //mRewrite.RichTextBox

            mRewrite.RichTextBox.SelectionStart = selStart;
            mRewrite.RichTextBox.SelectionLength = selLen;
            mRewrite.RichTextBox.SelectionColor = selCol;
        }
    }
}
