namespace KalikoSearch.Core {
    using System;
    using System.IO;
    using Lucene.Net.Analysis;
    using Lucene.Net.Index;
    using Lucene.Net.Store;

    public class SearchIndexer : IDisposable {
        private readonly FSDirectory _directory;
        private readonly IndexWriter _indexWriter;
        private readonly Analyzer _analyzer;
        private bool _open;

        internal IndexWriter IndexWriter {
            get {
                return _indexWriter;
            }
        }

        public SearchIndexer(FSDirectory directory, Analyzer analyzer) {
            _directory = directory;
            
            EnsureThatIndexIsUnlocked();
            
            _analyzer = analyzer;
            _indexWriter = new IndexWriter(directory, _analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            _open = true;
        }

        public void AddDocument(IndexDocument document) {
            _indexWriter.AddDocument(document.Document);
        }

        public void Close() {
            _analyzer.Close();
            _indexWriter.Dispose();
            _open = false;
        }

        private void EnsureThatIndexIsUnlocked() {
            if (IndexWriter.IsLocked(_directory)) {
                IndexWriter.Unlock(_directory);
            }
            
            DirectoryInfo directoryInfo = _directory.Directory;
            var lockFilePath = Path.Combine(directoryInfo.FullName, "write.lock");

            if (File.Exists(lockFilePath)) {
                File.Delete(lockFilePath);
            }
        }

        public void OptimizeIndex() {
            _indexWriter.Optimize();
            _indexWriter.Commit();
        }

        public void RemoveAll() {
            _indexWriter.DeleteAll();
        }

        public void RemoveDocument(string key) {
            var term  = new Term("key", key);
            _indexWriter.DeleteDocuments(term);
        }

        public void Dispose() {
            if(_open) {
                Close();
            }
        }

    }
}