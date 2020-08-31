using System.Collections.Generic;

namespace Core.Interfaces
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
