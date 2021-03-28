using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Nandel.Modules.AspNetCore
{
    public class ModulesHostedService : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly IEnumerable<IDependencyNode> _dependencies;

        public ModulesHostedService(IServiceProvider services, IEnumerable<IDependencyNode> dependencies)
        {
            _services = services;
            _dependencies = dependencies;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var node in _dependencies)
            {
                await node.StartAsync(_services);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var node in _dependencies)
            {
                await node.StopAsync(_services);
            }
        }
    }
}