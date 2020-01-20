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
using Plexdata.FileChecksum.Factories;
using Plexdata.FileChecksum.Gui.Constants;
using Plexdata.FileChecksum.Gui.Helpers;
using Plexdata.FileChecksum.Gui.Models;
using Plexdata.FileChecksum.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Plexdata.FileChecksum.Gui.Dialogs
{
    /// <summary>
    /// Interaction logic for VerifyDialog.xaml
    /// </summary>
    public partial class VerifyDialog : Window
    {
        private BackgroundWorker worker = null;
        private CancellationTokenSource cancellationTokenSource = null;
        private Cursor defaultCursor = null;

        public VerifyDialog()
            : this(App.Current.MainWindow)
        {
        }

        public VerifyDialog(Window owner)
        {
            this.InitializeComponent();
            base.Owner = owner;
            this.SetupValues();
        }

        public ObservableCollection<MethodHelper> Methods { get; set; }

        private void SetupValues()
        {
            this.worker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            this.worker.DoWork += this.OnWorkerDoWork;
            this.worker.ProgressChanged += this.OnWorkerProgressChanged;
            this.worker.RunWorkerCompleted += this.OnWorkerRunWorkerCompleted;

            this.cancellationTokenSource = new CancellationTokenSource();

            this.defaultCursor = this.Cursor;

            this.Methods = new ObservableCollection<MethodHelper>();

            foreach (Method method in Enum.GetValues(typeof(Method)))
            {
                this.Methods.Add(new MethodHelper(method));
            }

            this.DataContext = this;
        }

        private void OnStartButtonClick(Object sender, RoutedEventArgs args)
        {
            this.btStart.Visibility = Visibility.Collapsed;
            this.btCancel.Visibility = Visibility.Visible;
            this.gbProcessing.Visibility = Visibility.Visible;
            this.pbProgress.IsIndeterminate = false;

            Boolean selection = this.rbSelection.IsChecked ?? false;
            IList<FileItem> fileItems = (this.Owner as MainWindow).GetFileItems(selection);

            this.pbProgress.Minimum = 0;
            this.pbProgress.Maximum = fileItems.Count * this.Methods.Count(x => x.IsSelected);
            this.pbProgress.Value = 0;

            if (this.worker.IsBusy)
            {
                this.cancellationTokenSource.Cancel();
                this.worker.CancelAsync();
            }

            this.cancellationTokenSource = new CancellationTokenSource();

            this.worker.RunWorkerAsync(fileItems);
        }

        private void OnWorkerProgressChanged(Object sender, ProgressChangedEventArgs args)
        {
            this.pbProgress.Value = args.ProgressPercentage;
        }

        private void OnWorkerRunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs args)
        {
            this.btStart.Visibility = Visibility.Visible;
            this.btCancel.Visibility = Visibility.Collapsed;
            this.gbProcessing.Visibility = Visibility.Collapsed;
            this.Cursor = this.defaultCursor;
        }

        private void OnWorkerDoWork(Object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            IList<FileItem> fileItems = args.Argument as IList<FileItem>;
            Int32 index = 1;

            TaskFactory factory = new TaskFactory(
                this.cancellationTokenSource.Token,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

            // BUG: All checksum values are reset no matter if they are going to be calculated or not.
            fileItems.All(x => { x.Reset(Status.Unknown); return true; });

            foreach (FileItem fileItem in fileItems)
            {
                if (worker.CancellationPending)
                {
                    return;
                }

                foreach (MethodHelper current in this.Methods.Where(x => x.IsSelected))
                {
                    Checksum checksum = fileItem[current.Method];

                    if (checksum is null) { continue; }

                    checksum.Status = Status.Processing;

                    if (worker.CancellationPending)
                    {
                        checksum.Status = Status.Canceled;
                        return;
                    }

                    String result = factory
                        .StartNew<Task<String>>(() => this.HandleCreateAsync(fileItem.FullPath, checksum.Method, this.cancellationTokenSource.Token))
                        .Unwrap<String>()
                        .GetAwaiter()
                        .GetResult();

                    if (worker.CancellationPending)
                    {
                        checksum.Status = Status.Canceled;
                        return;
                    }

                    if (result.Equals(checksum.Value, StringComparison.InvariantCultureIgnoreCase))
                    {
                        checksum.Status = Status.Confirmed;
                    }
                    else
                    {
                        checksum.Status = Status.Unconfirmed;
                    }


                    worker.ReportProgress(index++);
                }
            }
        }

        private async Task<String> HandleCreateAsync(String fullpath, Method method, CancellationToken token)
        {
            using (FileStream stream = File.OpenRead(fullpath))
            {
                using (IChecksumCalculator calculator = ChecksumCalculatorFactory.Create(method))
                {
                    if (token.IsCancellationRequested)
                    {
                        return String.Empty;
                    }

                    return await calculator.CalculateAsync(stream, token);
                }
            }
        }

        private void OnCloseButtonClick(Object sender, RoutedEventArgs args)
        {
            if (this.worker.IsBusy)
            {
                this.cancellationTokenSource.Cancel();
                this.worker.CancelAsync();
            }

            this.worker.Dispose();
            this.cancellationTokenSource.Dispose();

            this.Close();
        }

        private void OnCancelButtonClick(Object sender, RoutedEventArgs args)
        {
            this.Cursor = Cursors.AppStarting;

            if (this.worker.IsBusy)
            {
                this.cancellationTokenSource.Cancel();
                this.worker.CancelAsync();
            }
        }
    }
}
