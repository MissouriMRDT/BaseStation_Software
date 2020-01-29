
using RoverAttachmentManager.ViewModels.Arm;

namespace RoverAttachmentManager.Models.ArmModels
{
    internal class ArmModel
    {
        internal ControlMultipliersViewModel _controlMultipliers;
        internal ControlFeaturesViewModel _controlFeatures;
        internal AngularControlViewModel _angularControl;
        internal int IKRangeFactor = 1000;
        internal string ControlState;
        internal ArmPowerViewModel ArmPower;
        
    }
}
