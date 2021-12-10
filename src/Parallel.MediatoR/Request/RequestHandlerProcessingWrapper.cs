// Copyright © Alexander Paskhin 2020. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Parallel.MediatoR.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel.MediatoR.Request
{
    /// <summary>
    /// The wrappers to the request processing delegate.
    /// </summary>
    /// <typeparam name="TNotification">The requesttype.</typeparam>
    public class RequestHandlerProcessingWrapper<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        public ServicingOrder ServicingOrder { get; }

        public bool ForEachResponse { get; }

        private readonly RequestResponseDelegateAsync<TRequest, TResponse> _delegate;

        /// <summary>
        /// Constructs the wrapper class.
        /// </summary>
        /// <param name="requestDelegate">The processing delegate.</param>
        /// <param name="servicingOrder">The processing priority.</param>
        public RequestHandlerProcessingWrapper(RequestResponseDelegateAsync<TRequest, TResponse> requestDelegate, ServicingOrder servicingOrder = ServicingOrder.Processing, bool forEachResponse = false)
        {
            _delegate = requestDelegate ?? throw new ArgumentNullException(nameof(requestDelegate));
            ServicingOrder = servicingOrder;
            ForEachResponse = forEachResponse;
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task with a response result.</returns>
        public Task<TResponse> ProcessAsync(TRequest request, ParallelExecContext<TResponse> parallelExecContext, CancellationToken cancellationToken)
        {
            return _delegate(request, cancellationToken);
        }
    }
}