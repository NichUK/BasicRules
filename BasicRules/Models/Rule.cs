/* Copyright (C) 2021 Nich Overend <nich@nixnet.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the MIT License.
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using BasicRules.Interfaces;

namespace BasicRules.Models
{
    public abstract class Rule<T> : IRule<T>
    {
        protected IList<T> __matches;
        protected bool __halted;

        public virtual bool FireMultiple => false;

        public bool CanFire => FireMultiple || !Fired;

        public bool Halted => __halted;

        public bool Activated { get; set; }
        public bool Fired { get; set; }

        /// <summary>
        /// This method is meant for internal use. Override the Match method to define whether a rule
        /// matches for Activation.
        /// </summary>
        /// <returns></returns>
        public bool RunMatch()
        {
            if (__halted)
            {
                return false;
            }

            return Match();
        }

        /// <summary>
        /// The Match method is used to determine whether this rule matches the current data.
        /// If so, the rule will be "Activated".
        /// Override this method with your "Match" code to determine activation.
        /// Don't confuse this (Rule Matching) with Data Matching (which you will do
        /// simultaneously to determine if the rule should match).
        /// Use the GetMatches function to assist.
        /// </summary>
        /// <returns>True if the rule is to be activated, otherwise false.</returns>
        public virtual bool Match()
        {
            return false;
        }

        /// <summary>
        /// When multiple rules "match" simultaneously, the Resolve method is used to rank them.
        /// The largest match will be chosen to fire.
        /// </summary>
        /// <returns>ranking priority</returns>
        public virtual int Resolve()
        {
            return 100;
        }

        /// <summary>
        /// PreAct is executed before the Act method on the highest ranked Activated rule.
        /// You can use it to set up input or output data structures ready for the rule to run.
        /// </summary>
        public virtual void PreAct()
        {
            return;
        }

        /// <summary>
        /// ActAll runs the Act method for all data matches.
        /// </summary>
        public void ActAll()
        {
            foreach (var item in __matches)
            {
                Act(item);
            }
        }


        /// <summary>
        /// Act is executed on each matching data item when the rule is run. Act can access data from your entire system,
        /// which is both powerful, and dangerous. Use with care!
        /// </summary>
        /// <param name="item"></param>
        public virtual void Act(T item)
        {
            return;
        }

        /// <summary>
        /// PostAct is run after all data items have been ACTed on.
        /// </summary>
        public virtual void PostAct()
        {
            return;
        }

        /// <summary>
        /// Helper function which defines match and halt functions.
        /// These functions are 
        /// </summary>
        /// <param name="matchFunc">Determines which items from the primary data set are to be used when the rule is run.</param>
        /// <param name="haltFunc">For rules where FireMultiple is true, this Func allows the cycle to be halted.</param>
        /// <returns></returns>
        public bool GetDataMatches(Func<IEnumerable<T>> matchFunc, Func<IEnumerable<T>, bool> haltFunc = null)
        {
            __matches = matchFunc.Invoke().ToList();
            if (haltFunc != null && haltFunc.Invoke(__matches))
            {
                __halted = true;
            }

            return SetRuleActivated(__matches.Any());
        }

        private bool SetRuleActivated(bool shouldActivate)
        {
            if (!shouldActivate)
            {
                __halted = true;
            }

            return Activated = shouldActivate;
        }
    }
}
