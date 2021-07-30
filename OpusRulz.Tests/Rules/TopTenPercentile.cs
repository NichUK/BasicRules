﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpusRulz.Interfaces;
using OpusRulz.Models;

namespace OpusRulz.Tests.Rules
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
            if (__halted)
            {
                return false;
            }

            var min = _numbers.Min();
            var max = _numbers.Max();
            var cutoff = max - ((max - min) / 10);
            if (!GetMatches(() => _numbers.Where(n => n >= cutoff), (matches) => matches.Count() == 1))
            {
                __halted = true;
            }

            return true;
        }

        public override int Resolve()
        {
            return 100;
        }

        public override void SetupToAct()
        {
            _matches = new List<int>();
        }

        public override void Act(int number)
        {
            _matches.Add(number);
        }

        public override void Finally()
        {
            _numbers = _matches;
            _session.SetOutput("numbers", _numbers);
        }
    }
}
