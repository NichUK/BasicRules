/* Copyright (C) 2021 Nich Overend <nich@nixnet.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the MIT License.
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicRules.Interfaces;

namespace OpusRulz.Interfaces
{
    public interface IWorkspace : IDisposable
    {
        ISession CreateSession();
    }
}
