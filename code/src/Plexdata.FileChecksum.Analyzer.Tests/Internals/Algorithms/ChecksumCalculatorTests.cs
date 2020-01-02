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
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Plexdata.FileChecksum.Tests.Internals.Algorithms
{
    [TestFixture]
    [TestOf(typeof(ChecksumCalculator))]
    public class ChecksumCalculatorTests
    {
        private Byte[] buffer = null;
        private ChecksumCalculator instance = null;
        private HashAlgorithm algorithm = null;

        [SetUp]
        public void SetUp()
        {
            this.buffer = Encoding.UTF8.GetBytes("Hello, World!");
            this.algorithm = new HashAlgorithmTestClass();
            this.instance = new ChecksumTestClass(this.algorithm);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                this.instance.Dispose();
                this.instance = null;
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }

            try
            {
                this.algorithm.Dispose();
                this.algorithm = null;
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }

        [Test]
        public void Checksum_HashAlgorithmIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new ChecksumTestClass(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Checksum_HashAlgorithmValid_AlgorithmIsAppliedInstance()
        {
            using (ChecksumCalculator current = new ChecksumTestClass(this.algorithm))
            {
                Assert.That(current.Algorithm, Is.InstanceOf<HashAlgorithmTestClass>());
                Assert.That(current.Algorithm, Is.EqualTo(this.algorithm));
            }
        }

        [Test]
        public void IsDisposed_ObjectNotDisposed_IsDisposedIsFalse()
        {
            Assert.That(this.instance.IsDisposed, Is.False);
        }

        [Test]
        public void IsDisposed_ObjectDisposed_IsDisposedIsTrue()
        {
            this.instance.Dispose();

            Assert.That(this.instance.IsDisposed, Is.True);
        }

        [Test]
        public void Dispose_MultipleDisposals_DisposeThrowsNothing([Values(1, 2, 3)] Int32 count)
        {
            for (; count > 0; count--)
            {
                Assert.That(() => this.instance.Dispose(), Throws.Nothing);
            }
        }

        [Test]
        public void Calculate_WithStreamObjectDisposed_ThrowsObjectDisposedException()
        {
            this.instance.Dispose();

            Assert.That(() => this.instance.Calculate((Stream)null), Throws.InstanceOf<ObjectDisposedException>());
        }

        [Test]
        public void Calculate_WithStreamAndStreamIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => this.instance.Calculate((Stream)null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void Calculate_WithStreamAndCanReadIsFalse_ThrowsNotSupportedException()
        {
            Assert.That(() => this.instance.Calculate(new WriteOnlyStream()), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void Calculate_WithStreamAndData_HashAlgorithmCalledIsTrue()
        {
            using (MemoryStream stream = new MemoryStream(this.buffer))
            {
                this.instance.Calculate(stream);

                Assert.That((this.algorithm as HashAlgorithmTestClass).HashAlgorithmCalled, Is.True);
            }
        }

        [Test]
        public void Calculate_WithStreamAndData_HashAlgorithmReturnedExpectedData()
        {
            String expected = BitConverter.ToString(this.buffer).Replace("-", "").ToLower();

            using (MemoryStream stream = new MemoryStream(this.buffer))
            {
                String actual = this.instance.Calculate(stream);

                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        #region Private helpers

        private class ChecksumTestClass : ChecksumCalculator
        {
            public ChecksumTestClass(HashAlgorithm algorithm)
                : base(algorithm)
            { }
        }

        private class HashAlgorithmTestClass : HashAlgorithm
        {
            private Byte[] buffer = null;

            public Int32 InitializeCallCount { get; private set; }

            public Int32 HashCoreCallCount { get; private set; }

            public Int32 HashFinalCallCount { get; private set; }

            public Boolean HashAlgorithmCalled
            {
                get
                {
                    return this.HashCoreCallCount == 1 && this.InitializeCallCount == 1 && this.HashFinalCallCount == 1;
                }
            }

            public override void Initialize()
            {
                this.InitializeCallCount += 1;
            }

            protected override void HashCore(Byte[] array, Int32 ibStart, Int32 cbSize)
            {
                this.HashCoreCallCount += 1;
                this.buffer = new Byte[cbSize];
                Array.Copy(array, ibStart, this.buffer, 0, cbSize);
            }

            protected override Byte[] HashFinal()
            {
                this.HashFinalCallCount += 1;
                return this.buffer;
            }
        }

        private class WriteOnlyStream : Stream
        {
            public override Boolean CanRead
            {
                get
                {
                    return false;
                }
            }

            public override Boolean CanSeek
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override Boolean CanWrite
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override Int64 Length
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override Int64 Position
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
            {
                throw new NotImplementedException();
            }

            public override Int64 Seek(Int64 offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(Int64 value)
            {
                throw new NotImplementedException();
            }

            public override void Write(Byte[] buffer, Int32 offset, Int32 count)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
