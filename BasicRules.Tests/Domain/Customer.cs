using System;

namespace BasicRules.Tests.Domain
{
    public class Customer
    {
        public string Name { get; }
        public bool IsPreferred { get; set; }

        public Customer()
        {

        }

        public Customer(string name)
        {
            Name = name;
        }

        public void NotifyAboutDiscount()
        {
            Console.WriteLine($"Customer {Name} was notified about a discount");
        }
    }
}
