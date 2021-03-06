﻿/*
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

namespace Plexdata.FileChecksum.Constants
{
    /// <summary>
    /// Enumeration of supported checksum methods.
    /// </summary>
    /// <remarks>
    /// This enumeration represents all supported checksum methods.
    /// </remarks>
    public enum Method
    {
        /// <summary>
        /// Use `MD5` method.
        /// </summary>
        /// <remarks>
        /// The `MD5` checksum method should be used.
        /// </remarks>
        Md5,

        /// <summary>
        /// Use `SHA1` method.
        /// </summary>
        /// <remarks>
        /// The `SHA1` checksum method should be used.
        /// </remarks>
        Sha1,

        /// <summary>
        /// Use `SHA256` method.
        /// </summary>
        /// <remarks>
        /// The `SHA256` checksum method should be used.
        /// </remarks>
        Sha256,

        /// <summary>
        /// Use `SHA384` method.
        /// </summary>
        /// <remarks>
        /// The `SHA384` checksum method should be used.
        /// </remarks>
        Sha384,

        /// <summary>
        /// Use `SHA512` method.
        /// </summary>
        /// <remarks>
        /// The `SHA512` checksum method should be used.
        /// </remarks>
        Sha512,
    }
}
