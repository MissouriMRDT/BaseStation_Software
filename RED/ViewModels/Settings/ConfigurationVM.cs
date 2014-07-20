namespace RED.ViewModels.Settings
{
    using Models.ControlCenter;
    using Properties;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ConfigurationVM : BaseVM
    {
        public string DefaultIp
        {
            get
            {
                return Settings.Default.DefaultIP;
            }
            set
            {
                if (value.GetTypeCode() != TypeCode.String) return;
                Settings.Default.DefaultIP = value;
                Settings.Default.Save();
                RaisePropertyChanged("DefaultIp");
            }
        }
        public int DefaultPort
        {
            get
            {
                return Settings.Default.DefaultPort;
            }
            set
            {
                if (value.GetTypeCode() != TypeCode.Int32) return;
                Settings.Default.DefaultPort = value;
                Settings.Default.Save();
                RaisePropertyChanged("DefaultPort");
            }
        }
        public List<string> AvailableControlModes
        {
            get
            {
                return Enum.GetNames(typeof(ControlMode)).ToList();
            }
        }
        public string SelectedDefaultControlMode
        {
            get
            {
                return Settings.Default.DefaultControlMode;
            }
            set
            {
                Settings.Default.DefaultControlMode = value;
                Settings.Default.Save();
                RaisePropertyChanged("DefaultControlMode");
            }
        }
        public bool VoiceMute
        {
            get
            {
                return Settings.Default.VoiceMute;
            }
            set
            {
                Settings.Default.VoiceMute = value;
                Settings.Default.Save();
                RaisePropertyChanged("VoiceMute");
            }
        }
        public bool VoiceCommandsMute
        {
            get
            {
                return Settings.Default.VoiceCommandsMute;
            }
            set
            {
                Settings.Default.VoiceCommandsMute = value;
                Settings.Default.Save();
                RaisePropertyChanged("VoiceCommandsMute");
            }
        }
        public int SerialReadSpeed
        {
            get
            {
                return Settings.Default.SerialReadSpeed;
            }
            set
            {
                if (value.GetTypeCode() != TypeCode.Int32) return;
                Settings.Default.SerialReadSpeed = value;
                Settings.Default.Save();
                RaisePropertyChanged("SerialReadSpeed");
            }
        }
        public int DriveCommandSpeedMs
        {
            get
            {
                return Settings.Default.DriveCommandSpeed;
            }
            set
            {
                if (value.GetTypeCode() != TypeCode.Int32) return;
                Settings.Default.DriveCommandSpeed = value;
                Settings.Default.Save();
                RaisePropertyChanged("DriveCommandSpeed");
            }
        }
    }
}
