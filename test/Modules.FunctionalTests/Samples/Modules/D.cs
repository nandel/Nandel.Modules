using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nandel.Modules.FunctionalTests.Samples.Services;

namespace Nandel.Modules.FunctionalTests.Samples.Modules;

public class D : IModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.TryAddTransient<ServiceD>();
    }
}