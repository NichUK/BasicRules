/* Copyright (C) 2021 Nich Overend <nich@nixnet.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the MIT License.
 *
 */
namespace BasicRules.Interfaces
{
    public interface IRule
    {
        bool Activated { get; set; }

        bool Fired { get; set; }
        
        bool CanFire { get; }

        bool RunMatch();

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