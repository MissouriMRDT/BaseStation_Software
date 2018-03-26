using RED.Addons.Network;
using RED.Configurations;
using RED.Contexts;
using RED.Interfaces;
using RED.Interfaces.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RED.Roveprotocol
{
    public class MetadataManager : IIPAddressProvider, IDataIdResolver, IServerProvider
    {
        private readonly ILogger _log;
        private readonly IConfigurationManager _configManager;

        private const string MetadataConfigName = "DataIdMetadata";

        public List<MetadataServerContext> Servers { get; }
        public List<MetadataRecordContext> Commands { get; }
        public List<MetadataRecordContext> Telemetry { get; }

        private List<Server> ServerObjs;

        public MetadataManager(ILogger log, IConfigurationManager configs)
        {
            _log = log;
            _configManager = configs;

            _configManager.AddRecord(MetadataConfigName, MetadataManagerConfig.DefaultMetadata);

            Servers = new List<MetadataServerContext>();
            Commands = new List<MetadataRecordContext>();
            Telemetry = new List<MetadataRecordContext>();

            ServerObjs = new List<Server>();

            InitializeMetadata(_configManager.GetConfig<MetadataSaveContext>(MetadataConfigName));
        }

        public void InitializeMetadata(MetadataSaveContext config)
        {
            Servers.AddRange(config.Servers);

            foreach (var server in Servers)
            {
                Commands.AddRange(server.Commands);
                Telemetry.AddRange(server.Telemetry);
            }

            ServerObjs.AddRange(Servers.Select(x => new Server(x)));

            _log.Log("Metadata loaded");
        }

        public MetadataServerContext GetServer(ushort dataId)
        {
            return Servers.FirstOrDefault(x => x.Commands.Concat(x.Telemetry).Any(y => y.Id == dataId));
        }
        public MetadataRecordContext GetMetadata(ushort dataId)
        {
            return Telemetry.Concat(Commands).FirstOrDefault(x => x.Id == dataId);
        }
        public MetadataRecordContext GetMetadata(string name)
        {
            return Commands.Concat(Telemetry).FirstOrDefault(x => x.Name == name);
        }

        public ushort GetId(string name)
        {
            var data = GetMetadata(name);
            if (data == null)
            {
                _log.Log($"DataId for \"{name}\" not found");
                return (ushort)0;
            }
            return data.Id;
        }
        public string GetName(ushort DataId)
        {
            var data = GetMetadata(DataId);
            return data?.Name ?? String.Empty;
        }
        public string GetServerAddress(ushort DataId)
        {
            var data = GetServer(DataId);
            return data?.Address ?? String.Empty;
        }

        public IPAddress GetIPAddress(ushort dataId)
        {
            if (IPAddress.TryParse(GetServerAddress(dataId), out IPAddress ip))
                return ip;
            else
            {
                _log.Log($"Error Parsing IP Address for DataId {dataId}");
                return null;
            }
        }
        public ushort[] GetAllDataIds(IPAddress ip)
        {
            var server = Servers.FirstOrDefault(x => x.Address == ip.ToString());
            if (server == null)
                return new ushort[0];
            return server.Commands.Concat(server.Telemetry).Select(x => x.Id).ToArray();
        }

        public Server[] GetServerList()
        {
            return ServerObjs.ToArray();
        }
        public Server GetServer(IPAddress ip)
        {
            return ServerObjs.FirstOrDefault(x => x.Address.Equals(ip));
        }

        public IPAddress[] GetAllIPAddresses()
        {
            Server[] servers = GetServerList();

            IPAddress[] addresses = new IPAddress[servers.Length];

            for(int i = 0; i < servers.Length; i++)
            {
                addresses[i] = servers[i].Address;
            }

            return addresses;
        }
    }
}