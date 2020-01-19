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
using System.Windows;

namespace Plexdata.FileChecksum.Gui.Dialogs
{
    /// <summary>
    /// Interaction logic for ImportDialog.xaml
    /// </summary>
    public partial class ImportDialog : Window
    {
        private readonly ImportResult result = null;

        public ImportDialog(Window owner, ImportResult result)
        {
            this.MaxHeight = SystemParameters.WorkArea.Height * 0.7; // 70% of screen height.

            this.InitializeComponent();

            base.Owner = owner;

            this.result = result;
            this.DataContext = this;
        }

        public Visibility IsUpdateVisible
        {
            get
            {
                if (this.result is null || !this.result.IsUpdate)
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }
        }

        public Visibility IsInsertVisible
        {
            get
            {
                if (this.result is null || !this.result.IsInsert)
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }
        }

        public Visibility IsFailedVisible
        {
            get
            {
                if (this.result is null || !this.result.IsFailed)
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }
        }

        public IEnumerable<String> UpdateValues
        {
            get
            {
                return this.result?.ToUpdate();
            }
        }

        public IEnumerable<String> InsertValues
        {
            get
            {
                return this.result?.ToInsert();
            }
        }

        public IEnumerable<String> FailedValues
        {
            get
            {
                return this.result?.ToFailed();
            }
        }

        private void OnCloseButtonClick(Object sender, RoutedEventArgs args)
        {
            this.Close();
        }
    }
}
