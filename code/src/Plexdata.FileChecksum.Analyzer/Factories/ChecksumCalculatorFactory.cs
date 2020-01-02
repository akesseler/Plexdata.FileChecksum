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
using Plexdata.FileChecksum.Interfaces;
using Plexdata.FileChecksum.Internals.Algorithms;
using System;

namespace Plexdata.FileChecksum.Factories
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    public static class ChecksumCalculatorFactory
    {
        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        static ChecksumCalculatorFactory() { }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="method">
        /// </param>
        /// <returns>
        /// </returns>
        /// <seealso cref="Method"/>
        /// <seealso cref="ChecksumCalculatorFactory.Create(Method, String)"/>
        /// <seealso cref="System.Security.Cryptography.MD5.Create()"/>
        /// <seealso cref="System.Security.Cryptography.SHA1.Create()"/>
        /// <seealso cref="System.Security.Cryptography.SHA256.Create()"/>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public static IChecksumCalculator Create(Method method)
        {
            return ChecksumCalculatorFactory.Create(method, null);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="method">
        /// </param>
        /// <param name="algorithm">
        /// </param>
        /// <returns>
        /// </returns>
        /// <seealso cref="Method"/>
        /// <seealso cref="ChecksumCalculatorFactory.Create(Method)"/>
        /// <seealso cref="System.Security.Cryptography.MD5.Create(String)"/>
        /// <seealso cref="System.Security.Cryptography.SHA1.Create(String)"/>
        /// <seealso cref="System.Security.Cryptography.SHA256.Create(String)"/>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public static IChecksumCalculator Create(Method method, String algorithm)
        {
            switch (method)
            {
                case Method.Md5:
                    return new Md5ChecksumCalculator(algorithm);
                case Method.Sha1:
                    return new Sha1ChecksumCalculator(algorithm);
                case Method.Sha256:
                    return new Sha256ChecksumCalculator(algorithm);
                default:
                    throw ChecksumCalculatorFactory.GetNotSupportedException(method, algorithm);
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="method">
        /// </param>
        /// <param name="algorithm">
        /// </param>
        /// <returns>
        /// </returns>
        /// <seealso cref="ChecksumCalculatorFactory.Create(Method)"/>
        /// <seealso cref="ChecksumCalculatorFactory.Create(Method, String)"/>
        private static Exception GetNotSupportedException(Method method, String algorithm)
        {
            if (String.IsNullOrWhiteSpace(algorithm))
            {
                return new NotSupportedException($"Method `{method}` is not supported.");
            }
            else
            {
                return new NotSupportedException($"Method `{method} ({algorithm})` is not supported.");
            }
        }
    }
}
