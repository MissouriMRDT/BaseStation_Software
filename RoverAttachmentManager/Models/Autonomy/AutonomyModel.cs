using Core;
using System;
using RoverAttachmentManager.ViewModels.Autonomy;

namespace RoverAttachmentManager.Models.Autonomy
{
    internal class AutonomyModel
    {
        internal WaypointManager Manager;
        internal ControlsViewModel Controls;
        internal StateControlViewModel _stateControl;
        internal SentWaypointsViewModel _sentWaypoints;
    }
}
