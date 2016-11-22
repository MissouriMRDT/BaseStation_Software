using Caliburn.Micro;
using RED.Interfaces;
using System.IO;

namespace RED.ViewModels.Modules
{
    public class SensorCombinedViewModel : PropertyChangedBase, ISubscribe
    {
        private IDataRouter _router;
        private IDataIdResolver _idResolver;

        public SensorCombinedViewModel(IDataRouter router, IDataIdResolver idResolver)
        {
            _router = router;
            _idResolver = idResolver;

            _router.Subscribe(this, _idResolver.GetId("SensorCombined"));
        }

        public void ReceiveFromRouter(ushort dataId, byte[] data)
        {
            switch (_idResolver.GetName(dataId))
            {
                //This is separating the separate sensors' data and forwarding it over the correct dataid
                case "SensorCombined":
                    var ms = new MemoryStream(data);
                    using (var br = new BinaryReader(ms))
                    {
                        var gpsBuffer = br.ReadBytes(23);
                        _router.Send(_idResolver.GetId("GPSData"), gpsBuffer);

                        var headingBuffer = br.ReadSingle();
                        _router.Send(_idResolver.GetId("Heading"), headingBuffer);

                        var ultrasonicBuffer0 = new byte[] { 0, br.ReadByte() };
                        _router.Send(_idResolver.GetId("Ultrasonic"), ultrasonicBuffer0);
                        var ultrasonicBuffer1 = new byte[] { 1, br.ReadByte() };
                        _router.Send(_idResolver.GetId("Ultrasonic"), ultrasonicBuffer1);
                        var ultrasonicBuffer2 = new byte[] { 2, br.ReadByte() };
                        _router.Send(_idResolver.GetId("Ultrasonic"), ultrasonicBuffer2);

                        var voltageBuffer = br.ReadInt16();
                        _router.Send(_idResolver.GetId("Voltage"), voltageBuffer);
                    }
                    break;
            }
        }
    }
}