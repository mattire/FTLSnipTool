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
    public partial class Rewrite : Form
    {
        //https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.previewkeydown?view=netcore-3.1

        public class Field {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public class FieldPlace
        {
            public FieldPlace(int occurrenceStart, int occurrenceEnd, int order, string fldText)
            {
                FldOuterStart = occurrenceStart;
                FldOuterEnd = occurrenceEnd + EndTag.Length;
                FldInnerStart = occurrenceStart + StartTag.Length;
                FldInnerEnd = occurrenceEnd;
                Order = order;
                OutPutTextStart = FldOuterStart - Order * (StartTag.Length + EndTag.Length);
                OutPutTextEnd = FldOuterEnd - (Order + 1) * (StartTag.Length + EndTag.Length);
                FldName = fldText.Substring(FldInnerStart, FldInnerEnd - FldInnerStart);
                // initial value is fld name
                FldValue = FldName;
            }

            public static string StartTag { get; set; } = "#*";
            public static string EndTag   { get; set; } = "*#";

            public int FldOuterStart { get; set; }
            public int FldOuterEnd { get; set; }

            public int FldInnerStart { get; set; }
            public int FldInnerEnd   { get; set; }

            public int OutPutTextStart { get; set; }
            public int OutPutTextEnd { get; set; }

            public string FldName { get; set; }
            
            public string fldValue { get; set; }
            public string FldValue
            {
                get { return fldValue; }
                set { fldValue = value; }
            }
            public string TagName { get { return StartTag + FldName + EndTag; }  }
            public string TagValue { get { return StartTag + FldValue + EndTag; } }


            public int Order { get; set; }

        }

        public class OutPlace {
            public int Start { get; set; }
            public int End   { get; set; }
            public int OutputStart { get; set; }
            public int OutputEnd   { get; set; }
            public int Order { get; set; }
            public string Value { get; set; }
            public OutPlace(int start, int end, int order, string txt)
            {
                Start = start;
                End   = end;
                Order = order;
                var diff = order * (FieldPlace.StartTag.Length + FieldPlace.EndTag.Length);
                OutputStart = start + diff;
                OutputEnd = end + diff;
                Value = txt.SubWithStartEndPoints(Start, End);
            }
            //public string GetOutTxt(string txt) {
            //    return txt.SubWithStartEndPoints(OutputStart, OutputEnd);
            //}
            public string GetOrigTxt(string txt)
            {
                return txt.SubWithStartEndPoints(Start, End);
            }

        }

        public List<FieldPlace> FieldPlaces      { get; set; }
        public List<OutPlace>   OutsideFldPlaces { get; set; } = new List<OutPlace>();


        public List<Field> Fields { get; set; }
        public string FldText { get; set; }
        public string OutputText { get; set; }

        public List<string> PartsBetween { get; set; } = new List<string>();

        public Rewrite()
        {
            InitializeComponent();
            //FldText = Clipboard.GetText();
            //richTextBox2.KeyDown
            //richTextBox2.PreviewKeyDown
            //richTextBox2.KeyDown += RichTextBox2_KeyDown;
            richTextBox2.KeyPress += RichTextBox2_KeyPress;
            richTextBox2.SelectionChanged += RichTextBox2_SelectionChanged;

            FldText = "<#*Element*#>\n#*SurroundContent*#\n</#*Element*#>\n<#*Element*#>\n#*SurroundContent*#\n</#*Element*#>";
            ProcessText();
        }

        private void RichTextBox2_SelectionChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.GetType().Name);
            System.Diagnostics.Debug.WriteLine(e.ToString());
            System.Diagnostics.Debug.WriteLine(richTextBox2.SelectionLength);
        }

        private void RichTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetterOrDigit(e.KeyChar) || e.KeyChar== '\b') {
                e.Handled = true;
            }
            int selStart;
            if (e.KeyChar == '\b') { selStart = richTextBox2.SelectionStart + 1; }
            else { selStart = richTextBox2.SelectionStart; }
            //int selStart = richTextBox2.SelectionStart;
            int selLen = richTextBox2.SelectionLength;

            FieldPlace fp;
            int fieldsInd;
            int fieldInd;
            if (selLen == 0)
            {
                //var fp = InsideFieldPlace(selStart);
                InsideFieldPlace2(selStart, out fp, out fieldInd, out fieldsInd);
                var outFp = InsideOutFieldPlace(selStart);
                if (fp != null)
                {
                    Process(fp, selStart, e.KeyChar, richTextBox2.Text);
                    RewriteFieldPlaces();
                    //string newOrigTxt;
                    //string newOutTxt;
                    //AssembleText(out newOrigTxt, out newOutTxt);
                    //richTextBox1.Text = newOrigTxt;
                    //richTextBox2.Text = newOutTxt;
                    //FldText = newOrigTxt;
                    //ProcessText();

                    if (e.KeyChar != '\b')
                    {
                        richTextBox2.SelectionStart = selStart + fieldInd + 1;
                    }
                    else
                    { richTextBox2.SelectionStart = selStart - 1 - fieldInd; }
                }
                if (outFp != null)
                {
                }
            }
            else // Selection Length > 0
            {
                SelectionInsideFieldPlace(selStart, selLen, out fp, out fieldInd, out fieldsInd);
                if (fp != null)
                {
                    Process(fp, selStart, selLen, e.KeyChar, richTextBox2.Text);
                    RewriteFieldPlaces();
                    if (e.KeyChar != '\b')
                    {
                        richTextBox2.SelectionStart = selStart + fieldInd + 1;
                    }
                    else
                    { richTextBox2.SelectionStart = selStart - fieldInd * selLen; }
                }
            }
                //throw new NotImplementedException();
        }

        private void RewriteFieldPlaces() {
            string newOrigTxt;
            string newOutTxt;
            AssembleText(out newOrigTxt, out newOutTxt);
            richTextBox1.Text = newOrigTxt;
            richTextBox2.Text = newOutTxt;
            FldText = newOrigTxt;
            ProcessText();
        }

        private int GetFieldPlaceCount(FieldPlace fp)
        {
            return FieldPlaces.Where(fplace => fplace.FldName == fp.FldName).Count();
        }

        private void Process(FieldPlace fp, int selStart, int selLen, char keyChar, string text)
        {
            int fldPos = selStart - fp.OutPutTextStart;
            string value = (string)fp.FldValue.Clone();
            value = value.Remove(fldPos, selLen);
            if (keyChar == '\b') { } // nothing to do
            else {
                value.Insert( fldPos, keyChar.ToString());
            }
            var fld = Fields.FirstOrDefault(f => f.Name == fp.FldName);
            fld.Value = value;
            FieldPlaces.Where(fpl => fpl.FldName == fld.Name).ToList().ForEach(fpl => fpl.FldValue = value);
        }

        private void Process(FieldPlace fp, int selStart, char keyChar, string text)
        {
            int fldPos = selStart - fp.OutPutTextStart;
            if (keyChar == '\b' && fldPos == 0) { return; }

            string value = (string)fp.FldValue.Clone();
            if (keyChar != '\b')
            {
                value = value.Insert(fldPos, keyChar.ToString());
            }
            else {
                value = value.Remove(fldPos -1, 1);
            }
            var fld = Fields.FirstOrDefault(f => f.Name == fp.FldName);
            fld.Value = value;
            // Write to all FieldPlaces
            FieldPlaces.Where(fpl => fpl.FldName == fld.Name).ToList().ForEach(fpl => fpl.FldValue = value);
        }

        private void RichTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            System.Windows.Input.KeyConverter keyConverter = new System.Windows.Input.KeyConverter();
            var str = keyConverter.ConvertToString(e.KeyCode);

            if (!IsNavigationEA(e)) {
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Back) {
                e.Handled = false;
            }
        }

        List<Keys> navKeys = new List<Keys>() { Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.PageUp, Keys.Tab, Keys.RButton, Keys.LButton, Keys.Escape, Keys.Return };
        private bool IsNavigationEA(KeyEventArgs e)
        {
            return navKeys.Contains(e.KeyCode);
        }

        //public FieldPlace InsideFieldPlace(int pos) {
        //    return FieldPlaces.FirstOrDefault(fld => fld.OutPutTextStart <= pos && pos <= fld.OutPutTextEnd);
        //}

        public void InsideFieldPlace2(int pos, out FieldPlace fp, out int fldInd, out int fldsInd)
        {
            fp = FieldPlaces.FirstOrDefault(fp1 => fp1.OutPutTextStart <= pos && pos <= fp1.OutPutTextEnd);
            if (fp == null) { fldInd = -1;fldsInd = -1; return; }
            fldsInd = FieldPlaces.IndexOf(fp);
            var fn = fp.FldName;
            fldInd = FieldPlaces.Where(fp1 => fp1.FldName == fn).ToList().IndexOf(fp);
        }

        public void SelectionInsideFieldPlace(int start, int selLength, out FieldPlace fp, out int fldInd, out int fldsInd)
        {
            // check start and end points are both inside fieldplace
            int end = start + selLength;

            FieldPlace fp1; int fldInd1; int fldsInd1;
            FieldPlace fp2; int fldInd2; int fldsInd2;

            InsideFieldPlace2(start, out fp1, out fldInd1, out fldsInd1);
            InsideFieldPlace2(start, out fp2, out fldInd2, out fldsInd2);

            if (Object.ReferenceEquals(fp1, fp2))
            {
                fp = fp1;
                fldInd = fldInd1;
                fldsInd = fldsInd1;
            } else {
                fp = null;
                fldInd  = -1;
                fldsInd = -1;
            }
        }


        public OutPlace InsideOutFieldPlace(int pos)
        {
            return this.OutsideFldPlaces.FirstOrDefault(pl => pl.OutputStart <= pos && pos <= pl.OutputEnd);
        }


        public void ProcessText()
        {
            OutsideFldPlaces = new List<OutPlace>();
            FieldPlaces = new List<FieldPlace>();
            Fields = new List<Field>();
            PartsBetween = new List<string>();

            richTextBox1.Text = FldText;
            var starts = FldText.AllIndexesOf(FieldPlace.StartTag);
            var stops  = FldText.AllIndexesOf(FieldPlace.EndTag);

            FieldPlaces = starts.Select((val, ind) => new { start = val, stop = stops[ind], order = ind }).Select(t => new FieldPlace(t.start, t.stop, t.order, FldText)).ToList();

            var outPlacesPoints = new List<int>() { 0 };
            FieldPlaces.ForEach(fp => { outPlacesPoints.Add(fp.FldOuterStart); outPlacesPoints.Add(fp.FldOuterEnd); });
            outPlacesPoints.Add(FldText.Length);
            for (int i = 0; i < outPlacesPoints.Count; i+=2)
            {
                var op = new OutPlace(outPlacesPoints[i], outPlacesPoints[i + 1], i / 2, FldText);
                System.Diagnostics.Debug.WriteLine(op.Value);
                OutsideFldPlaces.Add(op);
            }

            Fields = FieldPlaces.GroupBy(fld => fld.FldName).Select(g => g.First()).Select(fld => new Field() { Name = fld.FldName, Value = fld.FldName }).ToList();

            List<int> OutPutPoints = new List<int>() { 0 };
            FieldPlaces.ForEach(fld => { OutPutPoints.Add(fld.FldOuterStart); OutPutPoints.Add(fld.FldOuterEnd); });
            OutPutPoints.Add(FldText.Length);

            for (int i = 0; i < OutPutPoints.Count; i+=2)
            {
                var start = OutPutPoints.ElementAt(i);
                var end = OutPutPoints.ElementAt(i+1);
                PartsBetween.Add(FldText.SubWithStartEndPoints(start, end));
                //OutFieldPlaces.Add()
            }
            //OutPutPoints.Aggregate((p1, p2) => { PartsBetween.Add(FldText.SubWithStartEndPoints(p1, p2)); return p2; });

            StringBuilder sb = new StringBuilder();
            int ind1 = 0;
            foreach (var item in PartsBetween)
            {
                sb.Append(item);
                if (ind1 < FieldPlaces.Count) {
                    sb.Append(FieldPlaces.ElementAt(ind1).FldName);
                }
                ind1++;
            }
            OutputText = sb.ToString();
            richTextBox2.Text = OutputText;
        }

        private void AssembleText(out string newOrigTxt, out string newOutTxt)
        {
            var outTxt = richTextBox2.Text;
            var origTxt = richTextBox1.Text;

            StringBuilder outSb = new StringBuilder();
            StringBuilder origSb = new StringBuilder();

            for (int i = 0; i < FieldPlaces.Count; i++)
            {
                //outSb.Append( OutsideFldPlaces.ElementAt(i).GetOutTxt(outTxt));
                outSb.Append(OutsideFldPlaces.ElementAt(i).Value);
                outSb.Append(FieldPlaces.ElementAt(i).FldValue);

                //origSb.Append(OutsideFldPlaces.ElementAt(i).GetOutTxt(origTxt));
                origSb.Append(OutsideFldPlaces.ElementAt(i).Value);
                origSb.Append(FieldPlaces.ElementAt(i).TagValue);
            }
            var lstPiece = OutsideFldPlaces.Last();
            //outSb.Append(lstPiece.GetOutTxt(outTxt));
            outSb.Append(lstPiece.Value);
            //origSb.Append(lstPiece.GetOutTxt(origTxt));
            origSb.Append(lstPiece.Value);
            newOrigTxt = origSb.ToString();
            newOutTxt = outSb.ToString();
        }
        
    }
}
 