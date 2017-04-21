namespace KalikoSearch.Core {
    using System;
    using System.IO;
    using Lucene.Net.Analysis;
    using Lucene.Net.Index;
    using Lucene.Net.Store;

    public class SearchIndexer : IDisposable {
        private readonly FSDirectory _directory;
        private readonly Analyzer _analyzer;

        public SearchIndexer(FSDirectory directory, Analyzer analyzer) {
            _directory = directory;
            _analyzer = analyzer;
        }

        internal IndexWriter GetIndexWriter() {
            EnsureThatIndexIsUnlocked();
            return new IndexWriter(_directory, _analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
        }

        public void AddDocument(IndexDocument document) {
            using (var indexWriter = GetIndexWriter()) { 
                indexWriter.AddDocument(document.Document);
            }
        }

        public void Close() {
            _analyzer.Close();
        }

        private void EnsureThatIndexIsUnlocked() {
            if (IndexWriter.IsLocked(_directory)) {
                IndexWriter.Unlock(_directory);
            }
            
            var directoryInfo = _directory.Directory;
            var lockFilePath = Path.Combine(directoryInfo.FullName, "write.lock");

            if (File.Exists(lockFilePath)) {
                File.Delete(lockFilePath);
            }
        }

        public void OptimizeIndex() {
            using (var indexWriter = GetIndexWriter()) {
                indexWriter.Optimize();
                indexWriter.Commit();
            }
        }

        public void RemoveAll() {
            using (var indexWriter = GetIndexWriter()) {
                indexWriter.DeleteAll();
            }
        }

        public void RemoveDocument(string key) {
            var term  = new Term("key", key);
            using (var indexWriter = GetIndexWriter()) {
                indexWriter.DeleteDocuments(term);
            }
        }

        public void Dispose() {
            Close();
        }
    }
}