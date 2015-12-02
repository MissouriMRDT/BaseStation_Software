using RED.ViewModels.ControlCenter;

namespace RED.Interfaces
{
    public interface IControllerMode
    {
        string Name { get; set; }
        IInputDevice InputVM { get; set; }

        void EnterMode();
        void EvaluateMode();
        void ExitMode();
    }
}
