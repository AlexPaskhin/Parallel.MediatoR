// Copyright © Alexander Paskhin 2021. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Parallel.Mediator.InMem.Test.RequestHandlers.Mock
{
    public class TestSendResponse
    {
        public string Text { get; set; } = nameof(TestSendResponse);
        public string Response { get; set; }
    }

}
