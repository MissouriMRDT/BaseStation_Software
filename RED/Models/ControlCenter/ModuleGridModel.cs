using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RED.Interfaces;

namespace RED.Models.ControlCenter
{
    public class ModuleGridModel
    {
        internal string LeftSelection;
        internal string RightSelection;
        internal string TopSelection;
        internal string MiddleSelection;
        internal string BottomSelection;
        internal IModule LeftModule;
        internal IModule RightModule;
        internal IModule TopModule;
        internal IModule MiddleModule;
        internal IModule BottomModule;
        internal string Column1Width = "1*";
        internal string Column3Width = "2*";
        internal string Column5Width = "1*";
        internal string Row1Height = "1*";
        internal string Row3Height = "1*";
        internal string Row5Height = "1*";
    }
}