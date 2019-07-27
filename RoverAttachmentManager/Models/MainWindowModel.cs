using Core;
using Core.Configurations;
using Core.RoveProtocol;
using Core.ViewModels.Input;
using Core.ViewModels.Input.Controllers;
using RoverAttachmentManager.ViewModels;
using RoverAttachmentManager.ViewModels.Arm;
using RoverAttachmentManager.ViewModels.Autonomy;
using RoverAttachmentManager.ViewModels.Science;

namespace RoverAttachmentManager.Models
{
    internal class MainWindowModel
    {
        internal Rovecomm _rovecomm;

        internal InputManagerViewModel _input;
        internal XboxControllerInputViewModel _xboxController1;
        internal XboxControllerInputViewModel _xboxController2;
        internal XboxControllerInputViewModel _xboxController3;

        internal ArmViewModel _arm;
        internal CommonLog _console;
        internal MetadataManager _metadataManager;
        internal XMLConfigManager _configManager;
        internal AutonomyViewModel _autonomy;
        internal ScienceViewModel _science;
    }
}
