using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using OpusRulz.Autofac;
using OpusRulz.Interfaces;
using OpusRulz.Models;

namespace OpusRulz.Tests
{
    public class SessionTests
    {
        private IContainer _container;
        private IWorkspace _workspace;

        [SetUp]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(IRule).Assembly);
            _container = builder.Build();

            var workspaceFactory = _container.Resolve<Workspace.FromAssemblyFactory>();
            _workspace = workspaceFactory.Invoke(this.GetType().Assembly);
        }

        [Test]
        public void ShouldCreateASession()
        {
            var session = _workspace.CreateSession();
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.Workspace);
            Assert.That(session.Workspace, Is.EqualTo(_workspace));
        }
    }
}
