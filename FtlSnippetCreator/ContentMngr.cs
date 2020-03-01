using FtlcCommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtlSnippetCreator
{
    class ContentMngr
    {
        private IEnumerable<Match> mMatchLst;
        private FieldManager mFldMngr;
        public ContentMngr(){}

        public void Process(string content/*, RichTextBox richTextBox*/)
        {
            Regex rx = new Regex(Constants.FieldPattern);
            mMatchLst = rx.Matches(content).Cast<Match>();
            //mFldMngr = new FieldManager();
            //mFldMngr.MRichTextBox = richTextBox;
            //mFldMngr.UpdateContents(content);
            //mMatchCollection.Cast<Match>().
            //var lines = content.Split(new string[] { "\n\r", "\r\n", "\n" }, StringSplitOptions.None);

            //foreach (var l in lines)
            //{
            //    var mc = rx.Matches(l);
            //    mc.Cast<Match>().Select()
            //}
            //int i = 0;
            //i++;
            Func<Match, (int, string)> LocationAndStr = (m) => {
                var c = m.Captures.Cast<Capture>().First();
                return (c.Index, c.Value);
            };

            var locsStrs = mMatchLst.Select(m => LocationAndStr(m));

            foreach (var ls in locsStrs)
            {
                System.Diagnostics.Debug.WriteLine(ls.Item1 + " " + ls.Item2);
            }

            //var fh = new FtlcCommonLib.FieldManager.FieldHolder()

        }
    }
}
