// Copyright © Alexander Paskhin 2021. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Parallel.MediatoR.Common;

namespace Parallel.MediatoR.Request
{

    /// <summary>
    /// The struct defines of passed context between servicing ordres  groups. 
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public struct ParallelExecContext<TResponse>
    {
        /// <summary>
        /// Defines the previous results in servicing order chain.
        /// </summary>
        public TResponse[] PrevResponses { get; set; }

        /// <summary>
        /// Defines the servicing order of the handler
        /// </summary>
        public ServicingOrder ServicingOrder { get; set; }

        /// <summary>
        /// Invocation index for this task.
        /// </summary>
        public int InvocationIndex { get; set; }
    }

}
