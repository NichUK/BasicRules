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
        
        void PreAct();
        
        void ActAll();
        
        void PostAct();
    }

    public interface IRule<in T> : IRule
    {
        void Act(T item);
    }
}