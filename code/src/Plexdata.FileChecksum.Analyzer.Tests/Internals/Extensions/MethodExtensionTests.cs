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

using NUnit.Framework;
using Plexdata.FileChecksum.Constants;
using Plexdata.FileChecksum.Internals.Extensions;
using System;
using System.Security.Cryptography;

namespace Plexdata.FileChecksum.Tests.Internals.Extensions
{
    [TestFixture]
    [TestOf(typeof(MethodExtension))]
    public class MethodExtensionTests
    {
        [Test]
        [TestCase(Method.Md5, null, typeof(MD5CryptoServiceProvider))]
        [TestCase(Method.Md5, "", typeof(MD5CryptoServiceProvider))]
        [TestCase(Method.Md5, "  ", typeof(MD5CryptoServiceProvider))]
        [TestCase(Method.Sha1, null, typeof(SHA1CryptoServiceProvider))]
        [TestCase(Method.Sha1, "", typeof(SHA1CryptoServiceProvider))]
        [TestCase(Method.Sha1, "  ", typeof(SHA1CryptoServiceProvider))]
        [TestCase(Method.Sha256, null, typeof(SHA256Managed))]
        [TestCase(Method.Sha256, "", typeof(SHA256Managed))]
        [TestCase(Method.Sha256, "  ", typeof(SHA256Managed))]
        [TestCase(Method.Sha384, null, typeof(SHA384Managed))]
        [TestCase(Method.Sha384, "", typeof(SHA384Managed))]
        [TestCase(Method.Sha384, "  ", typeof(SHA384Managed))]
        [TestCase(Method.Sha512, null, typeof(SHA512Managed))]
        [TestCase(Method.Sha512, "", typeof(SHA512Managed))]
        [TestCase(Method.Sha512, "  ", typeof(SHA512Managed))]
        public void Create_ValidMethodInvalidAlgorithm_ResultAsExpected(Method method, String algorithm, Type expected)
        {
            HashAlgorithm actual = method.Create(algorithm);

            Assert.That(actual, Is.InstanceOf(expected));
        }

        [Test]
        [TestCase(Method.Md5, "MD5", typeof(MD5CryptoServiceProvider))]
        [TestCase(Method.Md5, "System.Security.Cryptography.MD5", typeof(MD5CryptoServiceProvider))]
        [TestCase(Method.Md5, "System.Security.Cryptography.MD5Cng", typeof(MD5Cng))]
        [TestCase(Method.Md5, "System.Security.Cryptography.MD5CryptoServiceProvider", typeof(MD5CryptoServiceProvider))]
        [TestCase(Method.Sha1, "SHA1", typeof(SHA1CryptoServiceProvider))]
        [TestCase(Method.Sha1, "System.Security.Cryptography.SHA1Cng", typeof(SHA1Cng))]
        [TestCase(Method.Sha1, "System.Security.Cryptography.SHA1Managed", typeof(SHA1Managed))]
        [TestCase(Method.Sha1, "System.Security.Cryptography.SHA1CryptoServiceProvider", typeof(SHA1CryptoServiceProvider))]
        [TestCase(Method.Sha256, "SHA256", typeof(SHA256Managed))]
        [TestCase(Method.Sha256, "System.Security.Cryptography.SHA256Cng", typeof(SHA256Cng))]
        [TestCase(Method.Sha256, "System.Security.Cryptography.SHA256Managed", typeof(SHA256Managed))]
        [TestCase(Method.Sha256, "System.Security.Cryptography.SHA256CryptoServiceProvider", typeof(SHA256CryptoServiceProvider))]
        [TestCase(Method.Sha384, "SHA384", typeof(SHA384Managed))]
        [TestCase(Method.Sha384, "System.Security.Cryptography.SHA384Cng", typeof(SHA384Cng))]
        [TestCase(Method.Sha384, "System.Security.Cryptography.SHA384Managed", typeof(SHA384Managed))]
        [TestCase(Method.Sha384, "System.Security.Cryptography.SHA384CryptoServiceProvider", typeof(SHA384CryptoServiceProvider))]
        [TestCase(Method.Sha512, "SHA512", typeof(SHA512Managed))]
        [TestCase(Method.Sha512, "System.Security.Cryptography.SHA512Cng", typeof(SHA512Cng))]
        [TestCase(Method.Sha512, "System.Security.Cryptography.SHA512Managed", typeof(SHA512Managed))]
        [TestCase(Method.Sha512, "System.Security.Cryptography.SHA512CryptoServiceProvider", typeof(SHA512CryptoServiceProvider))]
        public void Create_ValidMethodValidAlgorithm_ResultAsExpected(Method method, String algorithm, Type expected)
        {
            Assert.That(method.Create(algorithm), Is.InstanceOf(expected));
        }

        [Test]
        [TestCase((Method)(Int32.MinValue), null)]
        [TestCase((Method)(Int32.MinValue), "")]
        [TestCase((Method)(Int32.MinValue), "  ")]
        [TestCase((Method)(Int32.MinValue), "some-algorithm-name")]
        [TestCase((Method)(Int32.MaxValue), null)]
        [TestCase((Method)(Int32.MaxValue), "")]
        [TestCase((Method)(Int32.MaxValue), "  ")]
        [TestCase((Method)(Int32.MaxValue), "some-algorithm-name")]
        public void Create_InvalidMethodInvalidAlgorithm_ThrowsNotSupportedException(Method method, String algorithm)
        {
            Assert.That(() => method.Create(algorithm), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        [TestCase(Method.Md5, "some-algorithm-name")]
        [TestCase(Method.Md5, "System.Security.Cryptography.MD5Managed")] // See notes...
        [TestCase(Method.Sha1, "some-algorithm-name")]
        [TestCase(Method.Sha256, "some-algorithm-name")]
        [TestCase(Method.Sha384, "some-algorithm-name")]
        [TestCase(Method.Sha512, "some-algorithm-name")]
        public void Create_ValidMethodWrongAlgorithm_ThrowsInvalidOperationException(Method method, String algorithm)
        {
            // NOTE: Notes about this integration test.
            // * According to documentation, method "MD5.Create(String)" should accept 
            //   name "System.Security.Cryptography.MD5Managed", but actually it doesn't. 
            Assert.That(() => method.Create(algorithm), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}
