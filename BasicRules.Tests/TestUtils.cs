using System.Collections.Generic;
using BasicRules.Tests.Domain;

namespace BasicRules.Tests
{
    public static class TestUtils
    {
        public static IList<Customer> GetCustomers()
        {
            return new List<Customer>()
            {
                new Customer("Nich")
                {
                    IsPreferred = true
                },
                new Customer("Sarah")
                {
                    IsPreferred = false
                }
            };

        }

        public static IList<Order> GetOrders(IList<Customer> customers)
        {
            return new List<Order>()
            {
                new Order(1001, customers[0], 1, 1.1),
                new Order(1002, customers[0], 2, 2.2),
                new Order(1003, customers[1], 3, 3.3)
            };
        }
    }
}
