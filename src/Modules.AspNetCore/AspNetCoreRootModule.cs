using Microsoft.Extensions.DependencyInjection;

namespace Nandel.Modules.AspNetCore;

public sealed class AspNetCoreRootModule : IModule<IServiceCollection>
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Null Design Pattern
    }
}