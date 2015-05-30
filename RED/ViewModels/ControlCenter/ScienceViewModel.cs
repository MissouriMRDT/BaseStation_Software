using Caliburn.Micro;
using RED.Interfaces;
using RED.Models;
using RED.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class ScienceViewModel : PropertyChangedBase, ISubscribe
    {
        private ScienceModel _model;
        private ControlCenterViewModel _cc;

        private short[] CCDPixelBuffer;
        private short CCDPixelsRecieved;

        public float Ph
        {
            get
            {
                return _model.Ph;
            }
            set
            {
                _model.Ph = value;
                NotifyOfPropertyChange(() => Ph);
            }
        }
        public short Moisture
        {
            get
            {
                return _model.Moisture;
            }
            set
            {
                _model.Moisture = value;
                NotifyOfPropertyChange(() => Moisture);
            }
        }

        public short CCDPixelCount
        {
            get
            {
                return _model.CCDPixelCount;
            }
            set
            {
                _model.CCDPixelCount = value;
                NotifyOfPropertyChange(() => CCDPixelCount);
            }
        }
        public float CCDProgress
        {
            get
            {
                return _model.CCDProgress;
            }
            set
            {
                _model.CCDProgress = value;
                NotifyOfPropertyChange(() => CCDProgress);
            }
        }
        public string CCDFilePath
        {
            get
            {
                return _model.CCDFilePath;
            }
            set
            {
                _model.CCDFilePath = value;
                NotifyOfPropertyChange(() => CCDFilePath);
            }
        }
        public bool CCDIsReceiving
        {
            get
            {
                return _model.CCDIsReceiving;
            }
            set
            {
                _model.CCDIsReceiving = value;
                NotifyOfPropertyChange(() => CCDIsReceiving);
            }
        }

        public ScienceViewModel(ControlCenterViewModel cc)
        {
            _model = new ScienceModel();
            _cc = cc;

            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Ph"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Moisture"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("CCD"));
        }

        public void RequestPhData()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceRequest"), (byte)ScienceRequestTypes.Ph);
            _cc.Console.WriteToConsole("Science pH data requested.");
        }
        public void RequestMoistureData()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceRequest"), (byte)ScienceRequestTypes.Moisture);
            _cc.Console.WriteToConsole("Science moisture data requested.");
        }
        public void RequestCCDData()
        {
            StartCCDReceive();
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceRequest"), (byte)ScienceRequestTypes.CCD);
            _cc.Console.WriteToConsole("Science CCD data requested.");
        }
        public void RequestLaserOn()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceRequest"), (byte)ScienceRequestTypes.LaserOn);
            _cc.Console.WriteToConsole("Science Laser On requested.");
        }
        public void RequestLaserOff()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceRequest"), (byte)ScienceRequestTypes.LaserOff);
            _cc.Console.WriteToConsole("Science Laser Off requested.");
        }

        public void ForceCCDSave()
        {
            if (CCDIsReceiving)
                FinishCCDReceive();
        }

        public void ReceiveFromRouter(byte dataId, byte[] data)
        {
            switch (_cc.MetadataManager.GetTelemetry(dataId).Name)
            {
                case "Ph":
                    Ph = BitConverter.ToSingle(data, 0);
                    break;
                case "Moisture":
                    Moisture = BitConverter.ToInt16(data, 0);
                    break;
                case "CCD":
                    if (!CCDIsReceiving)
                    {
                        _cc.Console.WriteToConsole("Unexpected CCD data received. Packet number " + data[0].ToString());
                        break;
                    }

                    byte packetNum = data[0];
                    int index = packetNum * 12;
                    for (int i = 1; i < data.Length; i += 2)
                    {
                        short pixel = BitConverter.ToInt16(data, i);
                        CCDPixelBuffer[index++] = pixel;
                        CCDPixelsRecieved++;
                    }

                    CCDProgress = (float)CCDPixelsRecieved / CCDPixelCount;

                    if (CCDPixelsRecieved == CCDPixelCount)
                        FinishCCDReceive();

                    break;
            }
        }

        private void StartCCDReceive()
        {
            CCDIsReceiving = true;
            CCDProgress = 0;
            CCDPixelBuffer = new short[CCDPixelCount];
            for (int i = 0; i < CCDPixelBuffer.Length; i++)
                CCDPixelBuffer[i] = -1;
        }

        private void FinishCCDReceive()
        {
            CCDIsReceiving = false;
            CCDProgress = 1.0f;

            //Save CSV File
            if (Path.GetFileName(CCDFilePath) == "") //new file
            {
                StringBuilder csv = new StringBuilder();
                for (var i = 0; i < CCDPixelBuffer.Length; i++)
                {
                    if (i > 0) csv.Append(", ");
                    csv.Append(i);
                }
                for (var i = 0; i < CCDPixelBuffer.Length; i++)
                {
                    if (i > 0) csv.Append(", ");
                    if (CCDPixelBuffer[i] >= 0) csv.Append(i);
                }

                string filename = "REDScienceData" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                string path = Path.Combine(CCDFilePath, filename);
                File.WriteAllText(path, csv.ToString());
                CCDFilePath = path;
                _cc.Console.WriteToConsole("CCD data saved to file " + path + ".");
            }
            else
            {
                StringBuilder csv = new StringBuilder();
                for (var i = 0; i < CCDPixelBuffer.Length; i++)
                {
                    if (i > 0) csv.Append(", ");
                    if (CCDPixelBuffer[i] >= 0) csv.Append(i);
                }

                string path = CCDFilePath;
                File.AppendAllText(path, csv.ToString());
                _cc.Console.WriteToConsole("CCD data appended to file " + path + ".");
            }
        }

        public enum ScienceRequestTypes
        {
            Ph = 1,
            Moisture = 2,
            CCD = 3,
            LaserOn = 4,
            LaserOff = 5
        }
    }
}