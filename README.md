![XPress Logo](/XPress_logo.png)
# XPress
**XPress** is a simple yet powerful expression language for .net string literals. Make string truth expressions in compile time using `binary`, `unary`, `relational`, and `conditional` operators. _XPress_ compiles expressions into an optimized `Func<XpressRuntimeContext, bool>` where it can be evaluated in runtime using variable values. _XPress_ supports `numerical`, `text`, `boolean`, `null`, and `variable` operands.


[![Build status](https://ci.appveyor.com/api/projects/status/pw0v9jpcq7sxyxol?svg=true)](https://ci.appveyor.com/project/eknowledger/xpress) [![codecov](https://codecov.io/gh/eknowledger/XPress/branch/master/graph/badge.svg)](https://codecov.io/gh/eknowledger/XPress) [![NuGet](https://img.shields.io/nuget/dt/XPress.svg)](https://www.nuget.org/packages/XPress) [![NuGet](https://img.shields.io/nuget/v/XPress.svg?color=blue)](https://www.nuget.org/packages/XPress) [![License](https://img.shields.io/github/license/eknowledger/XPress.svg)](https://raw.githubusercontent.com/eknowledger/XPress/master/LICENSE) 


## How do I get started?
First, instaniate `XpressCompiler` object, create Xpress string expression, compile and you're done!

```csharp

XpressCompiler _compiler = XpressCompiler.Default;

// Create and Compile Expression
var expression = "(x ne null and x+1 gt 10) or (y ne null and (y*(5+1)-2) lt 5)";
var compilationResult = _compiler.Compile(code);

// Create XpressRuntimeContext object with variable names and values
XpressRuntimeContext runtimeCtx = new XpressRuntimeContext() { { "x", "10" }, { "y", "9" } };

// Call compiled Func with runtimecontext
var result = compilationResult.Code(runtimeCtx);

```

More examples in the [wiki](https://github.com/eknowledger/XPress/wiki).

### Where can I get it?

First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [XPress](https://www.nuget.org/packages/XPress/) from the package manager console:

```
PM> Install-Package XPress
```

## How can I contribute?

### What do I need?
Download and Install **Visual Studio 2017** Community/Professional/Enterprise Edition, and **.Net 4.7**  or higher.

### How do I get source code?
- Clone **XPress** source Code to your local dev box
```bash
git clone --recursive https://github.com/eknowledger/XPress.git
```
- Open [Eknowledger.Language.Xpress](https://github.com/eknowledger/XPress/blob/master/Eknowledger.Language.Xpress.sln) solution in Visual Studio
- Start hacking!
- Add Unit Tests.
- Run Build Scripts locally.
- Commit your changes
- Submit a **Pull Request** and i will try to review your changes as oon as possible.

> If you'd like to contribute but don't have any particular features in mind to work on, check out [issue tracker](https://github.com/eknowledger/XPress/issues) and look for something that might pique your interest!

### How Do I build solution?

#### Debug Build
Run `debug` script in powershell cmd window with admin privilages, to compile and run unit tests without code coverage report or packaging.

```powershell
.\debug.ps1
``` 

#### Debug Build with Coverage
Run `debug-cover` script in powershell cmd window with admin privilages, to compile and run unit tests with code coverage report.

```powershell
.\debug-cover.ps1
```

#### Release Build
Run `release` script in powershell cmd window with admin privilages.

```powershell
.\release.ps1
```

### How do I Run Tests?
[Eknowledger.Language.Xpress.Test](/src/Eknowledger.Language.Xpress.Test/) contains extended list of unit tests covering many cases of **XPress** expressions. You can run unit tests using Visual Studio test explorer, or in command line using `debug.ps1` script

### How do i report a bug or a feature request?
[Open a new issue](https://github.com/eknowledger/XPress/issues). Provide details and code samples. Before opening a new issue, please search for existing issues to avoid submitting duplicates. You can always roll up your sleeves and [contribute](https://github.com/eknowledger/XPress#how-can-i-contribute) a fix!

## Built With
- [Irony](https://github.com/IronyProject/) - .NET Language Implementation Kit
- [xUnit](https://github.com/xunit/xunit) - Unit testing Framework
- [psake](https://github.com/psake/psake) - build automation tool
- [OpenCover](https://github.com/OpenCover/opencover) - Code Coverage Anlaysis Framework
- [Appveyor](https://ci.appveyor.com/project/eknowledger/xpress) - Build and Continious Integration
- [Codecov](https://codecov.io/gh/eknowledger/XPress) - Code Coverage Reporting Tool



## Versioning
_XPress_ uses [SemVer](https://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/eknowledger/XPress/tags).

## License
XPress is Copyright &copy; 2019 [Ahmed Elmalt](http://www.eknowledger.com/) under the [MIT license](https://raw.githubusercontent.com/eknowledger/XPress/master/LICENSE).
