﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nandel.Modules
{
    /// <summary>
    /// Use this when the module has something runing in the background
    /// and can be stoped
    /// </summary>
    public interface IHasStop
    {
        /// <summary>
        /// Stop the module
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task StopAsync(IServiceProvider services, CancellationToken cancellationToken);
    }
}