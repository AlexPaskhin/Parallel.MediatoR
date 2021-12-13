// Copyright © Alexander Paskhin 2021. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


using System.Collections.Generic;

namespace Parallel.Mediator.InMem.Test.RequestHandlers.Mock
{

    public class TestSendRequest
    {
        public string Text { get; set; } = nameof(TestSendRequest);
        public List<string> Visitor { get; } = new List<string>();
    }

}
