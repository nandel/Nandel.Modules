using System;
using System.Threading.Tasks;

namespace Nandel.Modules
{
    public interface IHasStart
    {
        Task StartAsync(IServiceProvider services);
    }
}