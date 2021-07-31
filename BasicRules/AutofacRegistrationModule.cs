/* Copyright (C) 2021 Nich Overend <nich@nixnet.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the MIT License.
 *
 */
using Autofac;
using Autofac.Core.Activators.Reflection;
using BasicRules.Autofac;
using BasicRules.Models;

namespace BasicRules
{
    public class AutofacRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsSelf()
                .AsImplementedInterfaces()
                .UsingConstructor(new MostParametersConstructorSelector());


            builder.RegisterType<Workspace>()
                .AsSelf()
                .AsImplementedInterfaces()
                .UsingConstructor(new MultipleDelegateConstructorSelector());
        }
    }
}
