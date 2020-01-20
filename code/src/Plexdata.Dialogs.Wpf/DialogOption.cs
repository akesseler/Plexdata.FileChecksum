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

namespace Plexdata.Dialogs
{
    public class DialogOption
    {
        private DialogButton button;
        private String label;

        public DialogOption()
            : this(DialogButton.Ok)
        { }

        public DialogOption(DialogButton button)
            : this(button, button.ToString())
        { }

        public DialogOption(DialogButton button, String label)
        {
            this.Button = button;
            this.Label = label;
        }

        public DialogButton Button
        {
            get
            {
                return this.button;
            }
            set
            {
                if ((value & (value - 1)) != 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.Button), "Apply only one single button flag.");
                }

                this.button = value;
            }
        }

        public String Label
        {
            get
            {
                return this.label;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentOutOfRangeException(nameof(this.Button), "The label may not be null, empty or consists only of white spaces.");
                }

                this.label = value.Trim();
            }
        }
    }
}
