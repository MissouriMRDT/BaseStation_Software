using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Models
{
    internal class ScienceModel
    {
        internal float Temperature1Value;
        internal float Temperature2Value;
        internal float Temperature3Value;
        internal float Temperature4Value;
        internal float Moisture1Value;
        internal float Moisture2Value;
        internal float Moisture3Value;
        internal float Moisture4Value;

        internal string CCDFilePath = Environment.CurrentDirectory;
    }
}