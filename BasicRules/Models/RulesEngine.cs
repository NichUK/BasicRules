/* Copyright (C) 2021 Nich Overend <nich@nixnet.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the MIT License.
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using BasicRules.Interfaces;

namespace BasicRules.Models
{
    public class RulesEngine : IRulesEngine
    {
        private readonly ILifetimeScope _container;
        private readonly ISession _session;
        private IList<IRule> _rules;

        private int _cycleCounter;
        private bool _disposedValue;

        public int CycleCount => _cycleCounter;

        public bool Disposed => _disposedValue;

        delegate IRulesEngine Factory(ISession session);

        public RulesEngine(ILifetimeScope container, ISession session)
        {
            _session = session;
            _container = container;
            _rules = _container.Resolve<IEnumerable<IRule>>().ToList();
        }

        public void Execute()
        {
            _cycleCounter = 0;
            while (ExecuteCycle())
            {
                _cycleCounter++;
            }
        }

        public bool ExecuteCycle()
        {
            // Setup

            // Match (Activate)
            var matches = _rules.Where(r => r.RunMatch())
                .Where(m => m.Activated && m.CanFire).ToList();

            // Resolve
            var select = matches.Count() == 1
                ? matches.First()
                : matches.OrderByDescending(m => m.Resolve()).FirstOrDefault();
            
            // Act
            if (select != null)
            {
                // Act Lifecycle
                select.PreAct();
                select.ActAll();
                select.Fired = true;
                select.PostAct();
                return true;
            }

            return false;
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
