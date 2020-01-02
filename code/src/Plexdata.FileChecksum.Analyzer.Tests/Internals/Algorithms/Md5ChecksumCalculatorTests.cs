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
using Plexdata.FileChecksum.Internals.Algorithms;
using System;
using System.Security.Cryptography;

namespace Plexdata.FileChecksum.Tests.Internals.Algorithms
{
    [TestFixture]
    [TestOf(typeof(Md5ChecksumCalculator))]
    public class Md5ChecksumCalculatorTests
    {
        [Test]
        [TestCase(null, typeof(MD5CryptoServiceProvider))]
        [TestCase("", typeof(MD5CryptoServiceProvider))]
        [TestCase("  ", typeof(MD5CryptoServiceProvider))]
        [TestCase("MD5", typeof(MD5CryptoServiceProvider))]
        [TestCase("System.Security.Cryptography.MD5", typeof(MD5CryptoServiceProvider))]
        [TestCase("System.Security.Cryptography.MD5Cng", typeof(MD5Cng))]
        [TestCase("System.Security.Cryptography.MD5CryptoServiceProvider", typeof(MD5CryptoServiceProvider))]
        public void Md5Checksum_SupportedAlgorithmName_AlgorithmInstanceAsExpected(String algorithm, Type expected)
        {
            Md5ChecksumCalculator actual = new Md5ChecksumCalculator(algorithm);

            Assert.That(actual.Algorithm, Is.InstanceOf(expected));
        }

        [Test]
        [TestCase("some-algorithm-name")]
        [TestCase("System.Security.Cryptography.MD5Managed")] // See notes...
        public void Md5Checksum_UnsupportedAlgorithmName_ThrowsInvalidOperationException(String algorithm)
        {
            // NOTE: Notes about this integration test.
            // * According to documentation, method "MD5.Create(String)" should accept 
            //   name "System.Security.Cryptography.MD5Managed", but actually it doesn't. 

            Assert.That(() => new Md5ChecksumCalculator(algorithm), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}
