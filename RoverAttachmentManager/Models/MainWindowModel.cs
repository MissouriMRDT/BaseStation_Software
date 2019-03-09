using Core;
using Core.Configurations;
using Core.RoveProtocol;
using Core.ViewModels.Input;
using RoverAttachmentManager.ViewModels.Arm;

namespace RoverAttachmentManager.Models
{
    internal class MainWindowModel
    {
        internal Rovecomm _rovecomm;
        internal InputManagerViewModel _input;
        
        internal ArmViewModel _arm;
        internal CommonLog _console;
        internal MetadataManager _metadataManager;
        internal XMLConfigManager _configManager;
    }
}
