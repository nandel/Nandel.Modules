using System;
using Microsoft.Extensions.Configuration;

namespace Nandel.Modules
{
    /// <summary>
    /// representation of a dependancy
    /// </summary>
    public interface IDependencyNode : IModule, IHasInitialize, IHasStart, IHasStop
    {
        /// <summary>
        /// Literaly the root dependency (Use this when need operate thought all the tree)
        /// </summary>
        IDependencyNode Root { get; }
        
        /// <summary>
        /// Find a node of the module type if aready exists
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        IDependencyNode FindNode(Type moduleType);
    }
}