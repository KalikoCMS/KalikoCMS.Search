namespace KalikoSearch.Core {
    using System.Collections.Generic;

    public class SearchResult {
        public SearchResult() {
            Hits = new List<SearchHit>();
        }

        public int NumberOfHits { get; internal set; }
        public double SecondsTaken { get; internal set; }
        public List<SearchHit> Hits { get; private set; }
    }
}