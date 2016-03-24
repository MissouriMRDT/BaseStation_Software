using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RED.Interfaces;
using System.Collections.ObjectModel;

namespace RED.Models
{
    public class InputManagerModel
    {
        internal IInputDevice _input;
        internal int SerialReadSpeed;
        internal string SelectedDevice;
        internal ObservableCollection<IControllerMode> ControllerModes = new ObservableCollection<IControllerMode>();
        internal int CurrentModeIndex;
    }
}
