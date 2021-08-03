/* Copyright (C) 2021 Nich Overend <nich@nixnet.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the MIT License.
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using BasicRules.Interfaces;
using OpusRulz.Interfaces;

namespace BasicRules.Models
{
    public class Workspace : IWorkspace
    {
        private readonly ILifetimeScope _container;
        private bool _disposedValue;

        public ILifetimeScope Container => _container;

        public bool Disposed => _disposedValue;

        public delegate Workspace FromAssemblyFactory(Assembly assembly,
            IEnumerable<Type> additionalTypes = null, IDictionary<string, object> inputs = null);

        public delegate Workspace FromAssembliesFactory(IEnumerable<Assembly> assemblies,
            IEnumerable<Type> additionalTypes = null, IDictionary<string, object> inputs = null);

        public delegate Workspace FromRulesFactory(IEnumerable<Type> ruleTypes, 
            IEnumerable<Type> additionalTypes = null, IDictionary<string, object> inputs = null);

        /// <summary>
        /// Create a workspace by loading rules from a given assembly
        /// </summary>
        /// <param name="container">Autofac application container</param>
        /// <param name="assembly">The assembly to scan for rules</param>
        /// <param name="additionalTypes">Additional types to register</param>
        /// <param name="inputs">Additional instances inputs to register</param>
        public Workspace(ILifetimeScope container, Assembly assembly,
            IEnumerable<Type> additionalTypes = null, IDictionary<string, object> inputs = null)
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
        /// <param name="additionalTypes">Additional types to register</param>
        /// <param name="inputs">Additional instances inputs to register</param>
        public Workspace(ILifetimeScope container, IEnumerable<Assembly> assemblies,
            IEnumerable<Type> additionalTypes = null, IDictionary<string, object> inputs = null)
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
        /// <param name="additionalTypes">Additional types to register</param>
        /// <param name="inputs">Additional instances inputs to register</param>
        public Workspace(ILifetimeScope container, IEnumerable<Type> ruleTypes, 
            IEnumerable<Type> additionalTypes = null, IDictionary<string, object> inputs = null)
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

                    if (additionalTypes != null)
                    {
                        scope.RegisterTypes(additionalTypes.ToArray());
                    }

                    if (inputs != null)
                    {
                        foreach (var input in inputs)
                        {
                            scope.RegisterInstance(input.Value)
                                .AsSelf()
                                .AsImplementedInterfaces()
                                .Named(input.Key, input.Value.GetType())
                                .SingleInstance();
                        }
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
