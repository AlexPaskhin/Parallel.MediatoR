// Copyright © Alexander Paskhin 2020. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


using Parallel.MediatoR.Common;
using Parallel.MediatoR.Notification;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel.Mediator.Abstractions.Test.NotificationHandlers.Mock
{
    class MqPublishHandler_PreProcessing : INotificationHandler<TestPublishNotication>
    {
        public ServicingOrder OrderInTheGroup => ServicingOrder.PreProcessing;

        public Task ProcessNotification(TestPublishNotication request, CancellationToken cancellationToken)
        {
            request.Visitor.Add(OrderInTheGroup.ToString());
            return Task.Delay(1000, cancellationToken);
        }
    }

}