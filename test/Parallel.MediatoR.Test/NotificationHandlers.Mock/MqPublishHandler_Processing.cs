// Copyright © Alexander Paskhin 2021. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


using Parallel.MediatoR.Common;
using Parallel.MediatoR.Notification;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel.Mediator.Abstractions.Test.NotificationHandlers.Mock
{
    class MqPublishHandler_Processing : INotificationHandler<TestPublishNotication>
    {
        public ServicingOrder OrderInTheGroup => ServicingOrder.Processing;

        public Task ProcessNotificationAsync(TestPublishNotication request, CancellationToken cancellationToken)
        {
            request.Visitor.Add(OrderInTheGroup.ToString());
            return Task.Delay(1000, cancellationToken);
        }
    }

}
