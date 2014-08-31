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

namespace KalikoSearch.Configuration {
    using System;
    using System.Configuration;
    using Version = Lucene.Net.Util.Version;

    public class SearchSettings : ConfigurationSection {
        private static SearchSettings _instance;

        public static SearchSettings Instance {
            get {
                return _instance ?? (_instance = ConfigurationManager.GetSection("searchSettings") as SearchSettings);
            }
        }

        [ConfigurationProperty("datastorePath", IsRequired = true)]
        public string DataStorePath {
            get {
                return ((string)base["datastorePath"]).Replace("|DataDirectory|", (string)AppDomain.CurrentDomain.GetData("DataDirectory"));
            }
        }

        [ConfigurationProperty("analyzer", IsRequired = false, DefaultValue = "KalikoSearch.Analyzers.StandardAnalyzer, KalikoSearch")]
        public string Analyzer {
            get { 
                return (string)base["analyzer"]; 
            }
        }

        [ConfigurationProperty("luceneVersion", IsRequired = false, DefaultValue = Version.LUCENE_30)]
        public Version LuceneVersion {
            get { return (Version)base["luceneVersion"]; }
        }

        [ConfigurationProperty("language", IsRequired = false, DefaultValue = "English")]
        public string Language {
            get { return (string)base["language"]; }
        }
    }
}