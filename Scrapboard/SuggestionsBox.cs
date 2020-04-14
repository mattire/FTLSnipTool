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
    public partial class SuggestionsBox : Form
    {
        public static string SelectedText { get; set; }
        public Action<string> DoneAction { get; }
        public Point StartLocation { get; }

        public SuggestionsBox(Action<string> doneAction, Point location)
        {
            InitializeComponent();
            listBox1.Items.Add("aa");
            listBox1.Items.Add("ab");
            listBox1.Items.Add("ac");
            listBox1.KeyDown += ListBox1_KeyDown;
            DoneAction = doneAction;
            StartLocation = location;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Location = StartLocation;
        }

        private void ListBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (listBox1.SelectedItem != null) {
                    var sel = listBox1.SelectedItem.ToString();
                    System.Diagnostics.Debug.WriteLine(sel);
                    SelectedText = sel;
                    this.Close();
                    DoneAction(sel);
                }
            }
        }
    }
}
