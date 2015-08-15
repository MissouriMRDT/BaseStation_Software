using Caliburn.Micro;
using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class SensorCombinedViewModel : PropertyChangedBase, ISubscribe
    {
        private ControlCenterViewModel _cc;

        public SensorCombinedViewModel(ControlCenterViewModel cc)
        {
            _cc = cc;

            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("SensorCombined"));
        }

        public void ReceiveFromRouter(byte dataId, byte[] data)
        {
            switch (_cc.MetadataManager.GetTelemetry(dataId).Name)
            {
                //This is separating the separate sensors' data and forwarding it over the correct dataid
                case "SensorCombined":
                    var ms = new MemoryStream(data);
                    using (var br = new BinaryReader(ms))
                    {
                        var gpsBuffer = br.ReadBytes(23);
                        _cc.DataRouter.Send(_cc.MetadataManager.GetId("GPSData"), gpsBuffer);

                        var headingBuffer = br.ReadSingle();
                        _cc.DataRouter.Send(_cc.MetadataManager.GetId("Heading"), headingBuffer);

                        var ultrasonicBuffer0 = new byte[] { 0, br.ReadByte() };
                        _cc.DataRouter.Send(_cc.MetadataManager.GetId("Ultrasonic"), ultrasonicBuffer0);
                        var ultrasonicBuffer1 = new byte[] { 1, br.ReadByte() };
                        _cc.DataRouter.Send(_cc.MetadataManager.GetId("Ultrasonic"), ultrasonicBuffer1);
                        var ultrasonicBuffer2 = new byte[] { 2, br.ReadByte() };
                        _cc.DataRouter.Send(_cc.MetadataManager.GetId("Ultrasonic"), ultrasonicBuffer2);

                        var voltageBuffer = br.ReadInt16();
                        _cc.DataRouter.Send(_cc.MetadataManager.GetId("Voltage"), voltageBuffer);
                    }
                    break;
            }
        }
    }
}