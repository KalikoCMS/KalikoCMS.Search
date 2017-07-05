namespace KalikoSearch.Core {
    using System;
    using System.IO;
    using System.Web;
    using Configuration;
    using Lucene.Net.Store;

    public class Collection : IDisposable {
        private readonly string _collectionName;
        private readonly SearchFinder _searchFinder;
        private readonly SearchIndexer _searchIndexer;

        // TODO: Overload to allow usage of different parameters than stored in the configuration.
        public Collection(string collectionName) {
            _collectionName = collectionName;
            var directory = GetDataDirectory();
            var analyzer = SearchAnalyzer.GetConfiguredAnalyzer();
            _searchIndexer = new SearchIndexer(directory, analyzer);
            _searchFinder = new SearchFinder(directory, analyzer);
        }

        public void AddDocument(IndexDocument document) {
            _searchIndexer.AddDocument(document);
            _searchFinder.Refresh();
        }

        public void RemoveDocument(string key) {
            _searchIndexer.RemoveDocument(key);
            _searchFinder.Refresh();
        }

        public void RemoveAll() {
            _searchIndexer.RemoveAll();
            _searchFinder.Refresh();
        }

        public void OptimizeIndex() {
            _searchIndexer.OptimizeIndex();
            _searchFinder.Refresh();
        }

        public SearchResult Search(string queryString, string[] searchFields, string[] metaData, int resultOffset, int resultLength) {
            return _searchFinder.Search(queryString, searchFields, metaData, resultOffset, resultLength);
        }

        public SearchResult FindSimular(string key, int resultOffset = 0, int resultLength = 10, bool matchCategory = true, string[] metaData = null) {
            return _searchFinder.FindSimular(key, resultOffset, resultLength, matchCategory, metaData);
        }

        private FSDirectory GetDataDirectory() {
            return FSDirectory.Open(new DirectoryInfo(DataDirectory));
        }

        private string DataDirectory {
            get {
                string dataStorePath = SearchSettings.Instance.DataStorePath;
                if (!dataStorePath.EndsWith("/")) {
                    dataStorePath += "/";
                }

                if (dataStorePath.StartsWith("/")) {
                    return HttpContext.Current.Server.MapPath(dataStorePath + _collectionName + "/");
                }
                else {
                    return dataStorePath + _collectionName + "/";
                }
            }
        }

        public void Dispose() {
            _searchIndexer.Close();
        }
    }
}