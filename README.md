# Parallel.Mediator

The simple abstract, Dependency Inversion solution for communicating between 
Application hosted MicroServices.

## Version 3.1.0
 - NetStandard 2.1
 - NetCoreApp  3.1

## Nuget packages:

### Abstractions:
- https://www.nuget.org/packages/Parallel.Mediator.Abstractions

### Event Bus for Rabbit MQ
- https://www.nuget.org/packages/Parallel.Mediator.EventBus.RabbitMq
  - https://www.nuget.org/packages/Parallel.Mediator.Abstractions

### "Notification" for Application MicroServices:

- https://www.nuget.org/packages/Parallel.Mediator.Notification.InMem
  - https://www.nuget.org/packages/Parallel.Mediator.Abstractions

### "Request" for Application MicroServices:

- https://www.nuget.org/packages/Parallel.Mediator.Request.InMem
  - https://www.nuget.org/packages/Parallel.Mediator.Abstractions


# How to use

The Parallel.Mediator implementations are based on using **Microsoft.Extensions.DependencyInjection**
dependency injection container, but they can be adopted to other types DI containers.


## Message Queue Notifications

Based on using package: Parallel.Mediator.Notification.InMem
### How to use

## Message Queue Requests
Based on using package: Parallel.Mediator.Request.InMem
### How to use

## Message Queue Event Bus
Based on using package: Parallel.Mediator.EventBus.RabbitMq
### How to use



