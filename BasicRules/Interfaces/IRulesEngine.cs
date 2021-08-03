/* Copyright (C) 2021 Nich Overend <nich@nixnet.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the MIT License.
 *
 */
using System;
using OpusRulz.Interfaces;

namespace BasicRules.Interfaces
{
    public interface IRulesEngine : IDisposable
    {

        void Execute();
    }
}
