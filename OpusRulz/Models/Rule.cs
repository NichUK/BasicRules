using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpusRulz.Interfaces;

namespace OpusRulz.Models
{
    public abstract class Rule<T> : IRule<T>
    {
        protected IList<T> __matches;
        protected bool __halted;

        public virtual bool FireMultiple => false;

        public bool CanFire => FireMultiple || !Fired;

        public bool Halted => __halted;

        public virtual bool Match()
        {
            return false;
        }

        public virtual int Resolve()
        {
            return 100;
        }

        public virtual void SetupToAct()
        {
            return;
        }

        public virtual void Act(T item)
        {
            return;
        }

        public void ActAll()
        {
            foreach (var item in __matches)
            {
                Act(item);
            }
        }

        public virtual void Finally()
        {
            return;
        }

        public bool GetMatches(Func<IEnumerable<T>> matchFunc, Func<IEnumerable<T>, bool> haltFunc = null)
        {
            __matches = matchFunc.Invoke().ToList();
            if (haltFunc != null && haltFunc.Invoke(__matches))
            {
                return false;
            }

            return Activated = __matches.Any();
        }

        public bool Activated { get; set; }
        public bool Fired { get; set; }
    }
}
