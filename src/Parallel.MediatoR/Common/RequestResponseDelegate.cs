// Copyright © Alexander Paskhin 2021. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Parallel.MediatoR.Request;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel.MediatoR.Common
{
    /// <summary>
    /// The delegate that handles a request.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <param name="request">The send request.</param>
    /// <param name="parallelExecContext">The processasync execution context</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task with a completion result.</returns>
    public delegate Task<TResponse> RequestResponseDelegateAsync<TRequest, TResponse>(
        TRequest request,
        ParallelExecContext<TResponse> parallelExecContext,
        CancellationToken cancellationToken) where TRequest : class where TResponse : class;
}