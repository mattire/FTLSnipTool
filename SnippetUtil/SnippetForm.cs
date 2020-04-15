using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnippetUtil
{
    public partial class FtlSnippetForm : Form
    {
        private const string SnippetFld = ".\\Snippets";
        private FieldManager fldMngr = new FieldManager();

        private string mClipboardOrigTxt;

        static public int? StartX { get; internal set; }
        static public int? StartY { get; internal set; }
        
        public FtlSnippetForm(string clipTxt)
        {
            mClipboardOrigTxt = clipTxt;
            InitializeComponent();
            RefreshSnippets();
            //txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSearchKeyPress);
            txtSearch.KeyDown += TxtSearch_KeyDown;
            //txtSnippet.Select
            fldMngr.MRichTextBox = richTextBoxSnippet;
            ((FTLRichTextBox)richTextBoxSnippet).EnterPressed += btnOk_Click;
            txtSearch.Focus();
            if (StartX != null && StartY != null)
            {
                StartPosition = FormStartPosition.Manual;
                Location = new Point(100, 100);
            }
            else {
                StartPosition = FormStartPosition.CenterScreen;
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.KeyCode);
            if (e.KeyCode == Keys.Enter)
            {
                var fn = txtSearch.Text + ".txt";
                var path = Path.Combine(SnippetFld, fn);
                var contents = File.ReadAllText(path);

                contents = HandleSurroundScripts(contents);

                txtSnippet.Text = contents;
                //fldMngr.
                fldMngr.UpdateContents(contents, path);

                //fldMngr = new FieldManager(contents, richTextBoxSnippet);

                fldMngr.HighlightFields();
                //var caps = CaptureFields(contents);
                //foreach (Capture c in caps)
                //{
                //    ListViewItem lvi = new ListViewItem(c.Value);
                //    listViewFields.Items.Add(lvi);
                //}
                richTextBoxSnippet.Focus();
            }
        }

        private string HandleSurroundScripts(string contents)
        {
            var searchStr = FieldManager.MStartStr + "SurroundContent" + FieldManager.MEndStr;
            if (contents.Contains(searchStr) && mClipboardOrigTxt!="") {
                contents = contents.Replace(searchStr, mClipboardOrigTxt);
            }
            return contents;
            //fldMngr.MHolders.ForEach(h => System.Diagnostics.Debug.WriteLine(h.OrigStr));
            //fldMngr.MHolders.ForEach(h => System.Diagnostics.Debug.WriteLine(h.Name));
            //var sc = fldMngr.MHolders.FirstOrDefault(h => h.Name == "SurroundContent");
        }

        private void RefreshSnippets()
        {
            System.Diagnostics.Debug.WriteLine(Directory.GetCurrentDirectory());

            List<string> fls = Directory.GetFiles(".\\Snippets").ToList();
            List<string> trimmed = new List<string>();
            fls.ForEach(s => 
            {
                var tr = s.Replace('.', ' ').Replace('\\', ' ').Trim().Split(' ')[1];
                trimmed.Add(tr);
            });
            
            this.txtSearch.AutoCompleteCustomSource.Clear();

            this.txtSearch.AutoCompleteCustomSource.AddRange(trimmed.ToArray());
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Clipboard.GetText();
            Clipboard.SetText(richTextBoxSnippet.Text);
            fldMngr.MSuggestMngr.Save();
            this.Dispose();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText(mClipboardOrigTxt);
            Clipboard.SetText("");
            this.Close();
        }


        private CaptureCollection CaptureFields(string contents)
        {
            Regex rx = new Regex("#\\*.*?\\*#");
            var m = rx.Match(contents);
            return m.Captures;
        }

        private void Test() {
            richTextBoxSnippet.Select(0, 5);
            richTextBoxSnippet.SelectionBackColor = Color.AliceBlue;
        }

        private void RbEntered(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("entered");
        }

        private void tbSnippetEnter(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private void rtbSnipKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                if (!e.Shift)
                { fldMngr.NextField(); }
                else
                { fldMngr.PrevField(); }
            }
        }
    }
}
