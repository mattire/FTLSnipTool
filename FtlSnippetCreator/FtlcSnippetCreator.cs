using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FtlcCommonLib;

namespace FtlSnippetCreator
{
    public partial class FtlcSnippetCreator : Form
    {
        private List<FieldHolder> mFldHolders;

        public string MPlainText  { get; set; }
        public string MFieldsText { get; set; }
        public string MDisplayTxt { get; set; }
        private ContentMngr ContentMngr { get; set; }

        public FtlcSnippetCreator()
        {
            InitializeComponent();
            mFldHolders = new List<FieldHolder>();
            ContentMngr = new ContentMngr();
            if (Clipboard.ContainsText()) {
                richTextBox1.Text = Clipboard.GetText();
                richTextBox2.Text = Clipboard.GetText();
                MPlainText = richTextBox1.Text;
                MFieldsText = richTextBox1.Text;
            }
        }

        private void UpdateLinefeedMap() {
            //MPlainText = richTextBox1.Text;
            Func<char, List<int>> GetCharLocations = (chr) =>
            {
                return richTextBox1.Text
                            .Select((ch, i) => new { i, ch })
                            .Where(t => t.ch == chr).Select(t => t.i)
                            .ToList();
            };

            var lineFeeds = richTextBox1.Text.Select((ch, i) => new { i, ch }).Where(t => t.ch == 
            '\n').ToList();
        }

        private int SpclChCountBeforeInd(int index) {
            Func<int, char, int> CountOfCharsBeforeInd = (i, ch) => {
                            return richTextBox1.Text
                                    .Where((chr, ind) =>
                                                ind <  i && 
                                                chr == ch).Count();
            };
            var lfCount = CountOfCharsBeforeInd(index, '\n');
            var crCount = CountOfCharsBeforeInd(index, '\r');
            return lfCount + crCount;
        }

        private void toFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // update MPlainText
            MPlainText = richTextBox1.Text;

            var allTxt = richTextBox1.Text;
            var selTxt = richTextBox1.SelectedText;
            //var selStart = richTextBox1.SelectionStart;

            var fhs = FieldHolder.Occurrences2Holders(MPlainText, selTxt);

            //var fldTxt = $"{Constants.MStartStr}{selTxt}{Constants.MEndStr}";
            //var fldHldr = new 
            //    FieldHolder(selStart, selTxt, fldTxt);

            //mFldHolders.Add(fldHldr);
            var occur = mFldHolders.FirstOrDefault(h => h.SelTxt == selTxt);
            if (occur == null) {
                mFldHolders.AddRange(fhs);
            }

            GenNewFldTxt();
        }

        private void GenNewFldTxt()
        {
            var ordrd = mFldHolders.OrderBy(fh => fh.SelStart);
            //int cumulDiff = 0;


            Func<FieldHolder, FieldHolder, string> GetPartBetween = (fh1, fh2) => {
                var spc1 = fh1 !=null ? SpclChCountBeforeInd(fh1.SelStart):0;
                var spc2 = fh2 !=null ? SpclChCountBeforeInd(fh2.SelStart):richTextBox1.Text.Length;
                var start = fh1 != null ? fh1.SelEnd + spc1 : 0;
                var end = fh2 != null ? fh2.SelStart + spc2 : MPlainText.Length;
                //var end = fh2 != null ? fh2.SelStart : (PlainText.Length - (fh1.SelStart + fh1.SelTxt.Length));
                return MPlainText.Substring(start, end-start);
            };
            List<string> parts = new List<string>();

            parts.Add(GetPartBetween(null, ordrd.First()));

            ordrd.Aggregate((fh1, fh2) => {
                parts.Add(GetPartBetween(fh1, fh2));
                return fh2;
            });
            parts.Add(GetPartBetween(ordrd.Last(), null));

            var sb = new StringBuilder();
            int ind = 0;
            sb.Append(parts.ElementAt(ind));

            foreach (var holder in ordrd) {
                ind++;
                var hldTxt     = holder.FldTxt       ;
                var betweenTxt = parts.ElementAt(ind);
                sb.Append(hldTxt    );
                sb.Append(betweenTxt);
                //cumulDiff += holder.LenDiff;
            }

            MFieldsText = sb.ToString();
            richTextBox2.Text = MFieldsText;
            //mFldHolders.Sort(fh => fh.SelStart);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mFldHolders.Clear();
            richTextBox2.Text = richTextBox1.Text;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(Application.ExecutablePath);
            //richTextBox2.Text
            var appSettings = ConfigurationManager.AppSettings;
            saveFileDialog1.InitialDirectory = appSettings["SaveDir"];

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream saveStream;
                if ((saveStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    using (var sw = new StreamWriter(saveStream))
                    {
                        sw.Write(richTextBox2.Text);
                    }
                    saveStream.Close();
                }
            }
        }
    }
}
