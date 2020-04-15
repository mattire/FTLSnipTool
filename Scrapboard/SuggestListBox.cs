using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrapboard
{
    class SuggestListBox : ListBox
    {
        public Action<string> DoneAction { get; set; }
        //public SuggestListBox(List<string> items)
        //{
        //    items.ForEach((s) => { Items.Add(s); });
        //}

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                this.Visible = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (this.SelectedItem != null)
                {
                    var sel = SelectedItem.ToString();
                    System.Diagnostics.Debug.WriteLine(sel);
                    DoneAction(sel);
                }
                this.Visible = false;
            }

        }
    }
}
