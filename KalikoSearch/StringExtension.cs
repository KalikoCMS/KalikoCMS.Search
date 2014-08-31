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

namespace KalikoSearch {
    using System;
    using System.Text.RegularExpressions;

    public static class StringExtension {
        private static readonly Regex GuidMatchPattern = new Regex(
        "^[A-Fa-f0-9]{32}$|" +
        "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
        "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");


        /* Based on code from http://geekswithblogs.net/colinbo/archive/2006/01/18/66307.aspx */
        public static bool TryParseGuid(this string value, out Guid result) {
            if (!string.IsNullOrEmpty(value) && GuidMatchPattern.IsMatch(value)) {
                result = new Guid(value);
                return true;
            }
            else {
                result = Guid.Empty;
                return false;
            }
        }
    }
}