// Copyright © Alexander Paskhin 2021. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Parallel.Mediator.InMem.Test.RequestHandlers.Mock;
using Parallel.MediatoR;
using Parallel.MediatoR.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Parallel.Mediator.Abstractions.Test
{
    public class UnitTestParallelMediatoR
    {
        ServiceProvider _serviceProvider = null;

        public UnitTestParallelMediatoR()
        {

            ServiceCollection sc = new ServiceCollection();
            sc.AddParallelMediator();
            sc.AddParallelMediatorClasses(ServiceLifetime.Singleton, this.GetType().Assembly);
            _serviceProvider = sc.BuildServiceProvider();
        }

        [Fact]
        public async Task CallMediatorInterface()
        {
            // arrange
            var mediator = _serviceProvider.GetRequiredService<IParallelMediatoR>();

            // act
            var res = mediator.SendAsync<TestSendRequest, TestSendResponse>(new TestSendRequest());

            var finish = await Task.WhenAll(res);

            // assert
            Assert.Equal(1, finish.Length);

        }




    }

}
