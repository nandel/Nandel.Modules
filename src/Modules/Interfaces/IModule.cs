using System;
using Microsoft.Extensions.DependencyInjection;

namespace Nandel.Modules
{
    public interface IModule
    {
        void RegisterServices(IServiceCollection services);
    }
}