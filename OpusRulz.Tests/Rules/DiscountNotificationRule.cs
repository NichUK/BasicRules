using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpusRulz.Models;
using OpusRulz.Tests.Domain;

namespace OpusRulz.Tests.Rules
{
    public class DiscountNotificationRule : Rule<Customer>
    {
        private IList<Customer> _customers;
        private IList<Order> _orders;

        public DiscountNotificationRule(IList<Customer> customers, IList<Order> orders)
        {
            _customers = customers;
            _orders = orders;
        }

        public override bool Match()
        {
            return GetMatches(() => _orders
                .Where(o => o.PercentDiscount > 0.0)
                .Select(o => o.Customer));
        }

        public override int Resolve()
        {
            return 100;
        }

        public override void Act(Customer customer)
        {
            customer.NotifyAboutDiscount();
        }
    }
}
