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

namespace Plexdata.FileChecksum.Cli.Helpers
{
    internal static class ExitCodeHelper
    {
        #region Constants

        // ---------------------------------------------------------------------
        // Comments have been taken from file "winerror.h".
        // ---------------------------------------------------------------------
        // 
        //  HRESULTs are 32 bit values laid out as follows:
        //
        //   3 3 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1
        //   1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0
        //  +-+-+-+-+-+---------------------+-------------------------------+
        //  |S|R|C|N|r|    Facility         |               Code            |
        //  +-+-+-+-+-+---------------------+-------------------------------+
        //
        //  where
        //
        //      S - Severity - indicates success/fail
        //
        //          0 - Success
        //          1 - Fail (COERROR)
        //
        //      R - reserved portion of the facility code, corresponds to NT's
        //              second severity bit.
        //
        //      C - reserved portion of the facility code, corresponds to NT's
        //              C field.
        //
        //      N - reserved portion of the facility code. Used to indicate a
        //              mapped NT status value.
        //
        //      r - reserved portion of the facility code. Reserved for internal
        //              use. Used to indicate HRESULT values that are not status
        //              values, but are instead message ids for display strings.
        //
        //      Facility - is the facility code
        //
        //      Code - is the facility's status code
        // ---------------------------------------------------------------------

        private const Int32 SEVERITY_SUCCESS = 0;
        private const Int32 SEVERITY_ERROR = 1;

        private const Int32 FACILITY_NULL = 0;

        private const Int32 CODE_SUCCESS = 0;
        private const Int32 CODE_FAILURE = 1;

        #endregion

        #region Construction

        static ExitCodeHelper() { }

        #endregion

        #region Properties

        public static Int32 Success
        {
            get
            {
                return ExitCodeHelper.MakeHResult(ExitCodeHelper.SEVERITY_SUCCESS, ExitCodeHelper.FACILITY_NULL, ExitCodeHelper.CODE_SUCCESS);
            }
        }

        public static Int32 Nothing
        {
            get
            {
                return ExitCodeHelper.MakeHResult(ExitCodeHelper.SEVERITY_SUCCESS, ExitCodeHelper.FACILITY_NULL, ExitCodeHelper.CODE_FAILURE);
            }
        }

        public static Int32 Failure
        {
            get
            {
                return ExitCodeHelper.MakeHResult(ExitCodeHelper.SEVERITY_ERROR, ExitCodeHelper.FACILITY_NULL, ExitCodeHelper.CODE_FAILURE);
            }
        }

        public static Int32 Canceled
        {
            get
            {
                // COR_E_OPERATIONCANCELED: -2146233029 (0x8013153B), S=1, F=19, C=5435
                return (new OperationCanceledException()).HResult;
            }
        }

        #endregion

        #region Methods

        private static Int32 MakeHResult(Int32 severity, Int32 facility, Int32 code)
        {
            return (Int32)((((UInt32)severity) << 31) | (((UInt32)facility) << 16) | ((UInt32)code));
        }

        #endregion
    }
}
