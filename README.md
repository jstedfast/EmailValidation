# EmailValidation

## What is EmailValidation?

A simple (but correct) .NET class for validating email addresses.

Supports Internationalized Mail Address standards (rfc653x).

## Installing via NuGet

The easiest way to install EmailValidation is via [NuGet](https://www.nuget.org/packages/EmailValidation/).

In Visual Studio's [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console),
enter the following command:

    Install-Package EmailValidation

## Getting the Source Code

First, you'll need to clone EmailValidation from my GitHub repository. To do this using the command-line version of
Git, you'll need to issue the following command in your terminal:

    git clone https://github.com/jstedfast/EmailValidation.git

If you are using [TortoiseGit](https://tortoisegit.org) on Windows, you'll need to right-click in the directory where
where you'd like to clone EmailValidation and select **Git Clone...** in the menu. Once you do that, you'll get a
dialog asking you to specify the repository you'd like to clone. In the textbox labeled **URL:**, enter
`https://github.com/jstedfast/EmailValidation.git` and then click **OK**. This will clone EmailValidation onto your
local machine.

## Updating the Source Code

Occasionally you might want to update your local copy of the source code if I have made changes to EmailValidation
since you downloaded the source code in the step above. To do this using the command-line version fo Git, you'll
need to issue the following command in your terminal within the EmailValidation directory:

    git pull

If you are using [TortoiseGit](https://tortoisegit.org) on Windows, you'll need to right-click on the EmailValidation
directory and select **Git Sync...** in the menu. Once you do that, you'll need to click the **Pull** button.

## Building

Open the **EmailValidation.sln** solution file in [Visual Studio](https://www.visualstudio.com/downloads/),
choose the desired build configuration (**Debug** or **Release**) and then build.

Note: The **Release** build will generate the xml API documentation, but the **Debug** build will not.

## Using EmailValidation

This is quite possibly the easiest API ever to use. There is simply 1 class with 1 method.

```csharp
using System;

using EmailValidation;

namespace Example {
    public class Program
    {
        public static void Main ()
        {
            do {
                Console.Write ("Enter an email address: ");

                var input = Console.ReadLine ();
                if (input == null)
                    break;

                input = input.Trim ();
                Console.WriteLine ("{0} is {1}!", input, EmailValidator.Validate (input) ? "valid" : "invalid");
            } while (true);

            Console.WriteLine ();
        }
    }
}
```

## License Information

```
EmailValidation is Copyright (C) 2013-2021 Jeffrey Stedfast and is licensed under the MIT license:

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
```

### Other Implementations:

If you need an implementation in something other than C#, perhaps one of the following ports will be helpful:

- [javascript](https://github.com/azanov/isMailFine) by Pavel Azanov
- [pascal](https://github.com/Xor-el/EmailValidationPascal) by Ugochukwu Mmaduekwe
- [Swift](https://github.com/evanrobertson/EmailValidator) by Evan Robertson
- [Dart](https://github.com/fredeil/email-validator.dart) by Fredrik Eilertsen
