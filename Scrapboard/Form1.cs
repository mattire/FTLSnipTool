using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrapboard
{
    public partial class Form1 : Form
    {
        public static RichTextBox RichTextBox1;
        
        public Form1()
        {
            InitializeComponent();

            richTextBox1.Text = "Some txt to get \nStarted";
            RichTextBox1 = richTextBox1;
            //listBox1.Visible = false;
            //new List<string>() { "ad", "afaw", "afaew" }.ForEach((s) => { listBox1.Items.Add(s); });
            suggestListBox1.Visible = false;
            new List<string>() { "ad", "afaw", "afaew" }.ForEach((s) => { suggestListBox1.Items.Add(s); });

            richTextBox1.KeyDown += RichTextBox1_KeyDown;
        }

        private void RichTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.U)
            {
                var selPos = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart);

                var offset = new Point() { X = 5, Y = 50 };
                var p1 = Utils.Add(this.Location, richTextBox1.Location, selPos, offset);

                suggestListBox1.Location = p1;
                
                suggestListBox1.DoneAction = (s) => {
                    var pos = richTextBox1.SelectionStart; // caret position??

                    richTextBox1.SelectionStart = pos;
                    richTextBox1.SelectionLength = 0;
                    richTextBox1.SelectedText = s;
                    //richTextBox1.Text.Insert(pos, s);
                };
                suggestListBox1.Visible = true;

                //SuggestionsBox sb = new SuggestionsBox((s)=> {
                //    var pos = richTextBox1.SelectionStart; // caret position??
                //    
                //    richTextBox1.SelectionStart = pos;
                //    richTextBox1.SelectionLength = 0;
                //    richTextBox1.SelectedText = s;
                //    //richTextBox1.Text.Insert(pos, s);
                //}, p1);
                //sb.Show();

                
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.U) {
                //SuggestionsBox sb = new SuggestionsBox();
                //sb.Show();
            }
            base.OnKeyDown(e);
        }
    }
}
