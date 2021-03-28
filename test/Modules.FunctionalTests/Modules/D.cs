using System;
using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Services;
using Nandel.Modules;

namespace Modules.FunctionalTests.Modules
{
    public class D : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ServiceD>();
        }
    }
}