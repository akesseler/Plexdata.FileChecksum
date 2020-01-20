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

using Plexdata.FileChecksum.Constants;
using Plexdata.FileChecksum.Gui.Constants;
using Plexdata.Formatters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Plexdata.FileChecksum.Gui.Models
{
    public class FileItem
    {
        public FileItem(String fullpath)
        {
            if (String.IsNullOrWhiteSpace(fullpath))
            {
                throw new ArgumentOutOfRangeException(nameof(fullpath));
            }

            this.Name = System.IO.Path.GetFileName(fullpath);
            this.Path = System.IO.Path.GetDirectoryName(fullpath);

            FileInfo info = new FileInfo(fullpath);

            this.Created = info.CreationTimeUtc;
            this.Changed = info.LastWriteTimeUtc;
            this.Accessed = info.LastAccessTimeUtc;

            this.Date = info.LastWriteTimeUtc;
            this.Size = info.Length;

            this.Attributes = info.Attributes;

            List<Checksum> checksums = new List<Checksum>();

            foreach (Method method in Enum.GetValues(typeof(Method)))
            {
                checksums.Add(new Checksum(method));
            }

            this.Checksums = checksums;

            this.Tooltip = this.GetTooltip(Thread.CurrentThread.CurrentUICulture);
        }

        public Checksum this[Method method]
        {
            get
            {
                return this.Checksums.Where(x => x.Method == method).FirstOrDefault();
            }
        }

        public Checksum this[String method]
        {
            get
            {
                return this.Checksums.Where(x => x.Method.ToString().Equals(method, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            }
        }

        public String Name { get; private set; }

        public String Path { get; private set; }

        public String FullPath
        {
            get
            {
                return System.IO.Path.Combine(this.Path, this.Name);
            }
        }

        public DateTime Created { get; private set; }

        public DateTime Changed { get; private set; }

        public DateTime Accessed { get; private set; }

        public DateTime Date { get; private set; }

        public Int64 Size { get; private set; }

        public FileAttributes Attributes { get; private set; }

        public IReadOnlyList<Checksum> Checksums { get; private set; }

        public Boolean CanCreate
        {
            get
            {
                return true;
            }
        }

        public Boolean CanVerify
        {
            get
            {
                return this.Checksums.Any(x => x.IsValue);
            }
        }

        public String Tooltip { get; private set; }

        public void Reset()
        {
            this.Checksums.All(x => { x.Value = String.Empty; return true; });
        }

        public void Reset(Status status)
        {
            this.Checksums.All(x => { x.Status = status; return true; });
        }

        public void Update(String method, String checksum)
        {
            Checksum affected = this[method];

            if (affected is null)
            {
                return;
            }

            affected.Status = Status.Unknown;
            affected.Value = checksum ?? String.Empty;
        }

        public override String ToString()
        {
            String checksums = "None";

            if (!this.Checksums.All(x => !x.IsValue))
            {
                checksums = String.Join("; ", this.Checksums.Where(x => x.IsValue).Select(x => x.ToString()));
            }

            return $"{nameof(this.Name)}: {this.Name}, {nameof(this.Checksums)}: {checksums}";
        }

        private String GetTooltip(CultureInfo culture)
        {
            StringBuilder builder = new StringBuilder(512);
            CapacityFormatter capacity = new CapacityFormatter(culture);

            builder.AppendLine(String.Format("Location: {0}", this.FullPath));
            builder.AppendLine(String.Format("Created: {0} (UTC)", this.Created.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss", culture)));
            builder.AppendLine(String.Format("Changed: {0} (UTC)", this.Changed.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss", culture)));
            builder.AppendLine(String.Format("Accessed: {0} (UTC)", this.Accessed.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss", culture)));
            builder.AppendLine(String.Format("Size: {0} ({1})", String.Format(capacity, "{0:two2}", this.Size), String.Format(capacity, "{0:bytes}", this.Size)));
            builder.Append(String.Format("Attributes: {0}", this.Attributes));

            return builder.ToString();
        }
    }
}
