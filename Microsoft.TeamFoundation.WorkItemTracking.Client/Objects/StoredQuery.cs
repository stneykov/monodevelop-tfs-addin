//
// StoredQuery.cs
//
// Author:
//       Ventsislav Mladenov <vmladenov.mladenov@gmail.com>
//
// Copyright (c) 2013 Ventsislav Mladenov
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using Microsoft.TeamFoundation.WorkItemTracking.Client.Metadata;

namespace Microsoft.TeamFoundation.WorkItemTracking.Client.Objects
{
    public class StoredQuery
    {
        [TableFieldName("ID")]
        public Guid Id { get; set; }

        [TableFieldName("ProjectID")]
        public int ProjectId { get; set; }

        [TableFieldName("fPublic")]
        public bool IsPublic { get; set; }

        [TableFieldName("Owner")]
        public string Owner { get; set; }

        [TableFieldName("QueryName")]
        public string QueryName { get; set; }

        [TableFieldName("QueryText")]
        public string QueryText { get; set; }

        [TableFieldName("Description")]
        public string Description { get; set; }

        [TableFieldName("CreateTime")]
        public DateTime CreateTime { get; set; }

        [TableFieldName("LastWriteTime")]
        public DateTime LastWriteTime { get; set; }

        [TableFieldName("fDeleted")]
        public bool IsDeleted { get; set; }

        [TableFieldName("ParentID")]
        public Guid ParentId { get; set; }
    }
}