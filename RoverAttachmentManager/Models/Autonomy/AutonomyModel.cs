using Core;
using RoverAttachmentManager.ViewModels;
using System;

namespace RoverAttachmentManager.Models.Autonomy
{
    internal class AutonomyModel
    {
        internal WaypointManager Manager;
        internal InputManagerViewModel InputManager;
        internal string _waypointsText = String.Empty;
    }
}
