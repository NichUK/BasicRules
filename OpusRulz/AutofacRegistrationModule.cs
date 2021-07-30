using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core.Activators.Reflection;
using OpusRulz.Autofac;

namespace OpusRulz
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
