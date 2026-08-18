# Nandel.Modules

This is a dotnet solution that adds an abstraction layer of modules on top of the default Microsoft dependency
injection. It becomes easier to manage and register dependencies between modules.

## Installation

Run `Install-Package Nandel.Modules` in your Nuget console, then also install `Install-Package Nandel.Modules.AspNetCore`
for ASP.NET Core support and extra features.

## Getting Started

Below we have 3 modules defined: `A` depends on `B` and `C`, and `B` depends on `C`. After we register module `A` in
the `IServiceCollection` we register all of its dependencies, checking that we haven't already done so, since `C` is
a dependency of both `A` and `B`.

```csharp
using Nandel.Modules;

[DependsOn(
    typeof(B),
    typeof(C)
)]
class A : IModule, IHasStart, IHasStop
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Here comes module A registration
    }

    public Task StartAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        Console.WriteLine("Module A has been started");
        return Task.CompletedTask;
    }

    public Task StopAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        Console.WriteLine("Module A has been stopped");
        return Task.CompletedTask;
    }
}

[DependsOn(typeof(C))]
class B : IModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Here comes module B registration
    }
}

class C : IModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Here comes module C registration
    }
}
```

Now all we have to do is invoke `AddRootModule<A>()` to register the module and its entire dependency tree in the
current `IServiceCollection`:

```csharp
using Nandel.Modules;

class Startup
{
    void ConfigureServices(IServiceCollection services)
    {
        services.AddRootModule<A>();
    }
}
```

`AddRootModule` can only be called once per `IServiceCollection`. Use `AddModule<T>()` afterwards to register further
modules against the same root — this is also how a module can pull in additional modules from inside its own
`ConfigureServices`.

## ASP.NET Core / Generic Host integration

Install `Nandel.Modules.AspNetCore` to drive `IHasStart`/`IHasStop` from the host lifecycle and register the root
module in a single call:

```csharp
using Nandel.Modules.AspNetCore;

var builder = Host.CreateApplicationBuilder(args);
builder.AddModule<A>();

var host = builder.Build();
host.Run();
```

`AddModule<T>`/`AddModules(...)` register the module tree, wire up `IConfiguration`/`IHostEnvironment` for constructor
injection, and call `services.AddModulesHostedService()` for you, which runs `IHasStart`/`IHasStop` as an
`IHostedService`. The equivalent `IHostBuilder` overloads (`Host.CreateDefaultBuilder()`) still work but are marked
`[Obsolete]` in favor of the `IHostApplicationBuilder` ones above.

## Invoking a custom contract across modules

`InvokeModulesContract<T>` calls one method on every module that implements a given interface `T`, in dependency
order:

```csharp
public interface IOnReady
{
    void OnReady();
}

serviceProvider.InvokeModulesContract<IOnReady>();
```

`T` must declare exactly one method — `InvokeModulesContract` throws `InvalidOperationException` otherwise.

## Trimming and Native AOT

The module and dependency resolution engine relies on reflection (`Activator.CreateInstance`, constructor and
attribute inspection). It is not annotated for trimming and is **not compatible** with `PublishTrimmed` or
`PublishAot`.
