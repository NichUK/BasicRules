using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;

namespace BasicRules.Autofac
{
    public class MultipleDelegateConstructorSelector : IConstructorSelector
    {
        /// <summary>
        /// Selects the best constructor from the available constructors.
        /// </summary>
        /// <param name="constructorBindings">Available constructors.</param>
        /// <param name="parameters">Parameters to the instance being resolved.</param>
        /// <returns>The best constructor.</returns>
        public BoundConstructor SelectConstructorBinding(BoundConstructor[] constructorBindings, IEnumerable<Parameter> parameters)
        {
            if (constructorBindings == null) throw new ArgumentNullException(nameof(constructorBindings));
            if (constructorBindings.Length == 0) throw new ArgumentOutOfRangeException(nameof(constructorBindings));

            if (constructorBindings.Length == 1)
                return constructorBindings[0];

            // we need to select the constructor(s) which matches the parameter names 
            var namesRequired = parameters.Select(p => ((NamedParameter)p).Name).ToList();

            var withNames = new List<BoundConstructor>();
            foreach (var binding in constructorBindings)
            {
                var isMatch = true;
                foreach (var name in namesRequired)
                {
                    if (binding.Binder.Parameters.All(p => p.Name != name))
                    {
                        isMatch = false;
                        continue;
                    }
                }

                if (isMatch)
                {
                    withNames.Add(binding);
                }
            }

            var largest = withNames.OrderByDescending(b => b.Binder.Parameters.Count).FirstOrDefault();

            if (largest != null)
            {
                return largest;
            }

            throw new DependencyResolutionException(string.Format(
                CultureInfo.CurrentCulture,
                "Cannot choose between multiple constructors by name"));
        }
    }
}
