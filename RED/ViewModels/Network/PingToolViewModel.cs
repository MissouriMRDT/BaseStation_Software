﻿using Caliburn.Micro;
using RED.Contexts;
using RED.Interfaces;
using RED.Models.Network;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RED.ViewModels.Network
{
    public class PingToolViewModel : PropertyChangedBase
    {
        private PingToolModel _model;
        private NetworkManagerViewModel _networkManager;

        private const string PingConfigName = "PingTool";

        public int AutoRate
        {
            get
            {
                return _model.AutoRate;
            }
            set
            {
                _model.AutoRate = value;
                NotifyOfPropertyChange(() => AutoRate);
            }
        }
        public int Timeout
        {
            get
            {
                return _model.Timeout;
            }
            set
            {
                _model.Timeout = value;
                NotifyOfPropertyChange(() => Timeout);
            }
        }

        public ObservableCollection<PingServer> PingServers
        {
            get
            {
                return _model.PingServers;
            }
            set
            {
                _model.PingServers = value;
                NotifyOfPropertyChange(() => PingServers);
            }
        }

        public PingToolViewModel(NetworkManagerViewModel network, IConfigurationManager config)
        {
            _model = new PingToolModel();
            _networkManager = network;

            PingServers = new ObservableCollection<PingServer>();

            config.AddRecord<PingToolContext>(PingConfigName, DefaultPingToolConfig);
            var context = config.GetConfig<PingToolContext>(PingConfigName);
            InitializeServers(context);
            AutoRate = context.AutoRate;
            Timeout = context.Timeout;
        }

        private void InitializeServers(PingToolContext context)
        {
            foreach (var server in context.Servers)
                PingServers.Add(new PingServer(this, server));
        }

        private async Task<long> SendICMPPing(IPAddress ip)
        {
            using (Ping ICMPPing = new Ping())
            {
                var reply = await ICMPPing.SendPingAsync(ip, Timeout);
                return (reply.Status == IPStatus.Success) ? reply.RoundtripTime : -1;
            }
        }

        private async Task<long> SendRoveCommPing(IPAddress ip)
        {
            TimeSpan roundtripTime = await _networkManager.SendPing(ip, TimeSpan.FromMilliseconds(Timeout));
            long time = (long)roundtripTime.TotalMilliseconds;
            return (time != 0) ? time : -1;
        }

        public class PingServer : PropertyChangedBase
        {
            private PingToolViewModel _pingTool;

            private string _Name;
            private IPAddress _Address;
            private bool _SupportsICMP;
            private bool _SupportsRoveComm;
            private long _Result;
            private bool _IsSendingICMP;
            private bool _IsSendingRoveComm;
            private bool _AutoModeEnabled;

            public string Name
            {

                get
                {
                    return _Name;
                }
                private set
                {
                    _Name = value;
                    NotifyOfPropertyChange(() => Name);
                }
            }
            public IPAddress Address
            {
                get
                {
                    return _Address;
                }
                private set
                {
                    _Address = value;
                    NotifyOfPropertyChange(() => Address);
                }
            }
            public bool SupportsICMP
            {
                get
                {
                    return _SupportsICMP;
                }
                private set
                {
                    _SupportsICMP = value;
                    NotifyOfPropertyChange(() => SupportsICMP);
                }
            }
            public bool SupportsRoveComm
            {
                get
                {
                    return _SupportsRoveComm;
                }
                private set
                {
                    _SupportsRoveComm = value;
                    NotifyOfPropertyChange(() => SupportsRoveComm);
                }
            }
            public long Result
            {
                get
                {
                    return _Result;
                }
                private set
                {
                    _Result = value;
                    NotifyOfPropertyChange(() => Result);
                }
            }
            public bool IsSendingICMP
            {
                get
                {
                    return _IsSendingICMP;
                }
                private set
                {
                    _IsSendingICMP = value;
                    NotifyOfPropertyChange(() => IsSendingICMP);
                }
            }
            public bool IsSendingRoveComm
            {
                get
                {
                    return _IsSendingRoveComm;
                }
                private set
                {
                    _IsSendingRoveComm = value;
                    NotifyOfPropertyChange(() => IsSendingRoveComm);
                }
            }
            public bool AutoModeEnabled
            {
                get
                {
                    return _AutoModeEnabled;
                }
                set
                {
                    _AutoModeEnabled = value;
                    if (AutoModeEnabled)
                        EnableAutoMode();
                    else
                        DisableAutoMode();
                    NotifyOfPropertyChange(() => AutoModeEnabled);
                }
            }

            private DispatcherTimer autoTimer;

            public PingServer(PingToolViewModel vm, PingServerContext context)
            {
                _pingTool = vm;
                Name = context.Name;
                IPAddress ip;
                IPAddress.TryParse(context.Address, out ip);
                Address = ip ?? null;
                SupportsICMP = context.SupportsICMP;
                SupportsRoveComm = context.SupportsRoveComm;
                Result = 0;
            }

            public async void RequestICMPPing()
            {
                if (!SupportsICMP) return;

                if (AutoModeEnabled)
                {
                    IsSendingICMP = !IsSendingICMP;
                }
                else
                {
                    IsSendingICMP = true;
                    await SendICMPPing();
                    IsSendingICMP = false;
                }
            }
            public async Task SendICMPPing()
            {
                Result = await _pingTool.SendICMPPing(Address);
            }

            public async void RequestRoveCommPing()
            {
                if (!SupportsRoveComm) return;

                if (AutoModeEnabled)
                {
                    IsSendingRoveComm = !IsSendingRoveComm;
                }
                else
                {
                    IsSendingRoveComm = true;
                    await SendRoveCommPing();
                    IsSendingRoveComm = false;
                }
            }
            public async Task SendRoveCommPing()
            {
                Result = await _pingTool.SendRoveCommPing(Address);
            }

            private void EnableAutoMode()
            {
                autoTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_pingTool.AutoRate) };
                autoTimer.Tick += AutoModeTick;
                autoTimer.Start();
            }
            private void DisableAutoMode()
            {
                autoTimer.Stop();
            }
            private async void AutoModeTick(object sender, EventArgs e)
            {
                if (SupportsICMP && IsSendingICMP)
                    await SendICMPPing();
                if (SupportsRoveComm && IsSendingRoveComm)
                    await SendRoveCommPing();
            }
        }

        public static PingToolContext DefaultPingToolConfig = new PingToolContext(1000, 1000, new[] { 
            new PingServerContext("Base Rocket", "192.168.1.83", true, false),
            new PingServerContext("Rover Rocket", "192.168.1.82", true, false),
            new PingServerContext("Drive Board", "192.168.1.130", true, true),
            new PingServerContext("Arm Board", "192.168.1.131", true, true),
            new PingServerContext("Power Board", "192.168.1.132", true, true),
            new PingServerContext("Nav Board", "192.168.1.133", true, true),
            new PingServerContext("External Controls Board", "192.168.1.134", true, true),
            new PingServerContext("Science Board", "192.168.1.135", true, true),
            new PingServerContext("Autonomy Board", "192.168.1.138", true, true),
            new PingServerContext("Science Arm Board", "192.168.1.139", true, true),
            new PingServerContext("Camera Board", "192.168.1.140", true, true),
            new PingServerContext("Grandstream", "192.168.1.226", true, false)
        });
    }
}