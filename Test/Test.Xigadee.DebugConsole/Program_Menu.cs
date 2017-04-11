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

using Xigadee;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
namespace Test.Xigadee
{
    static partial class Program
    {
        static Lazy<ConsoleMenu> sMenuMain = new Lazy<ConsoleMenu>(
            () => new ConsoleMenu(
                "Xigadee Microservice Scratchpad Test Console"
                , new ConsoleOption(
                    "Load Settings"
                    , (m, o) =>
                    {
                        MicroserviceLoadSettings();
                    }
                    , enabled: (m, o) => sContext.Client.Status == ServiceStatus.Created && sContext.Server.Status == ServiceStatus.Created
                )
                , new ConsoleOption(
                    "Set Persistence storage options"
                    , (m, o) =>
                    {
                    }
                    , enabled: (m, o) => sContext.Server.Status == ServiceStatus.Created
                    , childMenu: sMenuServerPersistenceSettings.Value
                )
                , new ConsoleSwitchOption(
                    "Start Extension Service Test", (m, o) =>
                    {
                        Task.Run(() => ExtensionMicroserviceStart());
                        return true;
                    }
                    , "Stop Extension Service Test", (m, o) =>
                    {
                        Task.Run(() => ExtensionMicroserviceStop());
                        return true;
                    }
                    , shortcut: "startclient"
                )
                , new ConsoleSwitchOption(
                    "Start Client", (m, o) =>
                    {
                        Task.Run(() => MicroserviceClientStart());
                        return true;
                    }
                    , "Stop Client", (m, o) =>
                    {
                        Task.Run(() => MicroserviceClientStop());
                        return true;
                    }
                    , shortcut: "startclient"
                )
                , new ConsoleSwitchOption(
                    "Start WebAPI client", (m, o) =>
                    {
                        Task.Run(() => MicroserviceWebAPIStart());
                        return true;
                    }
                    , "Stop WebAPI client", (m, o) =>
                    {
                        MicroserviceWebAPIStop();
                        return true;
                    }
                    , shortcut: "startapi"
                )
                , new ConsoleSwitchOption(
                    "Start Server", (m, o) =>
                    {
                        Task.Run(() => MicroserviceServerStart());
                        return true;
                    }
                    ,"Stop Server", (m, o) =>
                    {
                        Task.Run(() => MicroserviceServerStop());
                        return true;
                    }
                    , shortcut: "startserver"
                )
                , new ConsoleOption("Client Persistence methods"
                    , (m, o) =>
                    {
                        //sContext.ClientPersistence = () => sContext.Client.Status;
                    }
                    , childMenu: sMenuClientPersistence.Value
                    , enabled: (m, o) => sContext.Client.Status == ServiceStatus.Running
                )
                , new ConsoleOption("API Client Persistence methods"
                    , (m, o) =>
                    {
                        //sContext.ApiPersistenceStatus = () => 2;
                    }
                    , childMenu: sMenuApiPersistence.Value
                    , enabled: (m, o) => sContext.ApiServer.Status == ServiceStatus.Running
                )
                , new ConsoleOption("Server Shared Service Persistence methods"
                    , (m, o) =>
                    {
                        //sContext.PersistenceStatus = () => sContext.Server.Status;
                    }
                    , childMenu: sMenuServerPersistence.Value
                    , enabled: (m, o) => sContext.Server.Status == ServiceStatus.Running
                )
                , new ConsoleOption("Client Stress Tests"
                    , (m, o) =>
                    {
                    }
                    , childMenu: sMenuClientStressTests.Value
                    , enabled: (m, o) => sContext.Client.Status == ServiceStatus.Running
                )
                , new ConsoleOption("Server Stress Tests"
                    , (m, o) =>
                    {
                    }
                    , childMenu: sMenuClientStressTests.Value
                    , enabled: (m, o) => sContext.Server.Status == ServiceStatus.Running
                )
            ));
    }
}
