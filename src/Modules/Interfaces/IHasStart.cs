using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nandel.Modules
{
    /// <summary>
    /// Use this when the module run's something in the background
    /// that can be started
    /// </summary>
    public interface IHasStart
    {
        /// <summary>
        /// Start the module
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task StartAsync(IServiceProvider services, CancellationToken cancellationToken);
    }
}