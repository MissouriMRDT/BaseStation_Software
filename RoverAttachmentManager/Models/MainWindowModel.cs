using Core;
using Core.Configurations;
using Core.RoveProtocol;
using Core.ViewModels.Input;
using Core.ViewModels.Input.Controllers;
using RoverAttachmentManager.ViewModels;
using RoverAttachmentManager.ViewModels.Arm;
using RoverAttachmentManager.ViewModels.Autonomy;

namespace RoverAttachmentManager.Models
{
    internal class MainWindowModel
    {
        internal Rovecomm _rovecomm;

        internal InputManagerViewModel _input;
        internal XboxControllerInputViewModel _xboxController;

        internal ArmViewModel _arm;
        internal CommonLog _console;
        internal MetadataManager _metadataManager;
        internal XMLConfigManager _configManager;
        internal AutonomyViewModel _autonomy;
    }
}
