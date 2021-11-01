using System;
using System.Collections.Generic;

namespace Nandel.Modules
{
    /// <summary>
    /// representation of a dependancy
    /// </summary>
    public interface IDependencyNode : IHasStart, IHasStop
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

        /// <summary>
        /// Get the Nodes in the order that they should be invoked without repeating
        /// </summary>
        /// <returns></returns>
        IEnumerable<IDependencyNode> GetNodes();

        /// <summary>
        /// Configure services for this node
        /// </summary>
        /// <param name="services"></param>
        /// <typeparam name="TServiceCollection"></typeparam>
        void ConfigureServices<TServiceCollection>(TServiceCollection services);

        /// <summary>
        /// Invoke a contract
        /// </summary>
        /// <param name="args"></param>
        /// <typeparam name="T"></typeparam>
        void Invoke<T>(params object[] args) where T : class;
    }
}