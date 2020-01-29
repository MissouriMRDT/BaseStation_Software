
using RoverAttachmentManager.ViewModels.Arm;
using RoverAttachmentManager.ViewModels;
using Core.ViewModels.Input.Controllers;
using Core.Configurations;

namespace RoverAttachmentManager.Models.ArmModels
{
    internal class ArmModel
    {
        internal ControlMultipliersViewModel _controlMultipliers;
        internal ControlFeaturesViewModel _controlFeatures;
        internal AngularControlViewModel _angularControl;
        internal InputManagerViewModel InputManager;
        internal XMLConfigManager _configManager;
        internal XboxControllerInputViewModel _xboxController1;
        internal XboxControllerInputViewModel _xboxController2;
        internal XboxControllerInputViewModel _xboxController3;
        internal int IKRangeFactor = 1000;
        internal string ControlState;
        internal ArmPowerViewModel ArmPower;
        
    }
}
