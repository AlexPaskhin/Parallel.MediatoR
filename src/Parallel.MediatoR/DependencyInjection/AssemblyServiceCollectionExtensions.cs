// Copyright © Alexander Paskhin 2021. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Parallel.MediatoR.Notification;
using Parallel.MediatoR.Request;
using System;
using System.Linq;
using System.Reflection;

namespace Parallel.MediatoR.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding Parallel.Mediator services to the DI container.
    /// </summary>
    public static partial class ParallelMediatoRServiceCollectionExtensions
    {

        public static void AddParallelMediatorClasses(IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Transient, params Assembly[] assembliesToScanArray )
        {
            assembliesToScanArray = assembliesToScanArray.Distinct().ToArray();


            var allTypes = assembliesToScanArray
                  .Where(a => !a.IsDynamic && a.GetName().Name != nameof(Parallel.MediatoR))
                  .Distinct() // avoid duplicates
                  .SelectMany(a => a.DefinedTypes)
                  .ToArray();

            var openTypes = new[]
               {
                    typeof(IRequestHandler<,>),
                    typeof(INotificationHandler<>)
                };

            foreach (var type in openTypes.SelectMany(openType => allTypes
                .Where(t => t.IsClass
                    && !t.IsGenericTypeDefinition
                    && !t.ContainsGenericParameters
                    && !t.IsAbstract
                    && t.AsType().ImplementsGenericInterface(openType))))
            {
                switch (serviceLifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.TryAddSingleton(type.AsType());
                        break;
                    case ServiceLifetime.Scoped:
                        services.TryAddScoped(type.AsType());
                        break;
                    case ServiceLifetime.Transient:
                        services.TryAddTransient(type.AsType());
                        break;
                    default:
                        break;
                }
            }

        }

        private static bool ImplementsGenericInterface(this Type type, Type interfaceType)
           => type.IsGenericType(interfaceType) || type.GetTypeInfo().ImplementedInterfaces.Any(@interface => @interface.IsGenericType(interfaceType));

        private static bool IsGenericType(this Type type, Type genericType)
            => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericType;
    }
}

