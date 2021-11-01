using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Nandel.Modules
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Invoke module.StartAsync(IServiceProvider, CancellationToken) of every module registred
        /// </summary>
        /// <param name="services">Collection of services</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task StartModulesAsync(this IServiceProvider services, CancellationToken cancellationToken)
        {
            var dependencies = services.GetServices<IDependencyNode>();
            foreach (var dependency in dependencies)
            {
                await dependency.StartAsync(services, cancellationToken);
            }
        }

        /// <summary>
        /// Invoke module.StopAsync(IServiceProvvider, CancellationToken) of every module registred
        /// </summary>
        /// <param name="services">Collection of services</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task StopModulesAsync(this IServiceProvider services, CancellationToken cancellationToken)
        {
            var dependencies = services.GetServices<IDependencyNode>();
            foreach (var dependency in dependencies)
            {
                await dependency.StopAsync(services, cancellationToken);
            }
        }
    }
}