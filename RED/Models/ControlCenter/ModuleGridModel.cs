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
        internal string leftSelection;
        internal string rightSelection;
        internal string topSelection;
        internal string middleSelection;
        internal string bottomSelection;
        internal IModule leftModule;
        internal IModule rightModule;
        internal IModule topModule;
        internal IModule middleModule;
        internal IModule bottomModule;
        internal string column1Width = "1*";
        internal string column3Width = "2*";
        internal string column5Width = "1*";
        internal string row1Height = "1*";
        internal string row3Height = "1*";
        internal string row5Height = "1*";
    }
}