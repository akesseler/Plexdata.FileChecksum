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

using Plexdata.ArgumentParser.Extensions;
using Plexdata.FileChecksum.Cli.Extensions;
using Plexdata.FileChecksum.Cli.Helpers;
using Plexdata.FileChecksum.Cli.Models;
using Plexdata.FileChecksum.Constants;
using Plexdata.FileChecksum.Factories;
using Plexdata.FileChecksum.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Plexdata.FileChecksum.Cli
{
    class Program
    {
        private static CancellationTokenSource cancellationTokenSource = null;

        static void Main(String[] args)
        {
            Console.CancelKeyPress += OnCancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            try
            {
                Arguments arguments = new Arguments();
                String[] options = Environment.CommandLine.Extract(true);
                if (options != null && options.Length > 0)
                {
                    arguments.Process(options);
                    arguments.Validate();

                    if (arguments.IsVersion)
                    {
                        Environment.Exit(Program.PrintVersion());
                    }

                    if (!arguments.IsVerify && !arguments.IsCreate || arguments.IsHelp)
                    {
                        Environment.Exit(Program.PrintHelp());
                    }

                    if (arguments.IsVerify && arguments.IsCreate)
                    {
                        Environment.Exit(Program.PrintHelp("Verify and create mode cannot be used together."));
                    }

                    if (arguments.IsVerify)
                    {
                        Environment.Exit(Program.HandleVerify(arguments));
                    }

                    if (arguments.IsCreate)
                    {
                        Environment.Exit(Program.HandleCreate(arguments));
                    }

                    Environment.Exit(ExitCodeHelper.Failure);
                }
                else
                {
                    Environment.Exit(Program.PrintHelp());
                }
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception.Message);
                Environment.Exit(exception.HResult);
            }
        }

        private static void OnCancelKeyPress(Object sender, ConsoleCancelEventArgs args)
        {
            Environment.Exit(ExitCodeHelper.Canceled);
        }

        private static void OnProcessExit(Object sender, EventArgs args)
        {
            try
            {
                if (Program.cancellationTokenSource is null)
                {
                    return;
                }

                Program.cancellationTokenSource.Cancel();
                Program.cancellationTokenSource.Dispose();
                Program.cancellationTokenSource = null;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                // Closing the console window via [X] in debug mode 
                // causes the program to hang for about five seconds. 
                // Ok, this just applies to debug mode.
                Program.WaitConditional();
            }
        }

        private static Int32 HandleVerify(Arguments arguments)
        {
            if (Program.cancellationTokenSource is null)
            {
                Program.cancellationTokenSource = new CancellationTokenSource();
            }

            TaskFactory factory = new TaskFactory(
                Program.cancellationTokenSource.Token,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

            return factory
              .StartNew<Task<Int32>>(() => Program.HandleVerifyAsync(arguments, Program.cancellationTokenSource.Token))
              .Unwrap<Int32>()
              .GetAwaiter()
              .GetResult();
        }

        private static async Task<Int32> HandleVerifyAsync(Arguments arguments, CancellationToken token)
        {
            List<Output> outputs = new List<Output>();

            Method method = arguments.Methods[0];

            using (FileStream stream = File.OpenRead(arguments.File))
            {
                if (!arguments.IsSparse)
                {
                    outputs.Add(new Output("File", arguments.File));
                    outputs.Add(new Output("Hash", arguments.Hash));
                    outputs.Add(new Output("Method", method.ToDisplay()));
                }

                using (IChecksumCalculator calculator = ChecksumCalculatorFactory.Create(method))
                {
                    String checksum = await calculator.CalculateAsync(stream, token);

                    if (token.IsCancellationRequested)
                    {
                        return ExitCodeHelper.Canceled;
                    }

                    if (arguments.Hash.Equals(checksum, StringComparison.OrdinalIgnoreCase))
                    {
                        if (arguments.IsSparse)
                        {
                            outputs.Add(new Output("CONFIRMED", ConsoleColor.Green));
                        }
                        else
                        {
                            outputs.Add(new Output("Result", "CONFIRMED", ConsoleColor.Green));
                        }

                        if (token.IsCancellationRequested)
                        {
                            return ExitCodeHelper.Canceled;
                        }

                        outputs.Print();

                        return ExitCodeHelper.Success;
                    }
                    else
                    {
                        if (arguments.IsSparse)
                        {
                            outputs.Add(new Output("UNCONFIRMED", ConsoleColor.Red));
                        }
                        else
                        {
                            outputs.Add(new Output("Result", "UNCONFIRMED", ConsoleColor.Red));
                            outputs.Add(new Output("Detect", checksum, ConsoleColor.Red));
                        }

                        if (token.IsCancellationRequested)
                        {
                            return ExitCodeHelper.Canceled;
                        }

                        outputs.Print();

                        return ExitCodeHelper.Failure;
                    }
                }
            }
        }

        private static Int32 HandleCreate(Arguments arguments)
        {
            if (Program.cancellationTokenSource is null)
            {
                Program.cancellationTokenSource = new CancellationTokenSource();
            }

            TaskFactory factory = new TaskFactory(
                Program.cancellationTokenSource.Token,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

            return factory
              .StartNew<Task<Int32>>(() => Program.HandleCreateAsync(arguments, Program.cancellationTokenSource.Token))
              .Unwrap<Int32>()
              .GetAwaiter()
              .GetResult();
        }

        private static async Task<Int32> HandleCreateAsync(Arguments arguments, CancellationToken token)
        {
            List<Output> outputs = new List<Output>();

            Program.ExpandWildcards(arguments);

            foreach (String file in arguments.Files)
            {
                using (FileStream stream = File.OpenRead(file))
                {
                    if (!arguments.IsSparse)
                    {
                        outputs.Add(new Output("File", file));
                    }

                    foreach (Method method in arguments.Methods)
                    {
                        using (IChecksumCalculator calculator = ChecksumCalculatorFactory.Create(method))
                        {
                            String checksum = await calculator.CalculateAsync(stream, token);

                            if (token.IsCancellationRequested)
                            {
                                return ExitCodeHelper.Canceled;
                            }

                            if (arguments.IsSparse)
                            {
                                outputs.Add(new Output(method.Format(checksum, file)));
                            }
                            else
                            {
                                outputs.Add(new Output("Method", method.ToDisplay()));
                                outputs.Add(new Output("Result", checksum));
                            }
                        }

                        stream.Position = 0;
                    }
                }
            }

            if (token.IsCancellationRequested)
            {
                return ExitCodeHelper.Canceled;
            }

            outputs.Print();

            return ExitCodeHelper.Success;
        }

        private static void ExpandWildcards(Arguments arguments)
        {
            if (arguments.Files.Any(x => x.IndexOfAny(new Char[] { '*', '?' }) != -1))
            {
                List<String> results = new List<String>();

                foreach (String file in arguments.Files)
                {
                    String path = Path.GetDirectoryName(file);
                    String find = Path.GetFileName(file);

                    if (String.IsNullOrEmpty(path))
                    {
                        path = Directory.GetCurrentDirectory();
                    }

                    String[] expanded = Directory.GetFiles(path, find);

                    if (expanded != null && expanded.Any())
                    {
                        results.AddRange(expanded);
                    }
                }

                if (!results.Any())
                {
                    throw new FileNotFoundException("Could not find any fitting file.");
                }

                arguments.Files = results.Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray();
            }
        }

        private static Int32 PrintVersion()
        {
            FileVersionInfo version = Process.GetCurrentProcess().MainModule.FileVersionInfo;
            Console.WriteLine(version.ProductVersion);
            return ExitCodeHelper.Nothing;
        }

        private static Int32 PrintHelp()
        {
            return Program.PrintHelp(null);
        }

        private static Int32 PrintHelp(String message)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine($"{Environment.NewLine}{message}{Environment.NewLine}");
            }

            Arguments arguments = new Arguments();
            Console.WriteLine(arguments.Generate());

            return ExitCodeHelper.Nothing;
        }

        [Conditional("DEBUG")]
        private static void WaitConditional()
        {
            Console.WriteLine($"Exit: {Environment.ExitCode} (0x{Environment.ExitCode.ToString("X8")})");
            Console.WriteLine(String.Empty);
            Console.Write("Press any key to exit... ");
            Console.ReadKey(true);
        }
    }
}
