using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusRulz.Models
{
    public abstract class Rule<T> : IRule<T>
    {
        public abstract IEnumerable<T> Match();

        public abstract int Resolve();

        public abstract void Act(T item);
    }
}
