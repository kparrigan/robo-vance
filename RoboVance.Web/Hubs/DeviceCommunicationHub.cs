using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace RoboVance.Web.Hubs
{
    public class DeviceCommunicationHub : Hub
    {
        public void RegisterDevices(Guid agentId, IList<String> deviceNames)
        {
            Globals.AgentDictionary.AddOrUpdate(agentId, DateTime.UtcNow, (key, value) => DateTime.UtcNow);
            Globals.AgentDevices.AddOrUpdate(agentId, deviceNames, (key, value) => deviceNames);
            Clients.All.registeredDevices(Globals.AgentDevices.Select(d => new {id = d.Key, devices = d.Value}));
        }

        public void ConnectedToDevice(Guid agentId, String deviceName)
        {
            Clients.All.connectedToDevice(new { AgentId = agentId, DeviceName = deviceName });
        }

        public void ConnectionFailure(Guid agentId, String deviceName, Exception ex)
        {
            Clients.All.connectionFailure(agentId, deviceName);
        }

        public void ConnectToDevice(Guid agentId, String deviceName)
        {
            Clients.All.connectToDevice(agentId, deviceName);
        }

        public void SendCommand(Guid agentId, String deviceName, String commandName)
        {
            Clients.All.doCommand(agentId, deviceName, commandName);
        }
    }
}