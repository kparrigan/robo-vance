using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using RoboVance.Roomba.Services;
using System.Configuration;
using RoboVance.Roomba.Core;

namespace RoboVance.WindowsAgent
{
    // TODO Some kind of heartbeat from the client so we can kill a zombied roomba. Brraaaiiiinnns
    // TODO Multiple device connections at a time
    // TODO Concurrency
    public partial class WindowsAgent : ServiceBase
    {
        #region Member Variables
        private Guid _agentId;
        private Thread _devicePollingWorker;
        private AutoResetEvent _stopRequest = new AutoResetEvent(false);
        private TimeSpan _devicePollingInterval;
        private String _baseUri;
        private BluetoothDiscoveryService _discoveryService;
        private HubConnection _connection;
        private IHubProxy _proxy;
        private IRoomba _roomba;
        #endregion

        #region Constructor
        public WindowsAgent()
        {
            InitService();
            InitProxy();
            InitializeComponent();
        }
        #endregion

        #region Protected Methods
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            _devicePollingWorker = new Thread(PollDevices);
            _devicePollingWorker.Start();
        }

        protected override void OnStop()
        {            
            _connection.Dispose();
            _stopRequest.Set();
            _devicePollingWorker.Join();
            base.OnStop();
        }
        #endregion

        #region Public Methods
        public void StartConsole(string[] args)
        {
            OnStart(args);
        }

        public void StopConsole()
        {     
            OnStop();
        }
        #endregion

        #region Private Methods
        private void InitService()
        {
            Int32 devicePollingInterval = 30;

            _agentId = Guid.NewGuid();
            Int32.TryParse(ConfigurationManager.AppSettings["DevicePollingInterval"], out devicePollingInterval);
            _devicePollingInterval = TimeSpan.FromSeconds(devicePollingInterval);
            _discoveryService = new BluetoothDiscoveryService();
            _baseUri = ConfigurationManager.AppSettings["HubUri"];
        }

        private void InitProxy()
        {
            _connection = new HubConnection(_baseUri, useDefaultUrl: true);
            _proxy = _connection.CreateHubProxy("deviceCommunicationHub");
            _connection.Start().Wait();
            _proxy.Invoke("InitAgent", _agentId);

            
            _proxy.On<String>("connectToDevice", (deviceName) =>
                ConnectToDevice(deviceName)
            );

            _proxy.On<String>("disconnectFromDevice", (deviceName) =>
                DisconnectFromDevice(deviceName)
            );

            _proxy.On<String, String>("doCommand", (deviceName, commandName) =>
                DoCommand(deviceName, commandName)
            );
        }

        private void ConnectToDevice(String deviceName)
        {
            try
            {
                if (_roomba != null)
                {
                    _roomba.Dispose();
                }

                //TODO verify that this agent has access to this device.
                _roomba = new RoombaCreate(deviceName, CommunicationMethod.Bluetooth);
                _roomba.Start();
                _roomba.Control(); //TODO Control or Full?
                _proxy.Invoke("ConnectedToDevice", _agentId, deviceName);       
            }
            catch(Exception ex)
            {
                // TODO log exception
                _proxy.Invoke("ConnectionFailure", _agentId, deviceName, ex);
            }
        }

        private void DisconnectFromDevice(String deviceName)
        {
            if (_roomba != null)
            {
                try
                {
                    _roomba.PowerDown();
                    _roomba.Dispose();
                }
                catch(Exception ex)
                {
                    // TODO log exception
                    // TODO failure message
                }
            }
        }

        private void DoCommand(String deviceName, String commandName)
        {
            if (_roomba != null)
            {
                try
                {
                    //TODO something better than this switch
                    switch (commandName.ToLower())
                    {
                        case "forward":
                            _roomba.Forward();
                            break;
                        case "reverse":
                            _roomba.Reverse();
                            break;
                        case "left":
                            _roomba.TurnLeft();
                            break;
                        case "right":
                            _roomba.TurnRight();
                            break;
                        case "stop":
                            _roomba.Stop();
                            break;
                        case "powerDown":
                            _roomba.PowerDown();
                            break;
                        default:
                            _roomba.Stop();
                            break;
                    }
                }
                catch(Exception ex)
                {
                    // TODO log exception
                    // TODO failure message
                }                
            }
        }

        private void PollDevices(object arg)
        {            
            while (true)
            {
                RegisterDevices();
                if (_stopRequest.WaitOne(_devicePollingInterval)) return;
            }
        }

        private void RegisterDevices()
        {
            try
            {
                var devices = _discoveryService.GetDeviceNames().ToList();
                _proxy.Invoke("RegisterDevices", _agentId, devices);
            }
            catch (Exception ex)
            {
                // TODO log to event log
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
