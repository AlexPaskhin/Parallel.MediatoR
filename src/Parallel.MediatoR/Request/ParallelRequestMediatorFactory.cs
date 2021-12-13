// Copyright © Alexander Paskhin 2021. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Parallel.MediatoR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel.MediatoR.Request
{



    /// <summary>
    /// The default implementation of the <see cref="IRequestMediatorFactory{TRequest, TResponse}"/>.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public class ParallelRequestMediatorFactory<TRequest, TResponse> : IRequestMediatorFactory<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        readonly IEnumerable<IRequestHandler<TRequest, TResponse>> _handlers;
        IRequestMediator<TRequest, TResponse> _cached;


        /// <summary>
        /// Constructs the instance of the class.
        /// </summary>
        /// <param name="handlers">The list of handlers.</param>
        public ParallelRequestMediatorFactory(IEnumerable<IRequestHandler<TRequest, TResponse>> handlers)
        {
            _handlers = handlers;
        }

        /// <summary>
        /// Creates the <see cref="IRequestMediator{TRequest, TResponse}"/> instance.
        /// </summary>
        /// <returns>The <see cref="IRequestMediator{TRequest, TResponse}"/> instance.</returns>
        public IRequestMediator<TRequest, TResponse> CreateMqMediator()
        {
            if (_cached == null)
            {
                var executionSendSequence = new SortedList<ServicingOrder, List<IRequestHandler<TRequest, TResponse>>>(_handlers.Count());
                foreach (var item in _handlers)
                {
                    if (executionSendSequence.ContainsKey(item.ServicingOrder))
                    {
                        executionSendSequence[item.ServicingOrder].Add(item);
                    }
                    else
                    {
                        executionSendSequence[item.ServicingOrder] = new List<IRequestHandler<TRequest, TResponse>>() { item };
                    }

                }
                var maxSize = executionSendSequence.Count > 0 ? executionSendSequence.Max(s => s.Value.Count) : 0;
                _cached = new ParallelPostMediatorProvider(executionSendSequence, maxSize);
            }

            return _cached;
        }

        private class ParallelPostMediatorProvider : IRequestMediator<TRequest, TResponse>
        {
            private readonly CancellationTokenSource cancelled = new CancellationTokenSource();
            readonly SortedList<ServicingOrder, List<IRequestHandler<TRequest, TResponse>>> _executionSendSequence;
            readonly int _maxSize;

            public ParallelPostMediatorProvider(SortedList<ServicingOrder, List<IRequestHandler<TRequest, TResponse>>> executionSendSequence, int maxSize)
            {
                _maxSize = maxSize;
                _executionSendSequence = executionSendSequence;
                cancelled.Cancel();
            }

            public override bool Equals(object obj) => obj is ParallelPostMediatorProvider provider && _maxSize == provider._maxSize;
            public override int GetHashCode() => HashCode.Combine(_maxSize);

            /// <summary>
            /// Sends the request for processing by the abstract set of <see cref="IRequestHandler{TRequest, TResponse}"/>
            /// handlers. It returns the array of the responses.
            /// The request has been processed in the grouped by <see cref="ServicingOrder"/> order.
            /// If there is more than one priority group, so groups completed synchronously; i.e. there is a wait between groups.
            /// There is no timeout processing, so it should be provided in <see cref="IRequestHandler{TRequest, TResponse}"/> implementation.
            /// </summary>
            /// <param name="request">The send request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <returns>The array of tasks that indicates processing completion.</returns>
            public Task<TResponse>[] SendAsync(TRequest request, CancellationToken cancellationToken)
            {
                if (_maxSize == 0)
                {
                    return new Task<TResponse>[] { };
                }

                List<Task<TResponse>> result = new List<Task<TResponse>>(_maxSize);

                var indexRes = 0;
                var indexGrp = 0;
                var iG = 0;
                var forceTheCancellation = false;
                var parallelExecContext = new ParallelExecContext<TResponse>();
                parallelExecContext.PrevResponses = new TResponse[0];

                foreach (var group in _executionSendSequence)
                {
                    var list = group.Value;
                    /* execute in parallel */
                    foreach (var handler in group.Value)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            result.Add(Task.FromCanceled<TResponse>(cancellationToken));
                        }
                        else if (forceTheCancellation)
                        {
                            result.Add(Task.FromCanceled<TResponse>(cancelled.Token));
                        }
                        else
                        {
                            try
                            {
                                parallelExecContext.InvocationIndex = indexRes;
                                parallelExecContext.ServicingOrder = group.Key;

                                result.Add(handler.ProcessAsync(request, parallelExecContext, cancellationToken));
                            }
                            catch (Exception ae)
                            {
                                result.Add(Task.FromException<TResponse>(ae));
                                forceTheCancellation = true;
                            }
                        }
                        indexRes++;
                    }

                    if (++iG == _executionSendSequence.Count)
                    {
                        // In most cases it is a single group, so we are to return the tasks
                        break;
                    }

                    if (!cancellationToken.IsCancellationRequested && !forceTheCancellation)
                    {
                        if (list.Count > 0)
                        {
                            var waitList = result;
                            try
                            {
                                Task.WhenAll(waitList).Wait(cancellationToken);
                                parallelExecContext.PrevResponses = waitList.Select(x => x.Result).ToArray();
                                result.Clear();
                            }
                            catch
                            {
                                forceTheCancellation = true;
                            }
                        }
                    }

                    indexGrp += list.Count;
                }

                return result.ToArray();
            }
        }
    }
}
