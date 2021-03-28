using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nandel.Modules;
using Xunit;

namespace Modules.FunctionalTests.Services
{
    public class ModuleFactoryTests
    {
        [Fact]
        public void CreateInstance_WithDependencies()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>().Object;
            var factory = new ModuleFactory(new object[] {configuration});
           
            var services = new Mock<IServiceCollection>();
            services
                .Setup(x => x.Add(It.IsAny<ServiceDescriptor>()))
                .Verifiable()
                ;
            
            var dependencies = new DependencyList(factory, typeof(ModuleWithConfiguration));
            
            // Act
            dependencies.RegisterServices(services.Object);
            
            // Assert
            services.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Exactly(2));
        }
        
        [DependsOn(typeof(ModuleWithoutDependencies))]
        private class ModuleWithConfiguration : IModule
        {
            private readonly IConfiguration _config;

            public ModuleWithConfiguration(IConfiguration config)
            {
                _config = config ?? throw new ArgumentNullException(nameof(config));
            }
            
            public void RegisterServices(IServiceCollection services)
            {
                services.AddSingleton(this);
            }
        }

        private class ModuleWithoutDependencies : IModule
        {
            public void RegisterServices(IServiceCollection services)
            {
                services.AddSingleton(this);
            }
        }
    }
}