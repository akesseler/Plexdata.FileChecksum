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

using Plexdata.ArgumentParser.Attributes;
using Plexdata.ArgumentParser.Constants;
using Plexdata.FileChecksum.Cli.Converters;
using Plexdata.FileChecksum.Constants;
using System;

namespace Plexdata.FileChecksum.Cli.Models
{
    [ParametersGroup]
    [HelpLicense(Placeholders.Copyright)]
    [HelpPreface(Placeholders.Description)]
    [HelpUtilize(Placeholders.Program + " <options> <files>")]
    [HelpUtilize(Heading = "Examples:", Section = "Verify:", Content = Placeholders.Program + " -v -m <method> -h <hash> -f <path>")]
    [HelpUtilize(Heading = "Examples:", Section = "Create:", Content = Placeholders.Program + " -c -m <methods> file1 [file2 .. file(n)]")]
    internal class Arguments
    {
        [HelpSummary("Enables the program's verify mode.")]
        [SwitchParameter(SolidLabel = "verify", BriefLabel = "v", DependencyList = "Hash,File,Methods", DependencyType = DependencyType.Required)]
        public Boolean IsVerify { get; set; }

        [HelpSummary("Enables the program's create mode.")]
        [SwitchParameter(SolidLabel = "create", BriefLabel = "c", DependencyList = "Methods,Files", DependencyType = DependencyType.Required)]
        public Boolean IsCreate { get; set; }

        [HelpSummary("Enables the program's sparse mode.")]
        [SwitchParameter(SolidLabel = "sparse", BriefLabel = "s")]
        public Boolean IsSparse { get; set; }

        [HelpSummary("Comma separated list of checksum methods. Allowed are `md5`, `sha1`, `sha256`, `all` or any combination. But be aware, only the first applied method is used in verify mode.")]
        [OptionParameter(SolidLabel = "methods", BriefLabel = "m", Delimiter = ",")]
        [CustomConverter(typeof(MethodsConverter))]
        public Method[] Methods { get; set; }

        [HelpSummary("Single hash value to be verified. But note, this argument is only allowed along with checksum verification.")]
        [OptionParameter(SolidLabel = "hash", BriefLabel = "h")]
        public String Hash { get; set; }

        [HelpSummary("Fully qualified path of a single file that should be verified. But note, this argument is only allowed along with checksum verification.")]
        [OptionParameter(SolidLabel = "file", BriefLabel = "f")]
        public String File { get; set; }

        [HelpSummary("Shows this help screen.")]
        [SwitchParameter(SolidLabel = "help", BriefLabel = "?")]
        public Boolean IsHelp { get; set; }

        [HelpSummary(
            Options = "<files>",
            Content = "List of fully qualified file names (separated by spaces) for which checksums should be created. But note, " +
                      "this argument is only allowed along with checksum creation. Wildcards (such as '*' and '?') are supported.")]
        [VerbalParameter]
        public String[] Files { get; set; }
    }
}
