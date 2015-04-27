using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Models
{
    internal class DriveControllerModeModel
    {
        internal int speedLimit;
        internal DriveScalingMode ScalingMode;
    }

    public enum DriveScalingMode : byte
    {
        Linear = 0,
        Parabolic = 1
    }
}
