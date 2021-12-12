// Copyright © Alexander Paskhin 2021. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace Parallel.MediatoR
{
    public interface IParallelMediatoR
    {
        /// <summary>
        /// Publish the notification for processing by the abstract set of <see cref="INotificationHandler{TRequest}"/>
        /// handlers. It returns the array of the completion tasks.
        /// The published notification has been processed in the grouped by <see cref="ServicingOrder"/> order.
        /// If there is more than one handler's group, so groups completed synchronously; i.e. there is a wait between groups.
        /// There is no timeout processing, so it should be provided in <see cref="INotificationHandler{TRequest}"/> implementation.
        /// </summary>
        /// <param name="notification">The send notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The array of tasks that indicates processing completion.</returns>
        Task[] PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken) where TNotification : class;

        /// <summary>
        /// Sends the request for processing by the abstract set of <see cref="IRequestHandler{TRequest, TResponse}"/>
        /// handlers. It returns the array of the responses.
        /// The request has been processed in the grouped by <see cref="ServicingOrder"/> order.
        /// If there is more than one priority group, so groups completed synchronously; i.e. there is a wait between groups.
        /// There is no timeout processing, so it should be provided in <see cref="IRequestHandler{TRequest, TResponse}"/> implementation.
        /// </summary>
        /// <param name="request">The send request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The array of tasks that indicates a processing completion.</returns>
        Task<TResponse>[] SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken) where TRequest : class where TResponse : class;
    }
}