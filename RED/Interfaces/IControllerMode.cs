using RED.ViewModels.ControlCenter;

namespace RED.Interfaces
{
    public interface IControllerMode
    {
        string Name { get; set; }

        void EnterMode();
        void EvaluateMode(IInputDevice InputVM);
        void ExitMode();
    }
}
