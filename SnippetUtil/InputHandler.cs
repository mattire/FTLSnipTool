using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SnippetUtil
{
    // check if back space => increment or decrement fld len
    // then on session end copu from rich txt box
    //mRichTextBox.Text.Substring

    class InputHandler
    {
        private int len = 0;
        private FieldManager mFldMngr;
        public bool Changed { get; set; } = false;
        public int Len { get => len; set => len = value; }
        public int FldLen { get => len -1; }

        //public event EventHandler SessionEnd;
        public InputHandler(FieldManager fieldManager)
        {
            mFldMngr = fieldManager;
        }
        public void Handle(KeyEventArgs kea)
        {
            Changed = true;
            var kd = kea.KeyData;
            var actionKeys = new List<Keys>() {
                Keys.Tab, Keys.Enter, Keys.Control, Keys.Alt
            };
            
            if (kd == Keys.Back)
            {
                if (Len != 0) { Len--; }
            }
            else {
                if (!kea.Alt && !actionKeys.Contains(kd))
                {
                    Len++;
                }
                else {
                    //mFldMngr.SessionEnd();
                }
            }
        }
    }
}
