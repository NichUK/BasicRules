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
using OpusRulz.Tests.Domain;
using OpusRulz.Tests.Rules;

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
            var rules = new Type[] { typeof(PreferredCustomerDiscountRule), typeof(DiscountNotificationRule) };
            var workspaceFactory = _container.Resolve<Workspace.FromRulesFactory>();
            _workspace = workspaceFactory.Invoke(rules);
        }

        [Test]
        public void ShouldCreateAndDestroyASession()
        {
            ISession session = null;
            using (session = _workspace.CreateSession())
            {
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.Workspace);
                Assert.That(session.Workspace, Is.EqualTo(_workspace));
            }
        }

        [Test]
        public void ShouldLoadASession()
        {
            var customers = TestUtils.GetCustomers();
            var orders = TestUtils.GetOrders(customers);
            using (var session = _workspace.CreateSession())
            {
                session.Insert("customers", customers);
                Assert.That(session.Instances.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void ShouldLoadASessionWithMultipleFacts()
        {
            var customers = TestUtils.GetCustomers();
            var orders = TestUtils.GetOrders(customers);
            using (var session = _workspace.CreateSession())
            {
                session.Insert("customers", customers);
                session.Insert("orders", orders);
                Assert.That(session.Instances.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public void ShouldLoadASessionAndFire()
        {
            var customers = TestUtils.GetCustomers();
            var orders = TestUtils.GetOrders(customers);
            using (var session = _workspace.CreateSession())
            {
                session.Insert("customers", customers);
                session.Insert("orders", orders);

                session.Execute();
                var preferredCustomers = customers.Where(c => c.IsPreferred);
                var discountedOrders = orders.Where(o => preferredCustomers.Contains(o.Customer)).ToList();
                Assert.That(discountedOrders.Count, Is.EqualTo(2));
                foreach (var order in discountedOrders)
                {
                    Assert.That(order.PercentDiscount, Is.EqualTo(10.0));
                }
            }
        }

        [Test]
        public void ShouldLoadASessionAndFire2()
        {
            var numbers = new List<int>();
            var counter = 0;
            var seed = 0xFFFF & DateTime.UtcNow.Ticks;
            var rnd = new Random(Convert.ToInt32(seed));
            while (counter++ < 1000)
            {
                numbers.Add(rnd.Next(0, 10000));
            }

            var rules = new Type[] { typeof(TopTenPercentile) };
            var workspaceFactory = _container.Resolve<Workspace.FromRulesFactory>();
            using (var workspace = workspaceFactory.Invoke(rules))
            {
                using (var session = workspace.CreateSession())
                {
                    session.Insert("numbers", numbers);
                    session.Execute();
                    var outputNumbers = session.GetOutput<IList<int>>("numbers");
                    Assert.That(outputNumbers.Count, Is.EqualTo(1));
                }
            }
        }
    }
}
