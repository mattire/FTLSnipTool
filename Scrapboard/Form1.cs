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
        public Form1()
        {
            InitializeComponent();

            richTextBox1.Text = "Some txt to get \nStarted";

            richTextBox1.KeyDown += RichTextBox1_KeyDown;
        }

        private void RichTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.U)
            {
                var selPos = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart);

                var offset = new Point() { X = 5, Y = 50 };
                var p1 = Utils.Add(this.Location, richTextBox1.Location, selPos, offset);

                SuggestionsBox sb = new SuggestionsBox((s)=> {
                    var pos = richTextBox1.SelectionStart; // caret position??
                    
                    richTextBox1.SelectionStart = pos;
                    richTextBox1.SelectionLength = 0;
                    richTextBox1.SelectedText = s;
                    //richTextBox1.Text.Insert(pos, s);
                }, p1);

                //richTextBox1.Location
                //var p = richTextBox1.Location;
                //System.Diagnostics.Debug.WriteLine(p.X + " " + p.Y);
                //sb.Location = p;
                sb.Show();
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
