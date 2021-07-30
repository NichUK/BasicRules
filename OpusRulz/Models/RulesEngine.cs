using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using OpusRulz.Interfaces;

namespace OpusRulz.Models
{
    public class RulesEngine : IRulesEngine
    {
        private readonly ILifetimeScope _container;
        private readonly ISession _session;
        private IList<IRule> _rules;

        private int _cycleCounter;

        public int CycleCount => _cycleCounter;

        delegate IRulesEngine Factory(ISession session);

        public RulesEngine(ILifetimeScope container, ISession session)
        {
            _session = session;
            _container = container.BeginLifetimeScope(context =>
            {
                context.RegisterInstance(session)
                    .AsSelf()
                    .AsImplementedInterfaces();
            });
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
            var matches = _rules.Where(r => r.Match())
                .Where(m => m.Activated && m.CanFire).ToList();
            // Resolve
            var select = matches.Count() == 1
                ? matches.First()
                : matches.OrderByDescending(m => m.Resolve()).FirstOrDefault();
            // Act
            if (select != null)
            {
                select.SetupToAct();
                select.ActAll();
                select.Fired = true;
                select.Finally();
                return true;
            }

            return false;
        }
    }
}
