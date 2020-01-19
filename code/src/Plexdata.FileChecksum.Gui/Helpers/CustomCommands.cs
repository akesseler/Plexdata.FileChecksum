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

using System.Windows.Input;

namespace Plexdata.FileChecksum.Gui.Helpers
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Insert = new RoutedUICommand(
            "Insert",
            "Insert",
            typeof(CustomCommands)
        );

        public static readonly RoutedUICommand Clear = new RoutedUICommand(
            "Clear",
            "Clear",
            typeof(CustomCommands)
        );

        public static readonly RoutedUICommand Create = new RoutedUICommand(
            "Create",
            "Create",
            typeof(CustomCommands)
        );

        public static readonly RoutedUICommand Verify = new RoutedUICommand(
            "Verify",
            "Verify",
            typeof(CustomCommands)
        );

        public static readonly RoutedUICommand Export = new RoutedUICommand(
            "Export",
            "Export",
            typeof(CustomCommands)
        );

        public static readonly RoutedUICommand Import = new RoutedUICommand(
            "Import",
            "Import",
            typeof(CustomCommands)
        );
    }
}
