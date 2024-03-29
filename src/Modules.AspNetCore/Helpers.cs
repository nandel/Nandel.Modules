﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Nandel.Modules.AspNetCore;

public static class Helpers
{
    /// <summary>
    /// Register modules as hosted services to use IHasStart and IHasStart on aspnet architecture
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddModulesHostedService(this IServiceCollection services)
    {
        return services.AddHostedService<ModulesHostedService>();
    }

    /// <summary>
    /// Add the modules in the current host services
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="moduleTypes"></param>
    /// <returns></returns>
    public static IHostBuilder AddModules(this IHostBuilder builder, params Type[] moduleTypes)
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddRootModule<AspNetCoreRootModule>(new object[]
            {
                context.Configuration,
                context.HostingEnvironment
            });
            
            foreach (var moduleType in moduleTypes)
            {
                services.AddModule(moduleType);
            }

            services.AddModulesHostedService();
        });

        return builder;
    }

    /// <summary>
    /// Add the module in the current host services
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IHostBuilder AddModule<T>(this IHostBuilder builder)
    {
        return AddModules(builder, typeof(T));
    }
}