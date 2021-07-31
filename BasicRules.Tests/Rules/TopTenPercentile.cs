using System.Collections.Generic;
using System.Linq;
using BasicRules.Interfaces;
using BasicRules.Models;

namespace BasicRules.Tests.Rules
{
    public class TopTenPercentile : Rule<int>
    {
        private ISession _session;
        private IList<int> _numbers;
        private IList<int> _matches;

        public override bool FireMultiple => true;

        public TopTenPercentile(ISession session, IList<int> numbers)
        {
            _session = session;
            _numbers = numbers;
        }

        public override bool Match()
        {
            var min = _numbers.Min();
            var max = _numbers.Max();
            var cutoff = max - ((max - min) / 10);
            return GetDataMatches(
                () => _numbers.Where(n => n >= cutoff), 
                (matches) => matches.Count() == 1
                );
        }

        public override int Resolve()
        {
            return 100;
        }

        public override void PreAct()
        {
            _matches = new List<int>();
        }

        public override void Act(int number)
        {
            _matches.Add(number);
        }

        public override void PostAct()
        {
            _numbers = _matches;
            _session.SetOutput("numbers", _numbers);
        }
    }
}
