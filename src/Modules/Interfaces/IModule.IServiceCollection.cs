using Microsoft.Extensions.DependencyInjection;

namespace Nandel.Modules
{
    /// <summary>
    /// Represents a module. Use DependsOnAttribute to define dependencies
    /// </summary>
    public interface IModule : IModule<IServiceCollection>
    { }
}