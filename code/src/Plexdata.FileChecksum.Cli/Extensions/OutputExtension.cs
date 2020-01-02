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

using Plexdata.FileChecksum.Cli.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plexdata.FileChecksum.Cli.Extensions
{
    internal static class OutputExtension
    {
        #region Construction

        static OutputExtension() { }

        #endregion

        #region Methods

        public static void Print(this IEnumerable<Output> outputs)
        {
            if (outputs is null || !outputs.Any())
            {
                return;
            }

            Int32 total = outputs.Max(x => x.Title.Length);

            foreach (Output output in outputs)
            {
                output.Print(total);
            }
        }

        public static void Print(this Output output, Int32 length)
        {
            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            ConsoleColor restore = Console.ForegroundColor;

            try
            {
                if (output.Color.HasValue)
                {
                    Console.ForegroundColor = output.Color.Value;
                }

                if (String.IsNullOrWhiteSpace(output.Title))
                {
                    Console.WriteLine($"{output.Value}");
                }
                else
                {
                    length += 2; // Colon and at least one space.
                    Console.WriteLine($"{String.Format("{0}:", output.Title).PadRight(length, ' ')}{output.Value}");
                }
            }
            finally
            {
                if (output.Color.HasValue)
                {
                    Console.ForegroundColor = restore;
                }
            }
        }

        #endregion
    }
}
