# Changelog

## 10.0.0

### Added
- `IHostApplicationBuilder` overloads for `AddModules`/`AddModule` in `Nandel.Modules.AspNetCore`.
- Test coverage for `Nandel.Modules.AspNetCore` (hosted service, host builder extensions, module lifecycle, custom contract validation).
- NuGet package metadata: description, tags, repository URL, MIT license expression, packaged README.

### Changed
- Target framework upgraded from net9.0 to net10.0; `Microsoft.Extensions.*` packages upgraded to 10.0.x.
- Enabled nullable reference types across all projects.
- Modernized syntax: file-scoped namespaces, primary constructors, collection expressions; public concrete classes are now `sealed`.
- `Helpers` renamed to `HostBuilderExtensions` in `Nandel.Modules.AspNetCore`.
- `ModuleFactory` and `DependencyController` now fail fast with a clear exception instead of silently tolerating a `null` module instance.

### Deprecated
- `IHostBuilder` overloads of `AddModules`/`AddModule` in favor of the `IHostApplicationBuilder` overloads.

### Fixed
- A functional test was asserting against the wrong `IConfiguration` type due to a stray `Castle.Core.Configuration` import.
