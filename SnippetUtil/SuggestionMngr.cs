using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SnippetUtil
{
    class SuggestionMngr
    {
        private readonly string snipPath;
        private MatchCollection mMatchCollection;
        private string mDir;
        private Dictionary<string, List<string>> mSuggestionMap;
        private List<string> mCurrentFldNames;

        public SuggestionMngr(string snipPath)
        {
            this.snipPath = snipPath;
            EnsureDir();
        }

        private void EnsureDir() {
            var dn = Path.GetDirectoryName(snipPath);
            var fnwoe = Path.GetFileNameWithoutExtension(snipPath);
            var dir = Path.Combine(dn, fnwoe);
            mDir = dir;
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
        }

        public void SetOrigFlds(MatchCollection matchCollection)
        {
            mMatchCollection = matchCollection;
            mMatchCollection.Cast<Match>().ToList().ForEach((m) => {
                System.Diagnostics.Debug.WriteLine(m.Name);
                System.Diagnostics.Debug.WriteLine(m.Value);
                EnsureFile(m.Value);
            });
            LoadPreviousEntries();
        }

        private void EnsureFile(string value)
        {
            var fileName = value.Substring(
                FieldManager.MStartStr.Length,
                value.Length - (FieldManager.MStartStr.Length + FieldManager.MEndStr.Length));
            var filePath= Path.Combine(mDir, fileName);
            if (!File.Exists(filePath)) {
                File.Create(filePath);
            }
        }

        object loadLock = new object();
        private void LoadPreviousEntries() {
            lock (loadLock)
            {
                var fls = Directory.GetFileSystemEntries(mDir);
                mSuggestionMap = fls.ToDictionary(
                    (fpath) => Path.GetFileName(fpath),
                    (fpath) => File.ReadAllLines(fpath).ToList()
                ); 
            }
        }

        public List<string> GetSuggestions(int fldInd) {
            if (fldInd >= mSuggestionMap.Count) { return new List<string>(); } // no params/suggestions
            return mSuggestionMap.ElementAt(fldInd).Value;
        }

        public void AddSuggestion(int fldInd, string input) {
            var suggestions = mSuggestionMap.ElementAt(fldInd).Value;
            if (suggestions.Contains(input)) {
                var ind = suggestions.IndexOf(input);
                suggestions.RemoveAt(ind);
            }
            suggestions.Insert(0, input);
            if (suggestions.Count > 5)
            {
                suggestions.RemoveAt(5);
            }
        }

        public void UpdCurrentNamesMap(MatchCollection mMatchCollection)
        {
            mCurrentFldNames = mMatchCollection.Cast<Match>().Select(m => StripStartEndTags(m.Value)).ToList();
        }

        private string StripStartEndTags(string value) {
            return value.Substring(
                FieldManager.MStartStr.Length,
                value.Length - (FieldManager.MStartStr.Length + FieldManager.MEndStr.Length));
        }

        public void Save() {
            foreach (var key in mSuggestionMap.Keys)
            {
                var vals = mSuggestionMap[key];
                var content = string.Join("\n", vals);
                var path = Path.Combine(mDir, key);
                File.WriteAllText(path, content);
            }
        }
    }
}
