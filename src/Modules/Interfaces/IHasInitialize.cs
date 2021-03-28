using System;

namespace Nandel.Modules
{
    /// <summary>
    /// The module has a initialization process
    /// * For background operations use IHasStart
    /// </summary>
    public interface IHasInitialize
    {
        /// <summary>
        /// Initialize the module
        /// </summary>
        /// <param name="services"></param>
        void Initialize(IServiceProvider services);
    }
}