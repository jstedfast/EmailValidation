//
// AssemblyInfo.cs
//
// Author: Jeffrey Stedfast <jeff@xamarin.com>
//
// Copyright (c) 2013-2017 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Reflection;
using System.Runtime.CompilerServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle ("EmailValidation")]
[assembly: AssemblyDescription ("A simple (but correct) email address validator")]
[assembly: AssemblyConfiguration ("")]
[assembly: AssemblyCompany ("")]
[assembly: AssemblyProduct ("EmailValidation")]
[assembly: AssemblyCopyright ("Copyright © 2013-2017 Jeffrey Stedfast")]
[assembly: AssemblyTrademark ("")]
[assembly: AssemblyCulture ("")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Micro Version
//      Build Number
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
//
// Note: AssemblyVersion is what the CLR matches against at runtime, so be careful
// about updating it. The AssemblyFileVersion is the official release version while
// the AssemblyInformationalVersion is just used as a display version.
//
// Based on my current understanding, AssemblyVersion is essentially the "API Version"
// and so should only be updated when the API changes. The other 2 Version attributes
// represent the "Release Version".
//
// Making releases:
//
// If any classes, methods, or enum values have been added, bump the Micro Version
//    in all version attributes and set the Build Number back to 0.
//
// If there have only been bug fixes, bump the Micro Version and/or the Build Number
//    in the AssemblyFileVersion attribute.
[assembly: AssemblyInformationalVersion ("1.0.2")]
[assembly: AssemblyFileVersion ("1.0.2.0")]
[assembly: AssemblyVersion ("1.0.0.0")]
