using System.Collections.Generic;

namespace RoverNetworkManager.Interfaces.Input
{
    public interface IInputDevice
    {
        string Name { get; }
        string DeviceType { get; }

        void StartDevice();
        void StopDevice();
        Dictionary<string, float> GetValues();
        bool IsReady();
    }
}
