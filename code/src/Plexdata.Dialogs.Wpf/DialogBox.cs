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
using System.Windows;

namespace Plexdata.Dialogs
{
    public static class DialogBox
    {
        public static DialogResult Show(String message)
        {
            return DialogBox.Show((Window)null, message);
        }

        public static DialogResult Show(Window owner, String message)
        {
            return DialogBox.Show(owner, message, null);
        }

        public static DialogResult Show(String message, DialogSymbol symbol)
        {
            return DialogBox.Show((Window)null, message, symbol);
        }

        public static DialogResult Show(Window owner, String message, DialogSymbol symbol)
        {
            return DialogBox.Show(owner, message, owner?.Title, symbol, DialogButton.Close);
        }

        public static DialogResult Show(String message, String caption)
        {
            return DialogBox.Show(null, message, caption);
        }

        public static DialogResult Show(Window owner, String message, String caption)
        {
            return DialogBox.Show(owner, message, caption, DialogSymbol.None);
        }

        public static DialogResult Show(String message, String caption, DialogSymbol symbol)
        {
            return DialogBox.Show(null, message, caption, symbol);
        }

        public static DialogResult Show(Window owner, String message, String caption, DialogSymbol symbol)
        {
            return DialogBox.Show(owner, message, caption, symbol, DialogButton.Close);
        }

        public static DialogResult Show(String message, String caption, DialogButton buttons)
        {
            return DialogBox.Show(null, message, caption, DialogSymbol.None, buttons, null);
        }

        public static DialogResult Show(String message, String caption, DialogButton buttons, params DialogOption[] options)
        {
            return DialogBox.Show(null, message, caption, DialogSymbol.None, buttons, options);
        }

        public static DialogResult Show(Window owner, String message, String caption, DialogButton buttons)
        {
            return DialogBox.Show(owner, message, caption, DialogSymbol.None, buttons, null);
        }

        public static DialogResult Show(Window owner, String message, String caption, DialogButton buttons, params DialogOption[] options)
        {
            return DialogBox.Show(owner, message, caption, DialogSymbol.None, buttons, options);
        }

        public static DialogResult Show(String message, String caption, DialogSymbol symbol, DialogButton buttons)
        {
            return DialogBox.Show(null, message, caption, symbol, buttons, null);
        }

        public static DialogResult Show(String message, String caption, DialogSymbol symbol, DialogButton buttons, params DialogOption[] options)
        {
            return DialogBox.Show(null, message, caption, symbol, buttons, options);
        }

        public static DialogResult Show(Window owner, String message, String caption, DialogSymbol symbol, DialogButton buttons)
        {
            return DialogBox.Show(owner, message, caption, symbol, buttons, null);
        }

        public static DialogResult Show(Window owner, String message, String caption, DialogSymbol symbol, DialogButton buttons, params DialogOption[] options)
        {
            Internal.DialogBox dialog = new Internal.DialogBox(owner, message, caption, buttons, symbol, options);

            dialog.ShowDialog();

            return dialog.Result;
        }
    }
}
