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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Plexdata.Dialogs
{
    public class FolderEntry : INotifyPropertyChanged
    {
        #region Static fields

        public readonly static FolderEntry DummyEntry = new FolderEntry();

        #endregion

        #region Public events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private fields

        private Boolean selected;
        private Boolean expanded;
        private ObservableCollection<FolderEntry> children;

        #endregion

        #region Construction

        private FolderEntry()
            : base()
        {
            this.Label = ".:?*/DUMMY_ENTRY\\*?:.";
            this.children = new ObservableCollection<FolderEntry>(Enumerable.Repeat(FolderEntry.DummyEntry, 1));
        }

        public FolderEntry(DriveInfo drive)
            : this(drive?.RootDirectory)
        {
            if (String.IsNullOrWhiteSpace(drive.VolumeLabel))
            {
                this.Label = drive.Name.TrimEnd(Path.DirectorySeparatorChar);
            }
            else
            {
                this.Label = $"{drive.VolumeLabel} ({drive.Name.TrimEnd(Path.DirectorySeparatorChar)})";
            }
        }

        public FolderEntry(DirectoryInfo folder)
            : this()
        {
            this.Folder = folder ?? throw new ArgumentNullException(nameof(folder));

            this.Label = this.Folder.Name;

            if (!this.HasChildren(this.Folder))
            {
                this.children = new ObservableCollection<FolderEntry>();
            }

            this.Image = this.LoadImage(this.Folder, false);
        }

        #endregion

        #region Public properties

        public String Label { get; private set; }

        public DirectoryInfo Folder { get; private set; }

        public ImageSource Image { get; private set; }

        public ObservableCollection<FolderEntry> Children
        {
            get
            {
                return this.children;
            }
            set
            {
                if (this.children != value)
                {
                    this.children = value;
                    this.OnPropertyChanged(nameof(this.Children));
                }
            }
        }

        public Boolean IsSelected
        {
            get { return this.selected; }
            set
            {
                if (value != this.selected)
                {
                    this.selected = value;
                    this.OnPropertyChanged(nameof(this.IsSelected));
                }
            }
        }

        public Boolean IsExpanded
        {
            get { return this.expanded; }
            set
            {
                Boolean firing = false;

                try
                {
                    if (this.expanded != value)
                    {
                        this.expanded = value;

                        firing = true;

                        if (this.expanded)
                        {
                            this.LoadChildren();
                        }
                    }
                }
                catch
                {
                    this.expanded = false;
                    throw;
                }
                finally
                {
                    if (firing)
                    {
                        this.OnPropertyChanged(nameof(this.IsExpanded));
                    }
                }
            }
        }

        #endregion

        #region Public methods

        public override String ToString()
        {
            return this.Folder is null ? this.Label : this.Folder.FullName;
        }

        #endregion

        #region Event handlers

        protected void OnPropertyChanged(String property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region Private methods

        private Boolean HasChildren(DirectoryInfo folder)
        {
            try
            {
                if (folder is null)
                {
                    return false;
                }

                return folder.GetDirectories().Any();
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private void LoadChildren()
        {
            try
            {
                DirectoryInfo children = new DirectoryInfo(this.Folder.FullName);

                this.Children = new ObservableCollection<FolderEntry>(
                    children.GetDirectories()
                        .Where(x => !this.IsExcluded(x))
                        .Select(x => new FolderEntry(x))
                );
            }
            catch (UnauthorizedAccessException exception)
            {
                throw exception;
            }
        }

        private Boolean IsExcluded(DirectoryInfo folder)
        {
            if (folder is null)
            {
                return true;
            }

            if (folder.Name.Equals("$RECYCLE.BIN", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private BitmapSource LoadImage(DirectoryInfo folder, Boolean large)
        {
            try
            {
                SHFILEINFO shfi = new SHFILEINFO();

                IntPtr hResult = FolderEntry.SHGetFileInfo(folder.FullName, 0, ref shfi, (UInt32)Marshal.SizeOf(shfi), SHGFI_ICON | (large ? SHGFI_LARGEICON : SHGFI_SMALLICON));

                if (hResult != IntPtr.Zero)
                {
                    BitmapSource result = Imaging.CreateBitmapSourceFromHIcon(shfi.hIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                    FolderEntry.DestroyIcon(shfi.hIcon);

                    return result;
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }

            return null;
        }

        #endregion

        #region Win32 stuff

        private const UInt32 SHGFI_ICON = 0x00000100;
        private const UInt32 SHGFI_LARGEICON = 0x00000000;
        private const UInt32 SHGFI_SMALLICON = 0x00000001;

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public Int32 iIcon;
            public UInt32 dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public String szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public String szTypeName;
        };

        [DllImport("user32.dll")]
        public static extern Boolean DestroyIcon(IntPtr handle);

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(String pszPath, UInt32 dwFileAttributes, ref SHFILEINFO psfi, UInt32 cbSizeFileInfo, UInt32 uFlags);

        #endregion
    }
}
