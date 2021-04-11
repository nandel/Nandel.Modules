# Nandel.Modules

This is a dotnet solution adds an abstraction layer of modules on top of default microsoft dependecy injection. 
So it becomes easyer to manage and register dependencies between modules

## Instalation

run `Install-Package Nandel.Modules` in your Nuget console then also install `Install-Package Nandel.Modules.AspNetCore` for AspNetCore support for extra features.

## Getting Started

We can see below we have 3 modules defined, a `A` depends on `B` and `C`, and `B` depends on `C`, in this example after 
we register the module `A` in the `IServiceCollection` we will be registering all the dependencies with the with 
the addition that we are actualy checking if we aready haven'd done it yet since `C` is a dependency of `A` and `B`.

```csharp
using Nandel.Modules;

[DependsOn(
    typeof(B),
    typeof(C),
    )]
class A : IModule, IHasStart, IHasStop
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Here comes module A registration
    }

    public Task StartAsync(IServiceProvider services)
    {
        Console.WriteLine("Module A has been Started");
        return Task.CompletedTask;
    }
    
    public Task StopAsync(IServiceProvider services)
    {
        Console.WriteLine("Module A has been Stoped");
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
        // Here comes module B registration
    }
}
```

Now all we have to do is invoke `AddModule<ModuleTye>()` to register the module and its all deendency tree in the current `IServiceCollection`. 
We can see i also added a `services.AddModulesHostedService()` that is part of `Nandel.Modules.AspNetCore` that will manage the `IHasStart` and `IHasStop` signatures
in all dependency tree and run then in background as instance of `IHostedService`.

```csharp
using Nandel.Modules;
using Nandel.Modules.AspNetCore;

class Startup
{
    void ConfigureServices(IServiceCollection services)
    {
        services.AddModule<A>();
        services.AddModulesHostedService();
    }
}
```

