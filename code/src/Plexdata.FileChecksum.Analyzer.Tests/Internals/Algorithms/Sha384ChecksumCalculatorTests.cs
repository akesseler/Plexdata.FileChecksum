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
    [TestOf(typeof(Sha384ChecksumCalculator))]
    public class Sha384ChecksumCalculatorTests
    {
        [Test]
        [TestCase(null, typeof(SHA384Managed))]
        [TestCase("", typeof(SHA384Managed))]
        [TestCase("  ", typeof(SHA384Managed))]
        [TestCase("SHA384", typeof(SHA384Managed))]
        [TestCase("System.Security.Cryptography.SHA384Cng", typeof(SHA384Cng))]
        [TestCase("System.Security.Cryptography.SHA384Managed", typeof(SHA384Managed))]
        [TestCase("System.Security.Cryptography.SHA384CryptoServiceProvider", typeof(SHA384CryptoServiceProvider))]
        public void Sha384Checksum_SupportedAlgorithmName_AlgorithmInstanceAsExpected(String algorithm, Type expected)
        {
            Sha384ChecksumCalculator actual = new Sha384ChecksumCalculator(algorithm);

            Assert.That(actual.Algorithm, Is.InstanceOf(expected));
        }

        [Test]
        [TestCase("some-algorithm-name")]
        public void Sha384Checksum_UnsupportedAlgorithmName_ThrowsInvalidOperationException(String algorithm)
        {
            Assert.That(() => new Sha384ChecksumCalculator(algorithm), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}
