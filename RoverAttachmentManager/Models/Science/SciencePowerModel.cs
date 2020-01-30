using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverAttachmentManager.Models.Science
{
    class SciencePowerModel
    {
        internal bool AutoStartLog;

        internal float WOActuationCurrent1;
        internal float WOActiationCurrent2;
        internal float GenevaCurrent;
        internal float VacuumCurrent;
        internal float FluidPumpCurrent1;
        internal float FluidPumpCurrent2;
        internal float FluidPumpCurrent3;

        internal BitArray Status;
    }
}
