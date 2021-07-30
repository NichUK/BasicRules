using System.Linq;
using Autofac;
using Autofac.Core.Activators.Reflection;
using NUnit.Framework;
using OpusRulz.Autofac;
using OpusRulz.Models;
using OpusRulz.Tests.Rules;

namespace OpusRulz.Tests
{
    public class WorkspaceTests
    {
        private IContainer _container;

        [SetUp]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(IRule).Assembly);
            _container = builder.Build();
        }

        [Test]
        public void ShouldCreateAWorkspace()
        {
            var workspace = new Workspace(_container, this.GetType().Assembly);
            Assert.IsNotNull(workspace);
            Assert.IsNotNull(workspace.Container);
            Assert.That(workspace.Container.ComponentRegistry.Registrations.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void ShouldCreateAWorkspaceAndResolveARule()
        {
            var workspace = new Workspace(_container, this.GetType().Assembly);
            var ruleType = workspace.Container.Resolve<PreferredCustomerDiscountRule>();
            Assert.IsNotNull(ruleType);
            Assert.That(ruleType, Is.TypeOf<PreferredCustomerDiscountRule>());
        }

        [Test]
        public void ShouldCreateAWorkspaceViaAFactory()
        {
            var workspaceFactory = _container.Resolve<Workspace.FromAssemblyFactory>();
            var workspace = workspaceFactory.Invoke(this.GetType().Assembly);
            Assert.IsNotNull(workspace);
        }
    }
}