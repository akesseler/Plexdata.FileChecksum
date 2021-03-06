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

using NUnit.Framework;
using Plexdata.FileChecksum.Internals.Algorithms;
using System;
using System.Security.Cryptography;

namespace Plexdata.FileChecksum.Tests.Internals.Algorithms
{
    [TestFixture]
    [TestOf(typeof(Sha1ChecksumCalculator))]
    public class Sha1ChecksumCalculatorTests
    {
        [Test]
        [TestCase(null, typeof(SHA1CryptoServiceProvider))]
        [TestCase("", typeof(SHA1CryptoServiceProvider))]
        [TestCase("  ", typeof(SHA1CryptoServiceProvider))]
        [TestCase("SHA1", typeof(SHA1CryptoServiceProvider))]
        [TestCase("System.Security.Cryptography.SHA1Cng", typeof(SHA1Cng))]
        [TestCase("System.Security.Cryptography.SHA1Managed", typeof(SHA1Managed))]
        [TestCase("System.Security.Cryptography.SHA1CryptoServiceProvider", typeof(SHA1CryptoServiceProvider))]
        public void Sha1Checksum_SupportedAlgorithmName_AlgorithmInstanceAsExpected(String algorithm, Type expected)
        {
            Sha1ChecksumCalculator actual = new Sha1ChecksumCalculator(algorithm);

            Assert.That(actual.Algorithm, Is.InstanceOf(expected));
        }

        [Test]
        [TestCase("some-algorithm-name")]
        public void Sha1Checksum_UnsupportedAlgorithmName_ThrowsInvalidOperationException(String algorithm)
        {
            Assert.That(() => new Sha1ChecksumCalculator(algorithm), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}
