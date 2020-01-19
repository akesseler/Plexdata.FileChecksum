/*
 * MIT License
 *
 * Copyright(c) 2020 plexdata.de
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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Plexdata.Dialogs
{
    /// <summary>
    /// Interaction logic for OpenFolderDialog.xaml
    /// </summary>
    public partial class OpenFolderDialog : Window
    {
        #region Private fields

        private String initialPath = null;

        #endregion

        #region Construction

        public OpenFolderDialog()
            : this(null)
        { }

        public OpenFolderDialog(Window owner)
            : base()
        {
            this.InitializeComponent();
            base.Owner = owner;

            this.RootFolders = this.LoadRootFolders();
            base.DataContext = this;

            this.folderTreeView.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(OnTreeViewItemExpanded));
            this.folderTreeView.AddHandler(TreeViewItem.CollapsedEvent, new RoutedEventHandler(OnTreeViewItemCollapsed));
            this.folderTreeView.SelectedItemChanged += this.OnTreeViewSelectedItemChanged;
        }

        #endregion

        #region Public properties

        public ObservableCollection<FolderEntry> RootFolders { get; private set; }

        public Visibility IsMessageVisible
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this.Message))
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }
        }

        public String Message { get; set; }

        public DirectoryInfo SelectedFolder { get; private set; }

        public String InitialPath
        {
            get
            {
                return this.initialPath;
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    value = value.TrimEnd(Path.DirectorySeparatorChar);
                }

                if (this.initialPath != value)
                {
                    this.initialPath = value;

                    this.ExpandInitialPath();
                }
            }
        }

        #endregion

        #region Event handlers

        private void OnTreeViewItemExpanded(Object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is TreeViewItem affected)
            {
                if (affected.Header is FolderEntry source)
                {
                    Cursor cursor = this.Cursor;
                    this.Cursor = Cursors.Wait;

                    try
                    {
                        source.IsExpanded = true;
                    }
                    catch (UnauthorizedAccessException exception)
                    {
                        DialogBox.Show(this, exception.Message, this.Title, DialogSymbol.Error);
                    }
                    catch (Exception exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception);
                    }
                    finally
                    {
                        args.Handled = true;
                        this.Cursor = cursor;
                    }
                }
            }
        }

        private void OnTreeViewItemCollapsed(Object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is TreeViewItem affected)
            {
                if (affected.Header is FolderEntry source)
                {
                    try
                    {
                        source.IsExpanded = false;
                    }
                    catch (Exception exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception);
                    }
                    finally
                    {
                        args.Handled = true;
                    }
                }
            }
        }

        private void OnTreeViewSelectedItemChanged(Object sender, RoutedPropertyChangedEventArgs<Object> args)
        {
            if (args.NewValue is FolderEntry affected)
            {
                this.SelectedFolder = affected.Folder;
            }
        }

        private void OnDialogButtonClick(Object sender, RoutedEventArgs args)
        {
            if (sender is Button button)
            {
                // An exception is thrown if window was shown 
                // using method Show() instead of ShowDialog().
                try { this.DialogResult = !button.IsCancel; } catch { }
            }

            this.Close();
        }

        #endregion

        #region Private methods

        private ObservableCollection<FolderEntry> LoadRootFolders()
        {
            ObservableCollection<FolderEntry> result = new ObservableCollection<FolderEntry>();

            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        result.Add(new FolderEntry(drive));
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }

            return result;
        }

        private void ExpandInitialPath()
        {
            if (!String.IsNullOrWhiteSpace(this.InitialPath))
            {
                this.ExpandInitialPath(this.InitialPath.Split(Path.DirectorySeparatorChar).ToList(), this.RootFolders);
            }

            this.folderTreeView.Focus();
        }

        private void ExpandInitialPath(List<String> pieces, ObservableCollection<FolderEntry> children)
        {
            if (pieces is null || !pieces.Any())
            {
                return;
            }

            if (children is null || !children.Any())
            {
                return;
            }

            String piece = pieces[0];
            pieces.RemoveAt(0);

            foreach (FolderEntry child in children)
            {
                if (String.Equals(child.Folder.Name.TrimEnd(Path.DirectorySeparatorChar), piece, StringComparison.InvariantCultureIgnoreCase))
                {
                    try
                    {
                        child.IsExpanded = true;

                        if (!child.Children.Any())
                        {
                            child.IsSelected = true;
                            return;
                        }

                        this.ExpandInitialPath(pieces, child.Children);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        child.IsSelected = true;
                    }
                    catch (Exception exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception);
                    }
                }
            }
        }

        #endregion
    }
}
