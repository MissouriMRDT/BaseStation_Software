using System.Collections.Generic;

namespace RED.Interfaces.Input
{
    public interface IInputMode
    {
        string Name { get; }
        string ModeType { get; }

        void StartMode();
        void StopMode();
        void SetValues(Dictionary<string, float> values);
    }
}
