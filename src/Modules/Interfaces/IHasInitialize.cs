using System;

namespace Nandel.Modules
{
    public interface IHasInitialize
    {
        void Initialize(IServiceProvider services);
    }
}