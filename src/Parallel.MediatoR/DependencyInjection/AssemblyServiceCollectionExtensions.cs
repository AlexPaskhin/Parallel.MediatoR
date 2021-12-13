// Copyright © Alexander Paskhin 2021. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Parallel.MediatoR.Notification;
using Parallel.MediatoR.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Parallel.MediatoR.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding Parallel.Mediator services to the DI container.
    /// </summary>
    public static partial class ParallelMediatoRServiceCollectionExtensions
    {

        public static IServiceCollection AddParallelMediatorClasses(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Transient, params Assembly[] assembliesToScanArray)
        {
            assembliesToScanArray = assembliesToScanArray.Distinct().ToArray();


            var allTypes = assembliesToScanArray
                  .Where(a => !a.IsDynamic && a.GetName().Name != nameof(Parallel.MediatoR))
                  .Distinct() // avoid duplicates
                  .SelectMany(a => a.DefinedTypes)
                  .ToArray();

            Type[] openTypes = new[]
               {
                    typeof(IRequestHandler<,>),
                    typeof(INotificationHandler<>)
                };

            foreach (var openType in openTypes)
            {

                List<TypeInfo> filteredTypeInfo = allTypes
                    .Where(t => t.IsClass
                        && !t.IsGenericTypeDefinition
                        && !t.ContainsGenericParameters
                        && !t.IsAbstract
                        && t.AsType().ImplementsGenericInterface(openType)).ToList();

                foreach (var implementationTypeInfo in filteredTypeInfo)
                {

                    var iterfaces = implementationTypeInfo.ImplementedInterfaces.Where(t => t.ImplementsGenericInterface(openType));

                    foreach (var interfaceImpl in iterfaces)
                    {
                        Type serviceType = interfaceImpl;

                        switch (serviceLifetime)
                        {
                            case ServiceLifetime.Singleton:
                                services.AddSingleton(serviceType, implementationTypeInfo.AsType());
                                break;
                            case ServiceLifetime.Scoped:
                                services.AddScoped(serviceType, implementationTypeInfo.AsType());
                                break;
                            case ServiceLifetime.Transient:
                                services.AddTransient(serviceType, implementationTypeInfo.AsType());
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return services;

        }

        private static bool ImplementsGenericInterface(this Type type, Type interfaceType)
           => type.IsGenericType(interfaceType) || type.GetTypeInfo().ImplementedInterfaces.Any(@interface => @interface.IsGenericType(interfaceType));

        private static bool IsGenericType(this Type type, Type genericType)
            => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericType;
    }
}

