using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtlSnippetCreator
{
    class FieldHolder
    {
        private int selStart;
        public string SelTxt { get; set; }
        public string FldTxt { get; set; }

        public FieldHolder(int selStart, string selTxt, string fldTxt)
        {
            this.SelStart = selStart;
            this.SelEnd   = selStart + selTxt.Length;
            this.SelTxt = selTxt;
            this.FldTxt = fldTxt;
        }

        public int SelStart { get => selStart; set => selStart = value; }
        public int SelEnd { get; }

        public int LenDiff => FldTxt.Length - SelTxt.Length;
    }
}
