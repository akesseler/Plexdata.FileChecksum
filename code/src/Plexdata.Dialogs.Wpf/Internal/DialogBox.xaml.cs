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
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Plexdata.Dialogs.Internal
{
    /// <summary>
    /// Interaction logic for DialogBox.xaml
    /// </summary>
    public partial class DialogBox : Window
    {
        #region Private fields

        private readonly Dictionary<DialogButton, String> labels;

        #endregion

        #region Construction

        public DialogBox(Window owner, String message, String caption, DialogButton buttons, DialogSymbol symbol, DialogOption[] options)
        {
            this.MinHeight = 180;
            this.MaxHeight = SystemParameters.WorkArea.Height;
            this.MinWidth = 550; // Don't change! Otherwise the link is covered by buttons if all of them are visible.
            this.MaxWidth = 800;
            this.Height = 180;
            this.Width = 550;

            this.Message = (message ?? String.Empty).Trim();
            this.Title = (caption ?? String.Empty).Trim();
            this.Symbol = this.GetSymbol(symbol);
            this.Buttons = buttons;
            this.Result = Dialogs.DialogResult.None;

            this.labels = this.CreateLabels();

            this.ApplyOptions(options);

            this.InitializeComponent();

            base.Owner = owner;
            base.WindowStartupLocation = (base.Owner is null) ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;

            this.DataContext = this;
        }

        #endregion

        #region Public properties

        public String Message { get; private set; }

        public DialogButton Buttons { get; private set; }

        public BitmapSource Symbol { get; private set; }

        public DialogResult Result { get; private set; }

        #endregion

        #region Symbol Visibility

        public Visibility SymbolVisibility
        {
            get
            {
                if (this.Symbol is null)
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        #endregion

        #region Button visibility

        public Visibility OkButtonVisibility
        {
            get
            {
                if ((this.Buttons & DialogButton.Ok) == DialogButton.Ok)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        public Visibility CloseButtonVisibility
        {
            get
            {
                if ((this.Buttons & DialogButton.Close) == DialogButton.Close)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        public Visibility CancelButtonVisibility
        {
            get
            {
                if ((this.Buttons & DialogButton.Cancel) == DialogButton.Cancel)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        public Visibility YesButtonVisibility
        {
            get
            {
                if ((this.Buttons & DialogButton.Yes) == DialogButton.Yes)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        public Visibility NoButtonVisibility
        {
            get
            {
                if ((this.Buttons & DialogButton.No) == DialogButton.No)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        #endregion

        #region Button labels

        public String OkButtonLabel
        {
            get
            {
                return this.labels[DialogButton.Ok];
            }
        }

        public String CloseButtonLabel
        {
            get
            {
                return this.labels[DialogButton.Close];
            }
        }

        public String CancelButtonLabel
        {
            get
            {
                return this.labels[DialogButton.Cancel];
            }
        }

        public String YesButtonLabel
        {
            get
            {
                return this.labels[DialogButton.Yes];
            }
        }

        public String NoButtonLabel
        {
            get
            {
                return this.labels[DialogButton.No];
            }
        }

        #endregion

        #region Link labels

        public String CopyClipboardLabel
        {
            get
            {
                return "Copy to Clipboard";
            }

        }

        #endregion

        #region Event handlers

        private void OnOkButtonClick(Object sender, RoutedEventArgs args)
        {
            this.HandleClosing(Dialogs.DialogResult.OK);
        }

        private void OnCloseButtonClick(Object sender, RoutedEventArgs args)
        {
            this.HandleClosing(Dialogs.DialogResult.Close);
        }

        private void OnCancelButtonClick(Object sender, RoutedEventArgs args)
        {
            this.HandleClosing(Dialogs.DialogResult.Cancel);
        }

        private void OnYesButtonClick(Object sender, RoutedEventArgs args)
        {
            this.HandleClosing(Dialogs.DialogResult.Yes);
        }

        private void OnNoButtonClick(Object sender, RoutedEventArgs args)
        {
            this.HandleClosing(Dialogs.DialogResult.No);
        }

        private void OnCopyClipboardClick(Object sender, RoutedEventArgs args)
        {
            try
            {
                Clipboard.SetText(this.Message);
            }
            catch { }
        }

        #endregion

        #region Private methods

        private void HandleClosing(Dialogs.DialogResult result)
        {
            this.Hide();
            this.Result = result;
            this.Close();
        }

        private BitmapSource GetSymbol(DialogSymbol symbol)
        {
            IntPtr handle;

            switch (symbol)
            {
                case DialogSymbol.Application:
                    handle = SystemIcons.Application.Handle;
                    break;
                case DialogSymbol.Asterisk:
                    handle = SystemIcons.Asterisk.Handle;
                    break;
                case DialogSymbol.Error:
                    handle = SystemIcons.Error.Handle;
                    break;
                case DialogSymbol.Exclamation:
                    handle = SystemIcons.Exclamation.Handle;
                    break;
                case DialogSymbol.Hand:
                    handle = SystemIcons.Hand.Handle;
                    break;
                case DialogSymbol.Information:
                    handle = SystemIcons.Information.Handle;
                    break;
                case DialogSymbol.Question:
                    handle = SystemIcons.Question.Handle;
                    break;
                case DialogSymbol.Shield:
                    handle = SystemIcons.Shield.Handle;
                    break;
                case DialogSymbol.Warning:
                    handle = SystemIcons.Warning.Handle;
                    break;
                case DialogSymbol.WinLogo:
                    handle = SystemIcons.WinLogo.Handle;
                    break;
                default:
                    return null;
            }

            return Imaging.CreateBitmapSourceFromHIcon(handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private Dictionary<DialogButton, String> CreateLabels()
        {
            return new Dictionary<DialogButton, String>()
            {
                { DialogButton.Ok, "_Ok" },
                { DialogButton.Close, "Cl_ose" },
                { DialogButton.Cancel, "_Cancel" },
                { DialogButton.Yes, "_Yes" },
                { DialogButton.No,"_No" }
            };
        }

        private void ApplyOptions(DialogOption[] options)
        {
            if (options is null || !options.Any())
            {
                return;
            }

            foreach (DialogOption option in options)
            {
                this.labels[option.Button] = option.Label;
            }
        }

        #endregion
    }
}
