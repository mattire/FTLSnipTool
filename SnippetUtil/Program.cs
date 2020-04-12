using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnippetUtil
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            string origTxt = "";
            if (Clipboard.ContainsText()) {
                origTxt = Clipboard.GetText();
            }
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 2) {
                try
                {
                    FtlSnippetForm.StartX = int.Parse(args[0]);
                    FtlSnippetForm.StartY = int.Parse(args[1]);
                }
                catch (Exception)
                {
                }
            }
            var form = new FtlSnippetForm(origTxt);
            Application.Run(form);
        }
    }
}
