using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Models
{
    internal class ScienceModel
    {
        internal float Ph = 0;
        internal short Moisture;

        internal short CCDPixelCount = 3184;
        internal float CCDProgress = 0f;
        internal string CCDFilePath = Environment.CurrentDirectory;
        internal bool CCDIsReceiving = false;
    }
}