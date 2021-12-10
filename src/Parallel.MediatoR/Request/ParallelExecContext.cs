// Copyright © Alexander Paskhin 2020. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Parallel.MediatoR.Common;

namespace Parallel.MediatoR.Request
{
    public struct ParallelExecContext<TResponse>
    {
        public TResponse[] PrevResponses { get; set; }
        public ServicingOrder ServicingOrder { get; set; }
        public int InvocationIndex { get; set; }

    }

}
