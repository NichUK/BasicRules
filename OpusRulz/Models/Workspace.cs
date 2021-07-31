using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using OpusRulz.Interfaces;

namespace OpusRulz.Models
{
    public class Workspace : IWorkspace
    {
        private readonly ILifetimeScope _container;
        private bool _disposedValue;

        public ILifetimeScope Container => _container;

        public bool Disposed => _disposedValue;

        public delegate Workspace FromAssemblyFactory(Assembly assembly);

        public delegate Workspace FromAssembliesFactory(IEnumerable<Assembly> assemblies);

        public delegate Workspace FromRulesFactory(IEnumerable<Type> ruleTypes);

        /// <summary>
        /// Create a workspace by loading rules from a given assembly
        /// </summary>
        /// <param name="container">Autofac application container</param>
        /// <param name="assembly">The assembly to scan for rules</param>
        public Workspace(ILifetimeScope container, Assembly assembly)
            :this(container, assembly
                .GetExportedTypes()
                .Where(t => typeof(IRule).IsAssignableFrom(t)))
        {
        }

        /// <summary>
        /// Create a workspace by loading rules from multiple assemblies
        /// </summary>
        /// <param name="container">Autofac application container</param>
        /// <param name="assemblies">The assemblies to scan for rules</param>
        public Workspace(ILifetimeScope container, IEnumerable<Assembly> assemblies)
            : this(container, assemblies
                .SelectMany(a =>
                    a.GetExportedTypes()
                        .Where(t => typeof(IRule).IsAssignableFrom(t))))
        {
        }

        /// <summary>
        /// Create a workspace by loading rules supplied.
        /// This allows full control of which rules are loaded from any source
        /// </summary>
        /// <param name="container">Autofac application container</param>
        /// <param name="ruleTypes">Types defining rules to load</param>
        public Workspace(ILifetimeScope container, IEnumerable<Type> ruleTypes)
        {
            _container = container
                .BeginLifetimeScope("workspace", scope =>
                {
                    foreach (var ruleType in ruleTypes
                        .Where(t => typeof(IRule).IsAssignableFrom(t)))
                    {
                        scope.RegisterType(ruleType)
                            .AsSelf()
                            .AsImplementedInterfaces();
                    }

                    scope.RegisterInstance<IWorkspace>(this);
                });
        }

        public ISession CreateSession()
        {
            return _container.Resolve<ISession>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _container.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
