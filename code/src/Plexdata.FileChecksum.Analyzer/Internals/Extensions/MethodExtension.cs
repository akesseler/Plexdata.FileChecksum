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
using System.Security.Cryptography;

namespace Plexdata.FileChecksum.Internals.Extensions
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    internal static class MethodExtension
    {
        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        static MethodExtension() { }

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
        /// <seealso cref="MD5.Create()"/>
        /// <seealso cref="SHA1.Create()"/>
        /// <seealso cref="SHA256.Create()"/>
        /// <seealso cref="SHA384.Create()"/>
        /// <seealso cref="SHA512.Create()"/>
        /// <seealso cref="MD5.Create(String)"/>
        /// <seealso cref="SHA1.Create(String)"/>
        /// <seealso cref="SHA256.Create(String)"/>
        /// <seealso cref="SHA384.Create(String)"/>
        /// <seealso cref="SHA512.Create(String)"/>
        /// <exception cref="NotSupportedException">
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        internal static HashAlgorithm Create(this Method method, String algorithm)
        {
            switch (method)
            {
                case Method.Md5:
                    return MethodExtension.CreateMd5HashAlgorithm(algorithm);
                case Method.Sha1:
                    return MethodExtension.CreateSha1HashAlgorithm(algorithm);
                case Method.Sha256:
                    return MethodExtension.CreateSha256HashAlgorithm(algorithm);
                case Method.Sha384:
                    return MethodExtension.CreateSha384HashAlgorithm(algorithm);
                case Method.Sha512:
                    return MethodExtension.CreateSha512HashAlgorithm(algorithm);
                default:
                    throw MethodExtension.GetNotSupportedException(method, algorithm);
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="algorithm">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private static HashAlgorithm CreateMd5HashAlgorithm(String algorithm)
        {
            HashAlgorithm result;

            if (String.IsNullOrWhiteSpace(algorithm))
            {
                result = MD5.Create();
            }
            else
            {
                result = MD5.Create(algorithm);
            }

            if (result is null)
            {
                throw MethodExtension.GetInvalidOperationException(Method.Md5, algorithm);
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="algorithm">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private static HashAlgorithm CreateSha1HashAlgorithm(String algorithm)
        {
            HashAlgorithm result;

            if (String.IsNullOrWhiteSpace(algorithm))
            {
                result = SHA1.Create();
            }
            else
            {
                result = SHA1.Create(algorithm);
            }

            if (result is null)
            {
                throw MethodExtension.GetInvalidOperationException(Method.Sha1, algorithm);
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="algorithm">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private static HashAlgorithm CreateSha256HashAlgorithm(String algorithm)
        {
            HashAlgorithm result;

            if (String.IsNullOrWhiteSpace(algorithm))
            {
                result = SHA256.Create();
            }
            else
            {
                result = SHA256.Create(algorithm);
            }

            if (result is null)
            {
                throw MethodExtension.GetInvalidOperationException(Method.Sha256, algorithm);
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="algorithm">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private static HashAlgorithm CreateSha384HashAlgorithm(String algorithm)
        {
            HashAlgorithm result;

            if (String.IsNullOrWhiteSpace(algorithm))
            {
                result = SHA384.Create();
            }
            else
            {
                result = SHA384.Create(algorithm);
            }

            if (result is null)
            {
                throw MethodExtension.GetInvalidOperationException(Method.Sha384, algorithm);
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="algorithm">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private static HashAlgorithm CreateSha512HashAlgorithm(String algorithm)
        {
            HashAlgorithm result;

            if (String.IsNullOrWhiteSpace(algorithm))
            {
                result = SHA512.Create();
            }
            else
            {
                result = SHA512.Create(algorithm);
            }

            if (result is null)
            {
                throw MethodExtension.GetInvalidOperationException(Method.Sha512, algorithm);
            }

            return result;
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
        /// <seealso cref="Factories.ChecksumCalculatorFactory.Create(Method)"/>
        /// <seealso cref="Factories.ChecksumCalculatorFactory.Create(Method, String)"/>
        private static NotSupportedException GetNotSupportedException(Method method, String algorithm)
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
        /// <seealso cref="CreateMd5HashAlgorithm(String)"/>
        /// <seealso cref="CreateSha1HashAlgorithm(String)"/>
        /// <seealso cref="CreateSha256HashAlgorithm(String)"/>
        /// <seealso cref="CreateSha384HashAlgorithm(String)"/>
        /// <seealso cref="CreateSha512HashAlgorithm(String)"/>
        private static InvalidOperationException GetInvalidOperationException(Method method, String algorithm)
        {
            if (String.IsNullOrWhiteSpace(algorithm))
            {
                return new InvalidOperationException($"Creating a hash algorithm for method `{method}` has failed.");
            }
            else
            {
                return new InvalidOperationException($"Creating a hash algorithm for method `{method} ({algorithm})` has failed.");
            }
        }
    }
}
