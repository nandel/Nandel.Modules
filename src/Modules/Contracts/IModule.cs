namespace Nandel.Modules;

/// <summary>
/// Represents a module. Use DependsOnAttribute to define dependencies
/// </summary>
/// <typeparam name="TServiceCollection">Container for services registration</typeparam>
public interface IModule<in TServiceCollection>
{
    /// <summary>
    /// Register services for this module
    /// </summary>
    /// <param name="services"></param>
    void ConfigureServices(TServiceCollection services);
}