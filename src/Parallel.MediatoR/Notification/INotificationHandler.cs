// Copyright © Alexander Paskhin 2021. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Parallel.MediatoR.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel.MediatoR.Notification;

/// <summary>
/// Defines the notification processing handler. 
/// It gets "TNotification" and return Task when it is completed.
/// </summary>
/// <typeparam name="TNotification">The notification type.</typeparam>
public interface INotificationHandler<in TNotification> where TNotification : class
{
    /// <summary>
    /// The processing priority.
    /// </summary>
    ServicingOrder OrderInTheGroup { get; }

    /// <summary>
    /// Handles the notification.
    /// </summary>
    /// <param name="notification">The notification.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task which is completed when a notification has been processed.</returns>
    Task ProcessNotificationAsync(TNotification notification, CancellationToken cancellationToken);
}
