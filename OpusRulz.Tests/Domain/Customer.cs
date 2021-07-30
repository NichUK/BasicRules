using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusRulz.Tests.Domain
{
    public class Customer
    {
        public string Name { get; }
        public bool IsPreferred { get; set; }

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
