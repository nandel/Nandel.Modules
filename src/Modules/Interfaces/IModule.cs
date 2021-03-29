using Microsoft.Extensions.DependencyInjection;

namespace Nandel.Modules
{
    /// <summary>
    /// Represents a module. Use DependsOnAttribute to define dependencies
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Register services for this module
        /// </summary>
        /// <param name="services"></param>
        void ConfigureServices(IServiceCollection services);
    }
}