using System.Collections.Generic;
using System.Linq;
using BasicRules.Models;
using BasicRules.Tests.Domain;

namespace BasicRules.Tests.Rules
{
    /// <summary>
    /// Find all preferred customers.
    /// For every matching customer apply a discount of 10% to all orders
    /// </summary>
    public class PreferredCustomerDiscountRule : Rule<Customer>
    {
        private IList<Customer> _customers;
        private IList<Order> _orders;

        public PreferredCustomerDiscountRule(IList<Customer> customers, IList<Order> orders)
        {
            _customers = customers;
            _orders = orders;
        }

        public override bool Match()
        {
            return GetDataMatches(() => _customers.Where(c => c.IsPreferred));
        }

        public override int Resolve()
        {
            return 100;
        }

        public override void Act(Customer customer)
        {
            foreach (var order in _orders.Where(o => o.Customer == customer))
            {
                order.PercentDiscount = 10.0;
            }
        }
    }
}
