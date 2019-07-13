![XPress Logo](/XPress_logo.png)
# XPress
**XPress** is a simple yet powerful expression language for .net. Create string expression in compile time using binary, unary, relational, and conditional operands; compile it to an efficient boolean `Func` can be evaluated to a boolean value. Xpress supports numerical, text, boolean, null, and variables. 


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

### Prerequisites
Download and Install Visual Studio 2017 Community Edition, and .Net 4.7  or higher.

### Get Source Code
- Clone `XPress` source Code to your local dev box
- Open `Eknowledger.Language.Xpress` solution in Visual Studio
- Start hacking! 
- Add Unit Tests
- Run Build Scripts locally
- Submit a `pull request`

### How Do I Build Solution?

#### Debug Build
Run `debug` script in powershell cmd window with admin privilages, to compile and run unit tests without code coverage report or packaging

``` powershell
.\debug.ps1
``` 

#### Debug Build with Coverage
Run `debug-cover` script in powershell cmd window with admin privilages, to compile and run unit tests with code coverage report

``` powershell
.\debug-cover.ps1
```

#### Release Build
Run following `release` script in powershell cmd window with admin privilages

``` powershell
.\release.ps1
```

### How do I Run Tests?
[Eknowledger.Language.Xpress.Test](/src/Eknowledger.Language.Xpress.Test/) contains extended list of unit tests covering many cases of **XPress** expressions. You can run unit tests using Visual Studio test explorer, or in command line using `debug.ps1` script

## Built With
- [Irony](https://github.com/IronyProject/) - .NET Language Implementation Kit
- [xUnit](https://github.com/xunit/xunit) - Unit testing Framework
- [psake](https://github.com/psake/psake) - build automation tool
- [Appveyor](https://ci.appveyor.com/project/eknowledger/xpress) - CI

## Versioning
We use [SemVer](https://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/eknowledger/XPress/tags).

## License
XPress is Copyright &copy; 2019 [Ahmed Elmalt](http://www.eknowledger.com/) under the [MIT license](https://raw.githubusercontent.com/eknowledger/XPress/master/LICENSE).
