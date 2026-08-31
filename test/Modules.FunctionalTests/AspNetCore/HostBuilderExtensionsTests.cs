using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nandel.Modules.AspNetCore;
using Xunit;

namespace Nandel.Modules.FunctionalTests.AspNetCore;

public class HostBuilderExtensionsTests
{
    [Fact]
    public void AddModule_OnHostApplicationBuilder_ShouldRegisterModuleAndHostedService()
    {
        // arrange
        var builder = Host.CreateApplicationBuilder();

        // act
        builder.AddModule<SampleModule>();

        using var host = builder.Build();

        // assert
        Assert.NotNull(host.Services.GetRequiredService<Marker>());
        Assert.Single(host.Services.GetServices<IHostedService>());
    }

    [Fact]
    public void AddModule_OnHostBuilder_ShouldRegisterModuleAndHostedService()
    {
        // arrange
        var hostBuilder = Host.CreateDefaultBuilder();

        // act
#pragma warning disable CS0618
        hostBuilder.AddModule<SampleModule>();
#pragma warning restore CS0618

        using var host = hostBuilder.Build();

        // assert
        Assert.NotNull(host.Services.GetRequiredService<Marker>());
        Assert.Single(host.Services.GetServices<IHostedService>());
    }

    private class Marker
    {
    }

    private class SampleModule : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Marker>();
        }
    }
}
