using RED.Interfaces.Input;
using System.Collections.ObjectModel;

namespace RED.Models.Input.Controllers
{
    internal class XboxControllerInputModel
    {
        internal int SerialReadSpeed;
        internal int ManualDeadzone;
        internal bool AutoDeadzone = false;

        internal bool Connected;
    }
}