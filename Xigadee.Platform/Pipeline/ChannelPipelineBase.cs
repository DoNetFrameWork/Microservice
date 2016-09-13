﻿#region Copyright
// Copyright Hitachi Consulting
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xigadee
{
    /// <summary>
    /// This is the outgoing channel.
    /// </summary>
    public class ChannelPipelineOutgoing: ChannelPipelineBase
    {
        public ChannelPipelineOutgoing(MicroservicePipeline pipeline, Channel channel) : base(pipeline, channel)
        {

        }
    }

    /// <summary>
    /// This is the incoming channel.
    /// </summary>
    public class ChannelPipelineIncoming: ChannelPipelineBase
    {
        public ChannelPipelineIncoming(MicroservicePipeline pipeline, Channel channel) : base(pipeline, channel)
        {

        }
    }

    /// <summary>
    /// This class is used to hold the pipeline configuration.
    /// </summary>
    public abstract class ChannelPipelineBase
    {
        public ChannelPipelineBase(MicroservicePipeline pipeline, Channel channel)
        {
            Pipeline = pipeline;
            Channel = channel;
        }

        /// <summary>
        /// This is the configuration pipeline.
        /// </summary>
        public MicroservicePipeline Pipeline { get; }
        /// <summary>
        /// This is the registered channel
        /// </summary>
        public Channel Channel { get; }

    }
}
