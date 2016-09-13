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

#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging; 
#endregion
namespace Xigadee
{
    public abstract class AzureSBSenderBase<C, M> : MessagingSenderBase<C, M, AzureClientHolder<C, M>>
        where C : ClientEntity
    {
        #region Declarations
        /// <summary>
        /// This is the Azure connection class.
        /// </summary>
        protected AzureConnection mAzureSB;
        #endregion
        #region Constructor
        /// <summary>
        /// This is the default constructor.
        /// </summary>
        /// <param name="channelId">The internal channel id used to resolve the comms resource.</param>
        /// <param name="connectionString">The Azure connection string.</param>
        /// <param name="connectionName">The specific connection name to use.</param>
        public AzureSBSenderBase(string channelId
            , string connectionString
            , string connectionName
            , IEnumerable<SenderPartitionConfig> priorityPartitions
            , IBoundaryLogger boundaryLogger = null) 
            :base(channelId, priorityPartitions, boundaryLogger) 
        {
            mAzureSB = new AzureConnection() { ConnectionName = connectionName, ConnectionString = connectionString };
        } 
        #endregion


        protected override AzureClientHolder<C, M> ClientCreate(SenderPartitionConfig partition)
        {
            var client = base.ClientCreate(partition);

            client.Start = () =>
            {
                client.Client = client.ClientCreate();
                client.IsActive = true;
            };

            client.ClientClose = () => client.Client.Close();

            return client;
        }
    }
}
