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
        public void InitAgent(Guid agentId)
        {
            Globals.AgentTimestamps.AddOrUpdate(agentId, DateTime.UtcNow, (key, value) => DateTime.UtcNow);
            Globals.AgentConnectionIds.AddOrUpdate(agentId, Context.ConnectionId, (key, value) => Context.ConnectionId);
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var agentId = Globals.AgentConnectionIds.FirstOrDefault(kv => String.Equals(kv.Value, Context.ConnectionId)).Key;
            
            if (agentId != null)
            {
                DateTime timestamp;
                String connectionId;
                IList<String> devices;

                Globals.AgentTimestamps.TryRemove(agentId, out timestamp);
                Globals.AgentConnectionIds.TryRemove(agentId, out connectionId);
                Globals.AgentDevices.TryRemove(agentId, out devices);
            }

            return base.OnDisconnected(stopCalled);
        }

        public void RegisterDevices(Guid agentId, IList<String> deviceNames)
        {
            Globals.AgentTimestamps.AddOrUpdate(agentId, DateTime.UtcNow, (key, value) => DateTime.UtcNow);
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
            var connectionId = Globals.AgentConnectionIds[agentId];
            Clients.Client(connectionId).connectToDevice(deviceName);
        }

        public void SendCommand(Guid agentId, String deviceName, String commandName)
        {
            var connectionId = Globals.AgentConnectionIds[agentId];
            Clients.Client(connectionId).doCommand(deviceName, commandName);
        }
    }
}