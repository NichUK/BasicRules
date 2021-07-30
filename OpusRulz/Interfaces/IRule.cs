using System.Collections;
using System.Collections.Generic;

namespace OpusRulz.Interfaces
{
    public interface IRule
    {
        bool Activated { get; set; }
        bool Fired { get; set; }
        bool CanFire { get; }
        bool Match();
        int Resolve();
        void SetupToAct();
        void ActAll();
        void Finally();
    }

    public interface IRule<T> : IRule
    {
        void Act(T item);
    }
}