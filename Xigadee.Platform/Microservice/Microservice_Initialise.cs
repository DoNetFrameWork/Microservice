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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#endregion
namespace Xigadee
{
    //TaskManager
    public partial class Microservice
    {
        #region InitialiseTaskManager()
        /// <summary>
        /// This method creates the task manager and sets the default bulkhead reservations.
        /// </summary>
        /// <returns></returns>
        protected virtual TaskManager InitialiseTaskManager()
        {
            var taskTracker = new TaskManager(4, Execute, PolicyTaskManager());

            taskTracker.BulkheadReserve(3, 1, 2);
            taskTracker.BulkheadReserve(2, 2, 2);
            taskTracker.BulkheadReserve(1, 8, 8);
            taskTracker.BulkheadReserve(0, 0);

            return taskTracker;
        }

        /// <summary>
        /// This method retrieves the policy for the task manager.
        /// </summary>
        /// <returns></returns>
        protected virtual TaskManagerPolicy PolicyTaskManager()
        {
            var policy = new TaskManagerPolicy();

            policy.ConcurrentRequestsMax = ConfigurationOptions.ConcurrentRequestsMax;
            policy.ConcurrentRequestsMin = ConfigurationOptions.ConcurrentRequestsMin;
            policy.AutotuneEnabled = ConfigurationOptions.SupportAutotune;
            policy.ProcessKillOverrunGracePeriod = ConfigurationOptions.ProcessKillOverrunGracePeriod;
            policy.ProcessorTargetLevelPercentage = ConfigurationOptions.ProcessorTargetLevelPercentage;

            return policy;
        } 
        #endregion

        #region InitialiseResourceTracker()
        /// <summary>
        /// This method creates the default resource tracker for the Microservice.
        /// Resource trackers are used to limit incoming messages that use a particular resource
        /// that is overloaded.
        /// </summary>
        /// <returns>Returns the resource tracker.</returns>
        protected virtual ResourceTracker InitialiseResourceTracker()
        {
            var container = new ResourceTracker();

            return container;
        }
        #endregion

        #region InitialiseCommandContainer()
        /// <summary>
        /// This method creates the component container.
        /// This container holds the jobs, message initiators and handlers and is used to 
        /// assign incoming requests to the appropriate command.
        /// </summary>
        /// <returns>Returns the container.</returns>
        protected virtual CommandContainer InitialiseCommandContainer()
        {
            var container = new CommandContainer(PolicyCommand());

            return container;
        }
        /// <summary>
        /// This is the policy used to set the communication component settings.
        /// </summary>
        /// <returns></returns>
        protected virtual CommandContainerPolicy PolicyCommand()
        {
            return new CommandContainerPolicy();
        }
        #endregion

        #region InitialiseCommunicationContainer()
        /// <summary>
        /// This method creates the communication container. This container comtains all the 
        /// listeners and senders registered on the service, and assigns priority when polling for 
        /// new incoming requests.
        /// </summary>
        /// <returns>The communication container.</returns>
        protected virtual CommunicationContainer InitialiseCommunicationContainer()
        {
            var container = new CommunicationContainer(PolicyCommunication());

            return container;
        }
        /// <summary>
        /// This is the policy used to set the communication component settings.
        /// </summary>
        /// <returns></returns>
        protected virtual CommunicationPolicy PolicyCommunication()
        {
            return new CommunicationPolicy();
        }
        #endregion


        #region InitialiseSchedulerContainer()
        /// <summary>
        /// This method returns the default scheduler container.
        /// </summary>
        /// <returns>The default scheduler.</returns>
        protected virtual SchedulerContainer InitialiseSchedulerContainer()
        {
            var container = new SchedulerContainer(PolicyScheduler());

            return container;
        }

        protected virtual SchedulerPolicy PolicyScheduler()
        {
            var policy = new SchedulerPolicy();
            
            //policy.DefaultPollInMs = ConfigurationOptions.

            return policy;
        }
        #endregion

        #region InitialiseSecurityContainer()
        /// <summary>
        /// This method creates the component container.
        /// This container holds the jobs, message initiators and handlers and is used to 
        /// assign incoming requests to the appropriate command.
        /// </summary>
        /// <returns>Returns the container.</returns>
        protected virtual SecurityContainer InitialiseSecurityContainer()
        {
            var container = new SecurityContainer(PolicySecurity());

            return container;
        }
        /// <summary>
        /// This is the policy used to set the communication component settings.
        /// </summary>
        /// <returns></returns>
        protected virtual SecurityPolicy PolicySecurity()
        {
            return new SecurityPolicy();
        }
        #endregion

        #region InitialiseSerializationContainer(List<IPayloadSerializer> payloadSerializers)
        /// <summary>
        /// THis method returns the default scheduler container.
        /// </summary>
        protected virtual SerializationContainer InitialiseSerializationContainer(List<IPayloadSerializer> payloadSerializers)
        {
            var container = new SerializationContainer(payloadSerializers);

            return container;
        }
        #endregion

        #region InitialiseDataCollectionContainer()
        /// <summary>
        /// This method creates the data collection container, which is responsible for logging, event source management, and telemetry.
        /// </summary>
        /// <returns>The data collection container.</returns>
        protected virtual DataCollectionContainer InitialiseDataCollectionContainer()
        {
            var container = new DataCollectionContainer(PolicyDataCollection());

            return container;
        }
        /// <summary>
        /// This is the policy used to set the data collection container settings.
        /// </summary>
        /// <returns>The policy</returns>
        protected virtual DataCollectionPolicy PolicyDataCollection()
        {
            return new DataCollectionPolicy();
        }
        #endregion
    }
}
