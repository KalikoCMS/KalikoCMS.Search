#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * http://www.gnu.org/licenses/lgpl-3.0.html
 */
#endregion

namespace KalikoCMS.Search {
    using System;
    using System.Collections.ObjectModel;
    using Events;
    using KalikoSearch.Core;

    public class KalikoSearchProvider : SearchProviderBase {
        private readonly Collection _collection;
        private static readonly string[] SearchFields = { "title", "summary", "content", "category", "tags" };

        public KalikoSearchProvider() {
            _collection = new Collection("KalikoCMS");
        }

        public override void AddToIndex(IndexItem item) {
            var key = GetKey(item.PageId, item.LanguageId);
            var pageDocument = new PageDocument(key, item);
            _collection.RemoveDocument(key);
            _collection.AddDocument(pageDocument);

            IndexingFinished();
        }

        public override void RemoveAll() {
            _collection.RemoveAll();
        }

        public override void RemoveFromIndex(Guid pageId, int languageId) {
            var key = GetKey(pageId, languageId);
            _collection.RemoveDocument(key);
        }

        public override void RemoveFromIndex(Collection<Guid> pageIds, int languageId) {
            foreach (var pageId in pageIds) {
                var key = GetKey(pageId, languageId);
                _collection.RemoveDocument(key);
            }
        }

        public override void IndexingFinished() {
            _collection.OptimizeIndex();
        }

        private string GetKey(Guid pageId, int languageId) {
            var key = string.Format("{0}:{1}", pageId, languageId);
            return key;
        }

        public override void Init() {
            PageFactory.PagePublished += OnPagePublished;
        }

        void OnPagePublished(object sender, PageEventArgs e) {
            IndexPage(e.Page);
        }

        public override SearchResult Search(SearchQuery query) {
            var searchResult = _collection.Search(query.SearchString, SearchFields, query.MetaData, query.ReturnFromPosition, query.NumberOfHitsToReturn);
            var result = ConvertResult(searchResult);

            return result;
        }

        public override SearchResult FindSimular(Guid pageId, int languageId, int resultOffset = 0, int resultSize = 10, bool matchCategory = true, string[] metaData = null) {
            var key = GetKey(pageId, languageId);
            var searchResult = _collection.FindSimular(key, resultOffset, resultSize, matchCategory, metaData);
            var result = ConvertResult(searchResult);

            return result;
        }

        private SearchResult ConvertResult(KalikoSearch.Core.SearchResult searchResult) {
            var result = new SearchResult {
                NumberOfHits = searchResult.NumberOfHits, 
                SecondsTaken = searchResult.SecondsTaken
            };

            foreach (var hit in searchResult.Hits) {
                result.Hits.Add(new SearchHit {
                    Excerpt = hit.Excerpt, 
                    Path = hit.Path, 
                    Title = hit.Title, 
                    MetaData = hit.MetaData, 
                    PageId = hit.PageId,
                    Tags = hit.Tags,
                    Summary = hit.Summary
                });
            }

            return result;
        }

    }
}
