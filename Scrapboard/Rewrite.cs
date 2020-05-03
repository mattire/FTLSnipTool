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
            public string FldValue { get; set; }

            public int Order { get; set; }
        }

        public class OutPlace {
            public int Start { get; set; }
            public int End   { get; set; }
            public int OutputStart { get; set; }
            public int OutputEnd   { get; set; }
            public int Order { get; set; }
            public OutPlace(int start, int end, int order)
            {
                Start = start;
                End   = end;
                Order = order;
                var diff = order * (FieldPlace.StartTag.Length + FieldPlace.EndTag.Length);
                OutputStart = start + diff;
                OutputEnd = end + diff;
            }
        }

        public List<FieldPlace> FieldPlaces { get; set; }
        public List<OutPlace> OutPlaces { get; set; } = new List<OutPlace>();


        public List<Field> FieldClasses { get; set; }
        public string FldText { get; set; }
        public string OutputText { get; set; }

        public List<string> PartsBetween { get; set; } = new List<string>();

        public Rewrite()
        {
            InitializeComponent();
            //FldText = Clipboard.GetText();
            //richTextBox2.KeyDown
            //richTextBox2.PreviewKeyDown
            richTextBox2.KeyDown += RichTextBox2_KeyDown;
            ProcessText();
        }

        private void RichTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            System.Windows.Input.KeyConverter keyConverter = new System.Windows.Input.KeyConverter();
            var str = keyConverter.ConvertToString(e.KeyCode);
            int i = richTextBox1.SelectionStart;

            var fp = InsideFieldPlace(i);
            var outFp = InsideOutFieldPlace(i);
        }

        public FieldPlace InsideFieldPlace(int pos) {
            var fld1 = FieldPlaces.FirstOrDefault(fld => fld.OutPutTextStart <= pos && pos <= fld.OutPutTextEnd);
            return fld1;
        }

        public FieldPlace InsideOutFieldPlace(int pos)
        {
            //var outFld = OutFieldPlaces.FirstOrDefault(fld => fld.OutPutTextStart <= pos && pos <= fld.OutPutTextEnd);
            //return outFld;
            return null;
        }


        public void ProcessText()
        {
            FldText = "<#*Element*#>\n#*SurroundContent*#\n</#*Element*#>";

            richTextBox1.Text = FldText;
            var starts = FldText.AllIndexesOf(FieldPlace.StartTag);
            var stops  = FldText.AllIndexesOf(FieldPlace.EndTag);

            FieldPlaces = starts.Select((val, ind) => new { start = val, stop = stops[ind], order = ind }).Select(t => new FieldPlace(t.start, t.stop, t.order, FldText)).ToList();

            //var outPlacesPoints = new List<int>() { 0 };
            //FieldPlaces.ForEach(fp => { outPlacesPoints.Add(fp.FldOuterStart); outPlacesPoints.Add(fp.FldOuterEnd); );
            //FieldPlaces.First().
            //OutPlaces.Add(new OutPlace(0, starts.First(), 0));

            FieldClasses = FieldPlaces.GroupBy(fld => fld.FldName).Select(g => g.First()).Select(fld => new Field() { Name = fld.FldName, Value = fld.FldName }).ToList();

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

        private void ApplyChange() {

        }
        
    }
}
 