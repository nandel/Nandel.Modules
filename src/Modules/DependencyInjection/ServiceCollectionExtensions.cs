using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Nandel.Modules;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRootModule(this IServiceCollection services, Type moduleType, ModuleFactory moduleFactory)
    {
        if (GetRootDependencyNode(services) != null) throw new InvalidOperationException("AddRootModule should be used only once");
        
        var root = new DependencyNode(moduleType, moduleFactory);
        var descriptor = new ServiceDescriptor(typeof(DependencyNode), root);
        services.Add(descriptor);
        
        root.ConfigureServices(services);

        return services;
    }

    public static IServiceCollection AddRootModule<TModule>(this IServiceCollection services, ModuleFactory moduleFactory)
    {
        return AddRootModule(services, typeof(TModule), moduleFactory);
    }

    public static IServiceCollection AddRootModule(this IServiceCollection services, Type moduleType, IEnumerable<object> modulesDependencies)
    {
        return AddRootModule(services, moduleType, new ModuleFactory(modulesDependencies));
    }

    public static IServiceCollection AddRootModule<TModule>(this IServiceCollection services, IEnumerable<object> modulesDependencies)
    {
        return AddRootModule(services, typeof(TModule), modulesDependencies);
    }

    public static IServiceCollection AddRootModule(this IServiceCollection services, Type moduleType)
    {
        return AddRootModule(services, moduleType, ModuleFactory.Default);
    }
    
    public static IServiceCollection AddRootModule<TModule>(this IServiceCollection services)
    {
        return AddRootModule(services, typeof(TModule));
    }
    
    public static IServiceCollection AddModule(this IServiceCollection services, Type moduleType)
    {
        var root = GetRootDependencyNode(services);
        if (root is null)
        {
            throw new InvalidOperationException("Use AddRootModule before adding new modules");
        }
        
        root.AddDependencyNode(moduleType);
        root.ConfigureServices(services);
        
        return services;
    }
    
    public static IServiceCollection AddModule<TModule>(this IServiceCollection services)
    {
        return AddModule(services, typeof(TModule));
    }

    private static DependencyNode GetRootDependencyNode(IServiceCollection services)
    {
        return services.FirstOrDefault(x => x.ServiceType == typeof(DependencyNode))?.ImplementationInstance as DependencyNode;
    }
}