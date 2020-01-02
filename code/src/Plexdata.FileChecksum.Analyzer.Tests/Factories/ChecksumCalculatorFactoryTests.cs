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
using Plexdata.FileChecksum.Factories;
using Plexdata.FileChecksum.Interfaces;
using Plexdata.FileChecksum.Internals.Algorithms;
using System;

namespace Plexdata.FileChecksum.Tests.Factories
{
    [TestFixture]
    [TestOf(typeof(ChecksumCalculatorFactory))]
    public class ChecksumCalculatorFactoryTests
    {
        [Test]
        [TestCase(Method.Md5, typeof(Md5ChecksumCalculator))]
        [TestCase(Method.Sha1, typeof(Sha1ChecksumCalculator))]
        [TestCase(Method.Sha256, typeof(Sha256ChecksumCalculator))]
        public void Create_ValidMethod_ResultAsExpected(Method method, Type expected)
        {
            IChecksumCalculator actual = ChecksumCalculatorFactory.Create(method);

            Assert.That(actual, Is.InstanceOf(expected));
        }

        [Test]
        [TestCase(Method.Md5, null, typeof(Md5ChecksumCalculator))]
        [TestCase(Method.Md5, "", typeof(Md5ChecksumCalculator))]
        [TestCase(Method.Md5, "  ", typeof(Md5ChecksumCalculator))]
        [TestCase(Method.Sha1, null, typeof(Sha1ChecksumCalculator))]
        [TestCase(Method.Sha1, "", typeof(Sha1ChecksumCalculator))]
        [TestCase(Method.Sha1, "  ", typeof(Sha1ChecksumCalculator))]
        [TestCase(Method.Sha256, null, typeof(Sha256ChecksumCalculator))]
        [TestCase(Method.Sha256, "", typeof(Sha256ChecksumCalculator))]
        [TestCase(Method.Sha256, "  ", typeof(Sha256ChecksumCalculator))]
        public void Create_ValidMethodInvalidAlgorithm_ResultAsExpected(Method method, String algorithm, Type expected)
        {
            IChecksumCalculator actual = ChecksumCalculatorFactory.Create(method, algorithm);

            Assert.That(actual, Is.InstanceOf(expected));
        }

        [Test]
        [TestCase((Method)(Int32.MinValue))]
        [TestCase((Method)(Int32.MaxValue))]
        public void Create_InvalidMethod_ThrowsNotSupportedException(Method method)
        {
            Assert.That(() => ChecksumCalculatorFactory.Create(method), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        [TestCase((Method)(Int32.MinValue), null)]
        [TestCase((Method)(Int32.MinValue), "")]
        [TestCase((Method)(Int32.MinValue), "  ")]
        [TestCase((Method)(Int32.MaxValue), null)]
        [TestCase((Method)(Int32.MaxValue), "")]
        [TestCase((Method)(Int32.MaxValue), "  ")]
        public void Create_InvalidMethodInvalidAlgorithm_ThrowsNotSupportedException(Method method, String algorithm)
        {
            Assert.That(() => ChecksumCalculatorFactory.Create(method, algorithm), Throws.InstanceOf<NotSupportedException>());
        }
    }
}
