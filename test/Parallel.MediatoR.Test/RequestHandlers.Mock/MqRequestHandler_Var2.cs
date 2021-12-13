// Copyright © Alexander Paskhin 2021. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Parallel.MediatoR.Common;
using Parallel.MediatoR.Request;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel.Mediator.InMem.Test.RequestHandlers.Mock
{
    public class MqRequestHandler_Var2 : IRequestHandler<TestSendRequest, TestSendResponse>
    {
        public ServicingOrder ServicingOrder => ServicingOrder.PostProcessing;

        public bool ForEarchResponse => false;

        public async Task<TestSendResponse> ProcessAsync(TestSendRequest request, ParallelExecContext<TestSendResponse> parallelExecContext, CancellationToken cancellationToken)
        {
            request.Visitor.Add(ServicingOrder.ToString());
            await Task.Delay(1000, cancellationToken);
            return new TestSendResponse() { Response = ServicingOrder.ToString() + ":" + nameof(MqRequestHandler_Var1) };

        }
    }

}
