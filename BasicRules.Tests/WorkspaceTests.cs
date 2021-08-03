using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using BasicRules.Interfaces;
using BasicRules.Models;
using BasicRules.Tests.Domain;
using BasicRules.Tests.Rules;
using NUnit.Framework;

namespace BasicRules.Tests
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

        [Test]
        public void ShouldCreateAWorkspaceWithAdditionalRegisteredType()
        {
            var workspaceFactory = _container.Resolve<Workspace.FromRulesFactory>();
            var rules = new Type[] { typeof(PreferredCustomerDiscountRule) };
            var additionalTypes = new Type[] { typeof(Customer) };
            var inputs = new Dictionary<string, object>()
            {
                {"CustomerNich", new Customer("Nich")}
            };

            var workspace = workspaceFactory.Invoke(rules, additionalTypes, inputs);
            Assert.IsNotNull(workspace);
            Assert.IsNotNull(workspace.Container);
            var customer = workspace.Container.Resolve<Customer>();
            Assert.IsNotNull(customer);
            Assert.That(customer, Is.TypeOf<Customer>());
            var customerInput = workspace.Container.ResolveNamed<Customer>("CustomerNich");
            Assert.IsNotNull(customerInput);
            Assert.That(customerInput.Name, Is.EqualTo("Nich"));
        }
    }
}