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

using Plexdata.FileChecksum.Gui.Models;
using System;
using System.Collections.Generic;

namespace Plexdata.FileChecksum.Gui.Helpers
{
    internal static class ImportParser
    {
        public static IEnumerable<ImportItem> Parse(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), "No content to parse as import data.");
            }

            Int32 line = 0;

            foreach (String data in value.Split(new String[] { Environment.NewLine }, StringSplitOptions.None))
            {
                yield return ImportParser.ParseLine(++line, data);
            }
        }

        private static ImportItem ParseLine(Int32 line, String data)
        {
            ImportItem result = new ImportItem()
            {
                Line = line,
                Data = data
            };

            Int32 offset = ImportParser.IndexOfWhiteSpace(data);

            if (offset >= 0 && offset < data.Length)
            {
                result.Method = data.Substring(0, offset).Trim().Trim('"');
                data = data.Substring(offset).TrimStart();

                offset = ImportParser.IndexOfWhiteSpace(data);

                if (offset >= 0 && offset < data.Length)
                {
                    result.Checksum = data.Substring(0, offset).Trim().Trim('"');
                    result.FilePath = data.Substring(offset).Trim().Trim('"');
                }
            }

            return result;
        }

        private static Int32 IndexOfWhiteSpace(String value)
        {
            for (Int32 index = 0; index < value.Length; index++)
            {
                if (Char.IsWhiteSpace(value[index]))
                {
                    return index;
                }
            }

            return -1;
        }
    }
}
