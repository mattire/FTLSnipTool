using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapboard
{

    // Need inherit and rewrite Richtextbox to work
    public class TabHandler
    {
        public Rewrite mRewrite { get; }

        private int mTabInd;

        public TabHandler(Rewrite rewrite)
        {
            mRewrite = rewrite;
            mTabInd = 0;
        }

        public void SelectCurrent() {
            var fld = mRewrite.Fields[mTabInd];
            var fldPlace = mRewrite.FieldPlaces.FirstOrDefault(fp => fp.FldName == fld.Name);
            mRewrite.RichTextBox.Select(fldPlace.FldOuterStart, fldPlace.OutLength);
        }

        public void SelectFirst() {
            mTabInd = 0;
            SelectCurrent();
        }

        public void Next()
        {
            if (mTabInd < mRewrite.Fields.Count)
            {
                mTabInd++;
            }
            else {
                mTabInd = 0;
            }
            SelectCurrent();
        }
    }
}
