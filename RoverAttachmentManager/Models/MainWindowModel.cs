using RED.Roveprotocol;
using RED.ViewModels;
using RED.ViewModels.Input;
using RED.ViewModels.Network;
using RoverAttachmentManager.ViewModels.Arm;

namespace RoverAttachmentManager.Models
{
    internal class MainWindowModel
    {
        internal Rovecomm _rovecomm;
        internal InputManagerViewModel _input;
        
        internal ArmViewModel _arm;
        internal ConsoleViewModel _console;
        internal MetadataManager _metadataManager;
        internal XMLConfigManager _configManager;
        internal NetworkManagerViewModel _networkManager;
    }
}
