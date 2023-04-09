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

        public ModulesHostedService(IServiceProvider services)
        {
            _services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _services.StartModulesAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _services.StopModulesAsync(cancellationToken);
        }
    }
}