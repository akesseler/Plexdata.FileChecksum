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
using System;

namespace Plexdata.FileChecksum.Cli.Extensions
{
    internal static class MethodExtension
    {
        #region Construction

        static MethodExtension() { }

        #endregion

        #region Methods

        public static String ToDisplay(this Method method)
        {
            return method.ToString().ToUpper();
        }

        public static String Format(this Method method, String hash, String file)
        {
            if (String.IsNullOrWhiteSpace(hash))
            {
                throw new ArgumentOutOfRangeException(nameof(hash));
            }

            if (String.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentOutOfRangeException(nameof(file));
            }

            return String.Format("{0}\t{1}\t{2}", method.ToDisplay(), hash.Trim(), file.Trim());
        }

        #endregion
    }
}
