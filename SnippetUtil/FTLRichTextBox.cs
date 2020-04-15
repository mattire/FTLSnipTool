using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnippetUtil
{
    class FTLRichTextBox : RichTextBox
    {
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Tab) return true;
            return base.IsInputKey(keyData);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                if (!e.Shift)
                {
                    TabForward?.Invoke(this, e);
                }
                else
                {
                    TabBackward?.Invoke(this, e);
                }

                //const string tabtospaces = "    ";
                //var hassel = this.SelectionLength > 0;
                //this.SelectedText = tabtospaces;
                //if (!hassel) this.SelectionStart += tabtospaces.Length;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if(SuggestUIMngr.Current?.mListBox.Visible == true){
                    SuggestUIMngr.Current.HandleSuggestBoxKeys(null, e);
                } else {
                    EnterPressed?.Invoke(this, e);
                }
                e.SuppressKeyPress = true;
            }
            else
            {
                base.OnKeyDown(e);
                RtbKeyDown?.Invoke(this, e);
            }
        }

        public event EventHandler EnterPressed;
        public event EventHandler TabForward;
        public event EventHandler TabBackward;
        public event EventHandler RtbKeyDown;
    }
}
