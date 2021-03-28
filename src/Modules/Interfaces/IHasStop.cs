using System;
using System.Threading.Tasks;

namespace Nandel.Modules
{
    public interface IHasStop
    {
        Task StopAsync(IServiceProvider services);
    }
}