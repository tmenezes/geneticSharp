# Publishing a New Version of GeneticSharpCore to NuGet

This document describes the steps required to publish a new version of the **GeneticSharpCore** package to [NuGet](https://www.nuget.org/packages/GeneticSharpCore).

---

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed (version 8.0 or later)
- A valid [NuGet API key](https://www.nuget.org/account/apikeys) with push permissions for the `GeneticSharpCore` package
- Access to the repository: https://github.com/tmenezes/geneticSharp

---

## Steps

### 1. Update the Version Number

Open `src/GeneticSharp/GeneticSharp.csproj` and bump the version numbers:

```xml
<AssemblyVersion>X.Y.Z.0</AssemblyVersion>
<Version>X.Y.Z</Version>
```

Follow [Semantic Versioning](https://semver.org/):
- **Patch** (`X.Y.Z` → `X.Y.Z+1`): bug fixes, minor improvements — no breaking changes
- **Minor** (`X.Y.Z` → `X.Y+1.0`): new backwards-compatible features
- **Major** (`X.Y.Z` → `X+1.0.0`): breaking changes

### 2. Build and Test

Ensure all tests pass before packing:

```bash
dotnet build GeneticSharp.sln --configuration Release
dotnet test GeneticSharp.sln --configuration Release
```

### 3. Pack the NuGet Package

Generate the `.nupkg` file:

```bash
dotnet pack src/GeneticSharp/GeneticSharp.csproj --configuration Release --output ./nupkg
```

The package will be created at `./nupkg/GeneticSharpCore.X.Y.Z.nupkg`.

### 4. Publish to NuGet

Push the package using your NuGet API key:

```bash
dotnet nuget push ./nupkg/GeneticSharpCore.X.Y.Z.nupkg \
  --api-key <YOUR_NUGET_API_KEY> \
  --source https://api.nuget.org/v3/index.json
```

> **Note:** Replace `<YOUR_NUGET_API_KEY>` with your actual NuGet API key. Never commit API keys to source control.

### 5. Verify the Publication

After publishing, the new version should appear on NuGet within a few minutes:

https://www.nuget.org/packages/GeneticSharpCore

---

## Quick Reference

| Step | Command |
|------|---------|
| Build | `dotnet build GeneticSharp.sln -c Release` |
| Test  | `dotnet test GeneticSharp.sln -c Release` |
| Pack  | `dotnet pack src/GeneticSharp/GeneticSharp.csproj -c Release -o ./nupkg` |
| Push  | `dotnet nuget push ./nupkg/GeneticSharpCore.X.Y.Z.nupkg --api-key <KEY> --source https://api.nuget.org/v3/index.json` |

---

## Version History

| Version | Notes                  |
|---------|------------------------|
| 1.1.0   | Minor version update   |
| 1.0.1   | Previous release       |
| 1.0.0   | Initial release        |
