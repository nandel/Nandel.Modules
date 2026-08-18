using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Nandel.Modules.AspNetCore;

public static class HostBuilderExtensions
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
    [Obsolete("Use the IHostApplicationBuilder overload instead.")]
    public static IHostBuilder AddModules(this IHostBuilder builder, params Type[] moduleTypes)
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddRootModule<AspNetCoreRootModule>([context.Configuration, context.HostingEnvironment]);

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
    [Obsolete("Use the IHostApplicationBuilder overload instead.")]
    public static IHostBuilder AddModule<T>(this IHostBuilder builder)
    {
        return AddModules(builder, typeof(T));
    }

    public static IHostApplicationBuilder AddModules(this IHostApplicationBuilder builder, params Type[] moduleTypes)
    {
        builder.Services.AddRootModule<AspNetCoreRootModule>([builder.Configuration, builder.Environment]);

        foreach (var moduleType in moduleTypes)
        {
            builder.Services.AddModule(moduleType);
        }

        builder.Services.AddModulesHostedService();

        return builder;
    }

    public static IHostApplicationBuilder AddModule<T>(this IHostApplicationBuilder builder)
    {
        return AddModules(builder, typeof(T));
    }
}