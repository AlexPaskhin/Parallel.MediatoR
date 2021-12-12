// Copyright © Alexander Paskhin 2021. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Parallel.MediatoR.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel.MediatoR.Request
{

    /// <summary>
    /// Defines the request processing handler. It gets "TRequest" and returns "TResponse" when it is completed.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public interface IRequestHandler<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        /// <summary>
        /// The processing priority.
        /// </summary>
        ServicingOrder ServicingOrder { get; }

        bool ForEarchResponse { get; }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="parallelExecContext">The processasync execution context</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task with a response result.</returns>
        Task<TResponse> ProcessAsync(TRequest request, ParallelExecContext<TResponse> parallelExecContext, CancellationToken cancellationToken);
    }

}
