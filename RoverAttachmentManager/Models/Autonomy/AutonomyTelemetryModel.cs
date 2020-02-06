using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverAttachmentManager.Models.Autonomy
{
    internal class AutonomyTelemetryModel
    {
        internal string CurrentState;
        internal string TBD;
        internal short Left;
        internal short Right;
        internal float CurrentLatitude;
        internal float CurrentLongitude;
        internal float TargetLatitude;
        internal float TargetLongitude;
        internal float Pitch;
        internal float Roll;
        internal float Heading;
        internal float TargetHeading;
        internal float LiDAR;
    }
}
