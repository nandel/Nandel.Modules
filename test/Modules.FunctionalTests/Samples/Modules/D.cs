using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Samples.Services;
using Nandel.Modules;

namespace Modules.FunctionalTests.Samples.Modules
{
    public class D : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ServiceD>();
        }
    }
}