using Caliburn.Micro;
using RED.Configurations.Tools;
using RED.Contexts.Tools;
using RED.Interfaces;
using RED.Models.Tools;
using RED.ViewModels.Network;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RED.ViewModels.Tools
{
    public class PingToolViewModel : PropertyChangedBase
    {
        private readonly PingToolModel _model;
        private readonly IRovecomm _networkManager;

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
            private set
            {
                _model.PingServers = value;
                NotifyOfPropertyChange(() => PingServers);
            }
        }

        public PingToolViewModel(IRovecomm network, IConfigurationManager config)
        {
            _model = new PingToolModel();
            _networkManager = network;

            PingServers = new ObservableCollection<PingServer>();

            config.AddRecord<PingToolContext>(PingConfigName, PingToolConfig.DefaultPingToolConfig);
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
            private readonly PingToolModel.PingServerModel _model;
            private readonly PingToolViewModel _pingTool;

            public string Name
            {
                get
                {
                    return _model.Name;
                }
                private set
                {
                    _model.Name = value;
                    NotifyOfPropertyChange(() => Name);
                }
            }
            public IPAddress Address
            {
                get
                {
                    return _model.Address;
                }
                private set
                {
                    _model.Address = value;
                    NotifyOfPropertyChange(() => Address);
                }
            }
            public bool SupportsICMP
            {
                get
                {
                    return _model.SupportsICMP;
                }
                private set
                {
                    _model.SupportsICMP = value;
                    NotifyOfPropertyChange(() => SupportsICMP);
                }
            }
            public bool SupportsRoveComm
            {
                get
                {
                    return _model.SupportsRoveComm;
                }
                private set
                {
                    _model.SupportsRoveComm = value;
                    NotifyOfPropertyChange(() => SupportsRoveComm);
                }
            }
            public long Result
            {
                get
                {
                    return _model.Result;
                }
                private set
                {
                    _model.Result = value;
                    NotifyOfPropertyChange(() => Result);
                }
            }
            public bool IsSendingICMP
            {
                get
                {
                    return _model.IsSendingICMP;
                }
                private set
                {
                    _model.IsSendingICMP = value;
                    NotifyOfPropertyChange(() => IsSendingICMP);
                }
            }
            public bool IsSendingRoveComm
            {
                get
                {
                    return _model.IsSendingRoveComm;
                }
                private set
                {
                    _model.IsSendingRoveComm = value;
                    NotifyOfPropertyChange(() => IsSendingRoveComm);
                }
            }
            public bool AutoModeEnabled
            {
                get
                {
                    return _model.AutoModeEnabled;
                }
                private set
                {
                    _model.AutoModeEnabled = value;
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
                _model = new PingToolModel.PingServerModel();
                _pingTool = vm;
                Name = context.Name;
                IPAddress.TryParse(context.Address, out IPAddress ip);
                Address = ip;
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
    }
}
