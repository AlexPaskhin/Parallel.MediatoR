// Copyright © Alexander Paskhin 2020. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Parallel.MediatoR.Request;
using Parallel.MediatoR.Notification;
using Parallel.MediatoR.Common;

namespace Parallel.MediatoR.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding Parallel.Mediator services to the DI container.
    /// </summary>
    public static class ParallelMediatoRServiceCollectionExtensions
    {

        /// <summary>
        /// Adds services required for using of Request Parallel.Mediator.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddMqRequestMediator(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IRequestMediator<,>), typeof(RequestMediatorManager<,>)));
            services.TryAdd(ServiceDescriptor.Singleton(typeof(IRequestMediatorFactory<,>), typeof(ParallelRequestMediatorFactory<,>)));
            return services;
        }


        /// <summary>
        /// Add delegate of <see cref="RequestResponseDelegateAsync{TRequest, TResponse}"/> to the services collection.
        /// </summary>
        /// <typeparam name="TRequest">The request type</typeparam>
        /// <typeparam name="TResponse">The response time.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="requestDelegate">The instance of the publish processing delegate.</param>
        /// <param name="servicingOrder">The order of the processing.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddRequestProcessingHandler<TRequest, TResponse>(this IServiceCollection services, RequestResponseDelegateAsync<TRequest, TResponse> requestDelegate, ServicingOrder servicingOrder = ServicingOrder.Processing) where TRequest : class where TResponse : class
        {
            if (requestDelegate == null)
            {
                throw new ArgumentNullException(nameof(requestDelegate));
            }
            services.AddSingleton<IRequestHandler<TRequest, TResponse>>(new RequestHandlerProcessingWrapper<TRequest, TResponse>(requestDelegate, servicingOrder));
            return services;
        }

        /// <summary>
        /// Adds services required for using of Notification Parallel.Mediator.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddMqNotificationMediator(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(ServiceDescriptor.Singleton(typeof(INotificationMediator<>), typeof(NotificationMediatorManager<>)));
            services.TryAdd(ServiceDescriptor.Singleton(typeof(INotificationMediatorFactory<>), typeof(ParallelNotificationMediatorFactory<>)));
            return services;
        }

        /// <summary>
        /// Add delegate of <see cref="NotificationDelegateAsync{TRequest}"/> to the services collection.
        /// </summary>
        /// <typeparam name="TNotification">The request type</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="notificationDelegate">The instance of the publish processing delegate.</param>
        /// <param name="servicingOrder">The order of the processing.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddNotificationProcessingHandler<TNotification>(this IServiceCollection services, NotificationDelegateAsync<TNotification> notificationDelegate, ServicingOrder servicingOrder = ServicingOrder.Processing) where TNotification : class
        {
            if (notificationDelegate == null)
            {
                throw new ArgumentNullException(nameof(notificationDelegate));
            }
            services.AddSingleton<INotificationHandler<TNotification>>(new NotificationHandlerProcessingWrapper<TNotification>(notificationDelegate, servicingOrder));
            return services;
        }

    }
}
