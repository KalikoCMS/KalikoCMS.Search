namespace KalikoSearch.Core {
    using System;
    using System.Collections.Generic;

    public class SearchHit {
        public SearchHit() {
            MetaData = new Dictionary<string, string>();
        }

        public Guid PageId { get; internal set; }
        public string Title { get; internal set; }
        public string Path { get; internal set; }
        public string Excerpt { get; internal set; }
        public string Summary { get; set; }
        public string Tags { get; set; }
        public Dictionary<string, string> MetaData { get; internal set; }
    }
}