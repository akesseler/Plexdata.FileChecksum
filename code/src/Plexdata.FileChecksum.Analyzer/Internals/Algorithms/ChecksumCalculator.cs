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

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Plexdata.FileChecksum.Internals.Algorithms
{
    internal abstract class ChecksumCalculator : IDisposable
    {
        #region Construction

        protected ChecksumCalculator(HashAlgorithm algorithm)
            : base()
        {
            this.Algorithm = algorithm ?? throw new ArgumentNullException(nameof(algorithm));
        }

        ~ChecksumCalculator()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        internal HashAlgorithm Algorithm { get; private set; }

        internal Boolean IsDisposed
        {
            get
            {
                return this.Algorithm is null;
            }
        }

        #endregion

        #region Public methods

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public String Calculate(Stream stream)
        {
            this.ValidateDisposed();
            this.ValidateStream(stream);

            return this.BufferToString(this.Algorithm.ComputeHash(stream));
        }

        public Task<String> CalculateAsync(Stream stream)
        {
            return this.CalculateAsync(stream, CancellationToken.None);
        }

        public Task<String> CalculateAsync(Stream stream, CancellationToken token)
        {
            this.ValidateDisposed();
            this.ValidateStream(stream);

            return this.CalculateInternalAsync(stream, token);
        }

        #endregion

        #region Protected methods

        protected virtual void Dispose(Boolean disposing)
        {
            if (!this.IsDisposed && disposing)
            {
                this.Algorithm.Dispose();
                this.Algorithm = null;
            }
        }

        protected void ValidateDisposed()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        protected void ValidateStream(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new NotSupportedException("Stream does not support reading.");
            }
        }

        protected void ValidateBuffer(Byte[] buffer)
        {
            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }
        }

        #endregion

        #region Private methods

        private async Task<String> CalculateInternalAsync(Stream stream, CancellationToken token)
        {
            // Thanks for this answer: https://stackoverflow.com/questions/27133618/asynchronous-sha256-hashing/48279895#48279895
            try
            {
                Int32 picked = 0;
                Byte[] buffer = new Byte[8192];

                while ((picked = await stream.ReadAsync(buffer, 0, buffer.Length, token)) != 0)
                {
                    if (token.IsCancellationRequested)
                    {
                        return String.Empty;
                    }

                    this.Algorithm.TransformBlock(buffer, 0, picked, buffer, 0);
                }

                if (token.IsCancellationRequested)
                {
                    return String.Empty;
                }

                this.Algorithm.TransformFinalBlock(buffer, 0, picked);

                if (token.IsCancellationRequested)
                {
                    return String.Empty;
                }

                return this.BufferToString(this.Algorithm.Hash);
            }
            catch (TaskCanceledException)
            {
                // This exception may occur in method ReadAsync().
                return String.Empty;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private String BufferToString(Byte[] buffer)
        {
            // Might never happen, but safety first...
            if (buffer is null) { return String.Empty; }

            Char[] result = new Char[buffer.Length * 2];

            for (Int32 index = 0; index < buffer.Length; index++)
            {
                result[(index * 2) + 0] = this.NibbleToCharacter(buffer[index], true);
                result[(index * 2) + 1] = this.NibbleToCharacter(buffer[index], false);
            }

            return new String(result);
        }

        private Char NibbleToCharacter(Byte nibble, Boolean upper)
        {
            switch ((Byte)((upper ? (nibble >> 4) : nibble) & 0x0F))
            {
                case 0x00: return '0';
                case 0x01: return '1';
                case 0x02: return '2';
                case 0x03: return '3';
                case 0x04: return '4';
                case 0x05: return '5';
                case 0x06: return '6';
                case 0x07: return '7';
                case 0x08: return '8';
                case 0x09: return '9';
                case 0x0A: return 'a';
                case 0x0B: return 'b';
                case 0x0C: return 'c';
                case 0x0D: return 'd';
                case 0x0E: return 'e';
                case 0x0F: return 'f';
                default: throw new ArgumentOutOfRangeException(nameof(nibble));
            }
        }

        #endregion
    }
}