namespace SnippetUtil
{
    class Rint
    {
        public int val { get; set; }
        public int lim { get; set; }
        public Rint(int v, int m) { val = v; lim = m; }
        public void Inc() { val++; if (val >= lim) val = 0; }
        public void Dec() { val--; if (val <= lim) val = lim - 1; }
    }
}
