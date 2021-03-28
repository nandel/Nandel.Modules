using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Samples.Modules;
using Modules.FunctionalTests.Samples.Services;
using Moq;
using Nandel.Modules;
using Xunit;

namespace Modules.FunctionalTests.DependencyNode
{
    public class FunctionalTests
    {
        [Fact]
        public void RegisterServices_ShouldBeInvokedOnlyOnceByModule()
        {
            // Module `C` is registred twice in the tree
            // Since we register from A to D we should invoke 4 times

            // Arrange
            var tree = new DependencyList(ModuleFactory.Default, typeof(A));
            
            var services = new Mock<IServiceCollection>();
            services.Setup(x => x.Add(It.IsAny<ServiceDescriptor>()))
                .Verifiable()
                ;

            // Act
            tree.RegisterServices(services.Object);
            
            // Assert
            services.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), () => Times.Exactly(4));
        }

        [Fact]
        public void Initialize_ShouldBeInvokedOnlyOnceByModule()
        {
            // Module `C` is registred twice in the tree
            // and has a ServiceC singleton with a counter
            // so we can check how many times it was invoked
            
            // Arrange
            var tree = new DependencyList(ModuleFactory.Default, typeof(A));
            var collection = new ServiceCollection();
            tree.RegisterServices(collection);;
            var services = collection.BuildServiceProvider();
            
            // Act
            tree.Initialize(services);

            // Assert
            Assert.Equal(1, services.GetRequiredService<ServiceC>().Count);
        }
        
        [Fact]
        public async Task StartAsync_ShouldBeInvokedOnlyOnceByModule()
        {
            // Module `C` is registred twice in the tree
            // and has a ServiceC singleton with a counter
            // so we can check how many times it was invoked
            
            // Arrange
            var tree = new DependencyList(ModuleFactory.Default, typeof(A));
            var collection = new ServiceCollection();
            tree.RegisterServices(collection);;
            var services = collection.BuildServiceProvider();
            
            // Act
            await tree.StartAsync(services, CancellationToken.None);

            // Assert
            Assert.Equal(1, services.GetRequiredService<ServiceC>().Count);
        }
        
        [Fact]
        public async Task StopAsync_ShouldBeInvokedOnlyOnceByModule()
        {
            // Module `C` is registred twice in the tree
            // and has a ServiceC singleton with a counter
            // so we can check how many times it was invoked
            
            // Arrange
            var tree = new DependencyList(ModuleFactory.Default, typeof(A));
            var collection = new ServiceCollection();
            tree.RegisterServices(collection);
            var services = collection.BuildServiceProvider();
            
            // Act
            await tree.StopAsync(services, CancellationToken.None);

            // Assert
            Assert.Equal(1, services.GetRequiredService<ServiceC>().Count);
        }
    }
}