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

namespace Plexdata.FileChecksum.Gui.Models
{
    public class ImportItem
    {
        public ImportItem() { }

        public Boolean IsValid
        {
            get
            {
                return
                    !String.IsNullOrWhiteSpace(this.Method) &&
                    !String.IsNullOrWhiteSpace(this.Checksum) &&
                    !String.IsNullOrWhiteSpace(this.FilePath) &&
                    System.IO.File.Exists(this.FilePath);
            }
        }

        public String Method { get; set; }

        public String Checksum { get; set; }

        public String FilePath { get; set; }

        public Int32 Line { get; set; }

        public String Data { get; set; }

        public override String ToString()
        {
            return $"{nameof(this.IsValid)}: {this.IsValid}, {nameof(this.Method)}: \"{this.Method}\", {nameof(this.Checksum)}: \"{this.Checksum}\", {nameof(this.FilePath)}: \"{this.FilePath}\"";
        }
    }
}
