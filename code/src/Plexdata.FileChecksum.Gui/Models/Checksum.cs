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
using System;
using System.ComponentModel;

namespace Plexdata.FileChecksum.Gui.Models
{
    public class Checksum : INotifyPropertyChanged
    {
        private String value = null;
        private Status status = Status.Unknown;

        public event PropertyChangedEventHandler PropertyChanged;

        public Checksum(Method method)
            : this(method, null)
        {
        }

        public Checksum(Method method, String value)
        {
            this.Method = method;
            this.Value = value;
        }

        public Method Method { get; private set; }

        public Status Status
        {
            get
            {
                return this.status;
            }
            set
            {
                if (this.status != value)
                {
                    this.status = value;
                    this.OnPropertyChanged(nameof(this.Status));
                }
            }
        }

        public String Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (String.Compare(this.value, value, StringComparison.InvariantCultureIgnoreCase) != 0)
                {
                    this.value = value;
                    this.OnPropertyChanged(nameof(this.Value));
                }
            }
        }

        public Boolean IsValue
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Value);
            }
        }

        public override String ToString()
        {
            return $"{nameof(this.Method)}: {this.Method}, {nameof(this.Value)}: \"{this.Value}\"";
        }

        protected void OnPropertyChanged(String property)
        {
            this.PropertyChanged?.DynamicInvoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
