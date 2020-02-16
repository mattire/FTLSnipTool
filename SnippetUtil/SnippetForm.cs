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

        public FtlSnippetForm()
        {
            InitializeComponent();
            RefreshSnippets();
            //txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSearchKeyPress);
            txtSearch.KeyDown += TxtSearch_KeyDown;
            //txtSnippet.Select
            fldMngr.MRichTextBox = richTextBoxSnippet;
            ((FTLRichTextBox)richTextBoxSnippet).EnterPressed += btnOk_Click;
            txtSearch.Focus();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.KeyCode);
            if (e.KeyCode == Keys.Enter)
            {
                var fn = txtSearch.Text + ".txt";
                var path = Path.Combine(SnippetFld, fn);
                var contents = File.ReadAllText(path);
                
                txtSnippet.Text = contents;

                fldMngr.UpdateContents(contents);
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
            this.Dispose();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        
        private void btnCancel_Click(object sender, EventArgs e)
        {

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
