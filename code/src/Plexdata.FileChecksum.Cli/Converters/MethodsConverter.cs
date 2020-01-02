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

using Plexdata.ArgumentParser.Interfaces;
using Plexdata.FileChecksum.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plexdata.FileChecksum.Cli.Converters
{
    public class MethodsConverter : ICustomConverter<Method[]>
    {
        public Method[] Convert(String parameter, String argument, String delimiter)
        {
            if (String.IsNullOrWhiteSpace(argument))
            {
                return new Method[] { Method.Md5 };
            }

            if (argument.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                return Enum.GetValues(typeof(Method)).Cast<Method>().ToArray();
            }

            List<Method> results = new List<Method>();
            Method[] methods = Enum.GetValues(typeof(Method)).Cast<Method>().ToArray();

            foreach (String current in argument.Split(delimiter))
            {
                results.Add(methods.FirstOrDefault(x => x.ToString().Equals(current, StringComparison.OrdinalIgnoreCase)));
            }

            return results.ToArray();
        }
    }
}
