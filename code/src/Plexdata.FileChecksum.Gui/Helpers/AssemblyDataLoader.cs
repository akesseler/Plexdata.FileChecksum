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
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

namespace Plexdata.FileChecksum.Gui.Helpers
{
    internal static class AssemblyDataLoader
    {
        public static AssemblyData Load(Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            return new AssemblyData
            {
                Title = AssemblyDataLoader.GetAssemblyAttribute<AssemblyTitleAttribute>(assembly)?.Title,
                Description = AssemblyDataLoader.GetAssemblyAttribute<AssemblyDescriptionAttribute>(assembly)?.Description,
                Configuration = AssemblyDataLoader.GetAssemblyAttribute<AssemblyConfigurationAttribute>(assembly)?.Configuration,
                Company = AssemblyDataLoader.GetAssemblyAttribute<AssemblyCompanyAttribute>(assembly)?.Company,
                Product = AssemblyDataLoader.GetAssemblyAttribute<AssemblyProductAttribute>(assembly)?.Product,
                Copyright = AssemblyDataLoader.GetAssemblyAttribute<AssemblyCopyrightAttribute>(assembly)?.Copyright,
                Trademark = AssemblyDataLoader.GetAssemblyAttribute<AssemblyTrademarkAttribute>(assembly)?.Trademark,
                Version = AssemblyDataLoader.GetAssemblyAttribute<AssemblyFileVersionAttribute>(assembly)?.Version,
                Guid = AssemblyDataLoader.GetAssemblyAttribute<GuidAttribute>(assembly)?.Value,
                Culture = AssemblyDataLoader.GetAssemblyAttribute<AssemblyCultureAttribute>(assembly)?.Culture,
                AssemblyVersion = assembly.GetName().Version.ToString(),
                NeutralLanguage = AssemblyDataLoader.GetAssemblyAttribute<NeutralResourcesLanguageAttribute>(assembly)?.CultureName,
                IsComVisible = AssemblyDataLoader.GetAssemblyAttribute<ComVisibleAttribute>(assembly)?.Value
            };
        }

        private static TAttribute GetAssemblyAttribute<TAttribute>(Assembly assembly) where TAttribute : Attribute
        {
            Object[] attributes = assembly.GetCustomAttributes(typeof(TAttribute), true);

            if ((attributes is null) || (attributes.Length == 0))
            {
                return null;
            }

            return (TAttribute)attributes[0];
        }
    }
}
