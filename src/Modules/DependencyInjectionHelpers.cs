using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Nandel.Modules
{
    public static class DependencyInjectionHelpers
    {
        public static IServiceCollection AddModules(this IServiceCollection services, params Type[] modules)
        {
            var tree = new DependencyTree(modules);
            tree.RegisterServices(services);
            
            services.AddSingleton<IDependencyNode>(tree);

            return services;
        }

        public static IServiceProvider InitializeModules(this IServiceProvider services)
        {
            var nodes = services.GetServices<IDependencyNode>();
            if (nodes?.Any() == true)
            {
                foreach (var node in nodes)
                {
                    node.Initialize(services);
                }
            }

            return services;
        }
    }
}