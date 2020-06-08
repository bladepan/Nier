Nier.Commons
----------------------------------------------------------------------
Dotnet utility classes usually end up in folders named like "Utilities", "Utils", "Extensions" or "Commons" in various projects. This project intends to provide a set of core libraries for dotnet. It is similar to [Apache Commons](https://commons.apache.org) and [Guava](https://github.com/google/guava) for Java projects. Many classes and methods are ported from these 2 projects. There are also code written to address the needs specific to dotnet projects as well.

![Release](https://github.com/bladepan/Nier/workflows/Release/badge.svg)

# Install
- [Nuget](https://www.nuget.org/packages/Nier.Commons/)

# API Doc
- [API doc](https://bladepan.github.io/Nier/api/Nier.Commons.html)

# Notable features 
- Null safe methods. Most of the extension methods are null safe.
- Readable ToString for Collections. `ToString` methods in Collection types in dotnet does not have collection values. `ToReadableString` methods in extension classes creates string with collection values.
