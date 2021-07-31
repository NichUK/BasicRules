/* Copyright (C) 2021 Nich Overend <nich@nixnet.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the MIT License.
 *
 */
using System;
using System.Collections.Generic;
using Autofac;
using BasicRules.Interfaces;
using OpusRulz.Interfaces;

namespace BasicRules.Models
{
    public class Session : ISession
    {
        private readonly IWorkspace _workspace;
        private readonly ILifetimeScope _container;
        private readonly IDictionary<string, object> _instances = new Dictionary<string, object>();
        private readonly IDictionary<string, object> _outputs = new Dictionary<string, object>();
        private bool _disposedValue;

        public bool Disposed => _disposedValue;

        /// <summary>
        /// Instances to be passed to the engine
        /// </summary>
        public IDictionary<string, object> Instances => _instances;
        
        /// <summary>
        /// Outputs from the engine
        /// </summary>
        public IDictionary<string, object> Outputs => _outputs;

        /// <summary>
        /// Reference to owning Workspace
        /// </summary>
        public IWorkspace Workspace => _workspace;

        public Session(ILifetimeScope container, IWorkspace workspace)
        {
            _workspace = workspace;
            _container = container.BeginLifetimeScope(context =>
            {
                context.RegisterInstance(this)
                    .AsSelf()
                    .AsImplementedInterfaces();
            });
        }

        /// <summary>
        /// Insert facts into the session for use by rules.
        /// Facts (objects) which have already been registered in the container do not need to be inserted
        /// unless they are to override an existing registration or require a name for some reason.
        /// </summary>
        /// <param name="name">Name of fact</param>
        /// <param name="instance">Face Instance (could be collection)</param>
        public void Insert(string name, object instance)
        {
            if (!_instances.ContainsKey(name))
            {
                _instances.Add(name, instance);
            }
        }

        public void Execute()
        {
            var executionContainer = _container.BeginLifetimeScope(context =>
            {
                foreach (var instance in Instances)
                {
                    context.RegisterInstance(instance.Value)
                        .AsSelf()
                        .AsImplementedInterfaces()
                        .Named(instance.Key, instance.Value.GetType());
                }
            });

            var engine = executionContainer.Resolve<IRulesEngine>();
            engine.Execute();
        }

        public void SetOutput(string name, object output)
        {
            if (_outputs.ContainsKey(name))
            {
                _outputs[name] = output;
            }
            else
            {
                _outputs.Add(name, output);
            }
        }

        public T GetOutput<T>(string name)
        {
            if (Outputs.ContainsKey(name))
            {
                return (T) Outputs[name];
            }

            return default;
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
