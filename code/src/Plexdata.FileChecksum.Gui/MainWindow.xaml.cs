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

using Microsoft.Win32;
using Plexdata.Dialogs;
using Plexdata.FileChecksum.Gui.Constants;
using Plexdata.FileChecksum.Gui.Dialogs;
using Plexdata.FileChecksum.Gui.Helpers;
using Plexdata.FileChecksum.Gui.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Plexdata.FileChecksum.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.ApplyTitle();

            this.filesListBox.ItemContainerGenerator.StatusChanged += this.OnFilesListBoxContainersGenerated;

            this.FileItems = new ObservableCollection<FileItem>();
            this.DataContext = this.FileItems;
        }

        public ObservableCollection<FileItem> FileItems { get; private set; }

        public IList<FileItem> GetFileItems(Boolean selection)
        {
            List<FileItem> result = new List<FileItem>();

            if (selection)
            {
                foreach (FileItem current in this.filesListBox.SelectedItems)
                {
                    result.Add(current);
                }
            }
            else
            {
                result.AddRange(this.FileItems);
            }

            return result;
        }

        #region Command events

        private void OnExecutedExitCommand(Object sender, ExecutedRoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

        private void OnExecutedOpenCommand(Object sender, ExecutedRoutedEventArgs args)
        {
            this.LoadFolder();
        }

        private void OnExecutedInsertCommand(Object sender, ExecutedRoutedEventArgs args)
        {
            this.LoadFiles();
        }

        private void OnExecutedClearCommand(Object sender, ExecutedRoutedEventArgs args)
        {
            this.FileItems.Clear();
        }

        private void OnExecutedCreateCommand(Object sender, ExecutedRoutedEventArgs args)
        {
            if (!this.FileItems.Any())
            {
                if (!this.LoadFiles()) { return; }
            }

            CreateDialog dialog = new CreateDialog(this);
            dialog.ShowDialog();
        }

        private void OnExecutedVerifyCommand(Object sender, ExecutedRoutedEventArgs args)
        {
            if (!this.FileItems.Any())
            {
                this.LoadFiles();
                return;
            }

            if (!this.FileItems.Any(x => x.CanVerify))
            {
                DialogBox.Show(this, "At least one file with at least one checksum to verify is required.", DialogSymbol.Exclamation);
                return;
            }

            VerifyDialog dialog = new VerifyDialog(this);
            dialog.ShowDialog();
        }

        private void OnExecutedExportCommand(Object sender, ExecutedRoutedEventArgs args)
        {
            if (!this.FileItems.Any())
            {
                DialogBox.Show(this, "Load some files and create checksums beforehand.", DialogSymbol.Information);
                return;
            }

            var values = from fs in this.FileItems
                         from cs in fs.Checksums
                         where cs.IsValue && cs.Status == Status.Created
                         select $"{cs.Method.ToString().ToUpper()} {cs.Value} \"{fs.FullPath}\"";

            if (values.Any())
            {
                String message = "Do you want to copy results to clipboard or do you want to write results into a file?";

                DialogResult result = DialogBox.Show(this, message,
                    DialogSymbol.Question, DialogButton.YesNoCancel,
                    new DialogOption(DialogButton.Yes, "Clipboard"),
                    new DialogOption(DialogButton.No, "File"),
                    DialogOption.DefaultButtonCancel);

                switch (result)
                {
                    case Plexdata.Dialogs.DialogResult.Yes:
                        this.ExportClipboard(values);
                        break;
                    case Plexdata.Dialogs.DialogResult.No:
                        this.ExportFile(values);
                        break;
                }
            }
        }

        private void OnExecutedImportCommand(Object sender, ExecutedRoutedEventArgs args)
        {
            const Char SP = '\u2423';
            const Char CR = '\u23CE';

            String message = String.Format(
                "It is attempted to import data in the following format:{0}" +
                "{0}" +
                "<method-1>{1}<checksum>{1}<file-1-path>{2}{0}" +
                "<method-2>{1}<checksum>{1}<file-1-path>{2}{0}" +
                "<method-3>{1}<checksum>{1}<file-1-path>{2}{0}" +
                "<method-1>{1}<checksum>{1}<file-2-path>{2}{0}" +
                "<method-2>{1}<checksum>{1}<file-2-path>{2}{0}" +
                "<method-3>{1}<checksum>{1}<file-2-path>{2}{0}" +
                "{0}" +
                "Note: In doubt surround all file paths by double quotes.{0}" +
                "{0}" +
                "Do you want to import checksums from clipboard or from a file?",
                Environment.NewLine, SP, CR);

            DialogResult result = DialogBox.Show(this, message,
                DialogSymbol.Question, DialogButton.YesNoCancel,
                new DialogOption(DialogButton.Yes, "Clipboard"),
                new DialogOption(DialogButton.No, "File"),
                DialogOption.DefaultButtonCancel);

            switch (result)
            {
                case Plexdata.Dialogs.DialogResult.Yes:
                    this.ImportClipboard();
                    break;
                case Plexdata.Dialogs.DialogResult.No:
                    this.ImportFile();
                    break;
            }
        }

        private void OnExecutedAboutCommand(Object sender, RoutedEventArgs args)
        {
            // https://blogs.msdn.microsoft.com/pedrosilva/2009/05/08/wpfaboutbox-dialog-layout-and-styles/
            AboutDialog dialog = new AboutDialog(this);
            dialog.ShowDialog();
        }

        #endregion

        #region Listbox events

        private void OnFilesListBoxValidateDropData(Object sender, DragEventArgs args)
        {
            String[] items = this.GetDropData(args);

            if (items is null || !items.Any())
            {
                args.Effects = DragDropEffects.None;
            }
            else
            {
                args.Effects = DragDropEffects.Link;
            }

            args.Handled = true;
        }

        private void OnFilesListBoxDropData(Object sender, DragEventArgs args)
        {
            String[] filenames = this.GetDropData(args);

            foreach (String filename in filenames)
            {
                if (!this.FileItems.Any(x => x.FullPath.Equals(filename, StringComparison.InvariantCultureIgnoreCase)))
                {
                    this.FileItems.Add(new FileItem(filename));
                }
            }

            this.filesListBox.Items.MoveCurrentToLast();
            this.filesListBox.ScrollIntoView(this.filesListBox.Items.CurrentItem);
        }

        private void OnFilesListBoxContainersGenerated(Object sender, EventArgs args)
        {
            switch (this.filesListBox.ItemContainerGenerator.Status)
            {
                case GeneratorStatus.Error:
                case GeneratorStatus.ContainersGenerated:
                    Mouse.OverrideCursor = null;
                    break;
            }
        }

        #endregion

        #region Private methods

        private void ApplyTitle()
        {
            String title = AssemblyDataLoader.Load(Assembly.GetExecutingAssembly())?.Title;
            if (!String.IsNullOrWhiteSpace(title)) { this.Title = title; }
        }

        private String[] GetDropData(DragEventArgs args)
        {
            return args.Data.GetData("FileDrop") as String[];
        }

        private Boolean LoadFiles()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "*.*",
                Multiselect = true,
                Filter = "All Files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (String fullpath in dialog.FileNames)
                {
                    if (!this.FileItems.Any(x => String.Equals(x.FullPath, fullpath, StringComparison.OrdinalIgnoreCase)))
                    {
                        this.FileItems.Add(new FileItem(fullpath));
                    }
                }

                return this.FileItems.Any();
            }

            return false;
        }

        private Boolean LoadFolder()
        {
            DirectoryInfo result = OpenFolderDialog.Show(this, "Choose a folder to load all files from.", DriveInfo.GetDrives().FirstOrDefault()?.RootDirectory);

            if (result is null)
            {
                return false;
            }

            // NOTE: Cursor is reset in event handler `OnFilesListBoxContainersGenerated`.
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                IEnumerable<String> fullpaths = result.GetFiles().Where(x => x != null).Select(x => x.FullName);

                if (fullpaths.Any())
                {
                    this.FileItems.Clear();

                    foreach (String fullpath in fullpaths)
                    {
                        this.FileItems.Add(new FileItem(fullpath));
                    }

                    return this.FileItems.Any();
                }
                else
                {
                    DialogBox.Show(this, $"No files found in \"{result.FullName}\".", DialogSymbol.Warning);
                }
            }
            catch { }

            return false;
        }

        private void ExportClipboard(IEnumerable<String> values)
        {
            try
            {
                Clipboard.SetText(String.Join(Environment.NewLine, values));
            }
            catch (Exception exception)
            {
                DialogBox.Show(this, exception.ToString(), DialogSymbol.Error);
            }
        }

        private void ExportFile(IEnumerable<String> values)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                CheckFileExists = false,
                CheckPathExists = true,
                DefaultExt = ".txt",
                AddExtension = false,
                FileName = $"checksums-{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt",
                Filter = "Plain Text Files (*.txt)|*.txt|Checksum Files (*.chk)|*.chk|All Files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(dialog.FileName, String.Join(Environment.NewLine, values), Encoding.UTF8);
                }
                catch (Exception exception)
                {
                    DialogBox.Show(this, exception.ToString(), DialogSymbol.Error);
                }
            }
        }

        private void ImportClipboard()
        {
            try
            {
                String value = Clipboard.GetText();

                if (String.IsNullOrWhiteSpace(value))
                {
                    DialogBox.Show(this, "The Clipboard seems to be empty.", DialogSymbol.Exclamation);
                    return;
                }

                this.HandleImport(value);
            }
            catch (Exception exception)
            {
                DialogBox.Show(this, exception.ToString(), DialogSymbol.Error);
            }
        }

        private void ImportFile()
        {
            // TODO: Support of file extensions like md5, sha1, etc.
            // But keep in mind that a file with such an extension may not 
            // include the `method`. Therefore, it would be necessary to 
            // take the method type from the extension.

            OpenFileDialog dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".txt",
                Multiselect = false,
                Filter = "Plain Text Files (*.txt)|*.txt|Checksum Files (*.chk)|*.chk|All Files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    String value = File.ReadAllText(dialog.FileName);

                    if (String.IsNullOrWhiteSpace(value))
                    {
                        DialogBox.Show(this, "The File seems to be empty.", DialogSymbol.Exclamation);
                        return;
                    }

                    this.HandleImport(value);
                }
                catch (Exception exception)
                {
                    DialogBox.Show(this, exception.ToString(), DialogSymbol.Error);
                }
            }
        }

        private void HandleImport(String value)
        {
            IEnumerable<ImportItem> results = ImportParser.Parse(value);

            Dictionary<String, List<ImportItem>> updates = new Dictionary<String, List<ImportItem>>();
            Dictionary<String, List<ImportItem>> inserts = new Dictionary<String, List<ImportItem>>();

            ImportResult result = new ImportResult();

            result.AddFailed(results.Where(x => !x.IsValid));

            var examinees = results
                .Where(x => x.IsValid)
                .GroupBy(x => x.FilePath, (file, items) => new { File = file, Items = items.ToList() });

            foreach (var examinee in examinees)
            {
                if (this.FileItems.Any(x => x.FullPath.Equals(examinee.File, StringComparison.InvariantCultureIgnoreCase)))
                {
                    updates.Add(examinee.File, examinee.Items);
                }
                else
                {
                    inserts.Add(examinee.File, examinee.Items);
                }
            }

            foreach (KeyValuePair<String, List<ImportItem>> update in updates)
            {
                FileItem candidate = this.FileItems.Single(x => x.FullPath.Equals(update.Key, StringComparison.InvariantCultureIgnoreCase));

                foreach (ImportItem import in update.Value)
                {
                    candidate.Update(import.Method, import.Checksum);
                }

                result.AddUpdate(update.Value);
            }

            foreach (KeyValuePair<String, List<ImportItem>> insert in inserts)
            {
                FileItem candidate = new FileItem(insert.Key);

                foreach (ImportItem import in insert.Value)
                {
                    candidate.Update(import.Method, import.Checksum);
                }

                result.AddInsert(insert.Value);

                this.FileItems.Add(candidate);
            }

            if (result.IsResult)
            {
                ImportDialog dialog = new ImportDialog(this, result);
                dialog.ShowDialog();
            }
        }

        #endregion
    }
}

