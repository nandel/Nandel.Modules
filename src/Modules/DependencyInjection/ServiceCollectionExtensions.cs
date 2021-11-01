using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Nandel.Modules
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add modules to current ServiceCollection
        /// </summary>
        /// <param name="services">Collection of services</param>
        /// <param name="factory">Factory used to create the instance of modules</param>
        /// <param name="moduleTypes">Collection of classes that implements IModule</param>
        /// <returns></returns>
        private static IServiceCollection AddModules(this IServiceCollection services, ModuleFactory factory, params Type[] moduleTypes)
        {
            var dependencies = (DependencyTree) services
                .FirstOrDefault(x => x.ServiceType == typeof(IDependencyNode))
                ?.ImplementationInstance
                ;

            if (dependencies is null)
            {
                dependencies = new DependencyTree(factory);                                
                services.AddSingleton<IDependencyNode>(dependencies); // add first to re-use the same ModuleFactory instance  
            }

            dependencies.AddRange(moduleTypes);
            dependencies.ConfigureServices(services);
            
            return services;
        }

        /// <summary>
        /// Add modules to current ServiceCollection using a options configurer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurer"></param>
        /// <returns></returns>
        public static IServiceCollection AddModules(this IServiceCollection services, Action<AddModuleOptions> configurer)
        {
            var options = new AddModuleOptions();
            configurer.Invoke(options);

            return AddModules(services, new ModuleFactory(options.Services), options.Modules.ToArray());
        }

        /// <summary>
        /// Add a module to current ServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IServiceCollection AddModule<T>(this IServiceCollection services)
        {
            return AddModules(services, ModuleFactory.Default, typeof(T));
        }

        /// <summary>
        /// Add a module to current ServiceCollection using some other services to create the ModuleInstances
        /// </summary>
        /// <param name="services"></param>
        /// <param name="factoryServices"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IServiceCollection AddModule<T>(this IServiceCollection services, IEnumerable<object> factoryServices)
        {
            return AddModules(services, new ModuleFactory(factoryServices), typeof(T));
        }
    }
}
