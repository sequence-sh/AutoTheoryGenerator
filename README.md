# .NET Core 3.1 Library

[![pipeline status](https://gitlab.com/reductech/templates/dotnetlibrary/badges/master/pipeline.svg)](https://gitlab.com/reductech/templates/dotnetlibrary/-/commits/master)
[![coverage report](https://gitlab.com/reductech/templates/dotnetlibrary/badges/master/coverage.svg)](https://gitlab.com/reductech/templates/dotnetlibrary/-/commits/master)
[![Gitter](https://badges.gitter.im/reductech/dotnetlibrary.svg)](https://gitter.im/reductech/dotnetlibrary?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

An example of a .NET Core library that uses [NUnit](https://nunit.org) for testing, [Coverlet](https://github.com/tonerdo/coverlet) for code coverage, and GitLab CI.

# Releases

Can be downloaded [here](https://gitlab.com/reductech/templates/dotnetlibrary/-/releases).

# NuGet Feed

Releases, latest builds from the master branch, and any branch builds manually pushed are all pushed to the [Reductech NuGet feed](https://gitlab.com/reductech/nuget). Intructions on how to add this feed available [here](https://gitlab.com/reductech/nuget#dotnet)

```powershell
# To add the latest release:
dotnet add package -s reductech Reductech.Templates.DotNetLibrary
# To add a specific build:
dotnet add package -s reductech -v "0.1.0+f925dae8" Reductech.Templates.DotNetLibrary
```
