using System.Collections.Generic;

namespace OpusRulz.Models
{
    public interface IRule
    {

    }

    public interface IRule<T> : IRule
    {
        IEnumerable<T> Match();
        int Resolve();
        void Act(T item);
    }
}