/* Copyright (C) 2021 Nich Overend <nich@nixnet.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the GNU Lesser Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 */
using System;
using System.Collections.Generic;
using OpusRulz.Interfaces;

namespace BasicRules.Interfaces
{
    public interface ISession : IDisposable
    {
        IDictionary<string, object> Instances { get; }

        IDictionary<string, object> Outputs { get; }

        IWorkspace Workspace { get; }
 
        void Insert(string name, object instance);

        void Execute();

        void SetOutput(string name, object output);

        T GetOutput<T>(string name);
    }
}
