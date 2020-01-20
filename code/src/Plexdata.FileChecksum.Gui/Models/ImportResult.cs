/*
* MIT License
* 
* Copyright (c) 2020 plexdata.de
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Plexdata.FileChecksum.Gui.Models
{
    public class ImportResult
    {
        private const String UpdateKey = "update";
        private const String InsertKey = "insert";
        private const String FailedKey = "failed";

        private readonly Dictionary<String, List<ImportItem>> result = null;

        public ImportResult()
        {
            this.result = new Dictionary<String, List<ImportItem>>()
            {
                { ImportResult.UpdateKey, new List<ImportItem>() },
                { ImportResult.InsertKey, new List<ImportItem>() },
                { ImportResult.FailedKey, new List<ImportItem>() }
            };
        }

        public Boolean IsResult
        {
            get
            {
                return this.IsUpdate || this.IsInsert || this.IsFailed;
            }
        }

        public Boolean IsUpdate
        {
            get
            {
                return this.result[ImportResult.UpdateKey].Count > 0;
            }
        }

        public IEnumerable<ImportItem> Update
        {
            get
            {
                return this.result[ImportResult.UpdateKey];
            }
        }

        public Boolean IsInsert
        {
            get
            {
                return this.result[ImportResult.InsertKey].Count > 0;
            }
        }

        public IEnumerable<ImportItem> Insert
        {
            get
            {
                return this.result[ImportResult.InsertKey];
            }
        }

        public Boolean IsFailed
        {
            get
            {
                return this.result[ImportResult.FailedKey].Count > 0;
            }
        }

        public IEnumerable<ImportItem> Failed
        {
            get
            {
                return this.result[ImportResult.FailedKey];
            }
        }

        public void AddUpdate(ImportItem value)
        {
            this.AddValues(ImportResult.UpdateKey, new ImportItem[] { value });
        }

        public void AddUpdate(IEnumerable<ImportItem> values)
        {
            this.AddValues(ImportResult.UpdateKey, values);
        }

        public void AddInsert(ImportItem value)
        {
            this.AddValues(ImportResult.InsertKey, new ImportItem[] { value });
        }

        public void AddInsert(IEnumerable<ImportItem> values)
        {
            this.AddValues(ImportResult.InsertKey, values);
        }

        public void AddFailed(ImportItem value)
        {
            this.AddValues(ImportResult.FailedKey, new ImportItem[] { value });
        }

        public void AddFailed(IEnumerable<ImportItem> values)
        {
            this.AddValues(ImportResult.FailedKey, values);
        }

        public IEnumerable<String> ToUpdate()
        {
            return this.ToDisplay(ImportResult.UpdateKey);
        }

        public IEnumerable<String> ToInsert()
        {
            return this.ToDisplay(ImportResult.InsertKey);
        }

        public IEnumerable<String> ToFailed()
        {
            return this.ToDisplay(ImportResult.FailedKey);
        }

        private void AddValues(String key, IEnumerable<ImportItem> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Any(x => x is null))
            {
                throw new ArgumentOutOfRangeException(nameof(values));
            }

            this.result[key].AddRange(values);
        }

        private IEnumerable<String> ToDisplay(String key)
        {
            return this.result[key]
                .Where(x => !String.IsNullOrWhiteSpace(x.Data))
                .OrderBy(x => x.Line)
                .Select(x => $"{x.Line.ToString().PadLeft(3, '0')}:\t{x.Data}").ToList();
        }
    }
}
