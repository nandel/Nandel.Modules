using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nandel.Modules.FunctionalTests.Samples.Services;

namespace Nandel.Modules.FunctionalTests.Samples.Modules;

[DependsOn(typeof(C))]
public class B : IModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.TryAddTransient<ServiceB>();
    }
}