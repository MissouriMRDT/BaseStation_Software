﻿using RoverAttachmentManager.ViewModels.Arm;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace RoverAttachmentManager.Views.Arm
{
    /// <summary>
    /// Interaction logic for ArmView.xaml
    /// </summary>
    public partial class ArmView : UserControl
    {
        public ArmView()
        {
            InitializeComponent();
            SetupVLC();
        }

        internal void SetupVLC()
        {
            var vlcLibDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "libvlc", System.IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            var options = new string[]
            {
                ":network-caching=0",
                "--rtsp-caching=0"
				 //"--file-logging", "-vvv", "--extraintf=logger", "--logfile=vlc.log"
			};

            vlcPlayer.MediaPlayer.VlcLibDirectory = vlcLibDirectory;
            vlcPlayer.MediaPlayer.VlcMediaplayerOptions = options;
            vlcPlayer.MediaPlayer.EndInit();
            PlayChannel(2);
        }

        public void PlayChannel(int channel)
        {
            vlcPlayer.MediaPlayer.Play($"rtsp://admin:Rovin2012@192.168.1.226:554/mpeg4/ch0{channel}/main/av_stream");
        }

        private async void OverrideButton_Click(object sender, RoutedEventArgs e)
        {
            byte busIndex1 = Byte.Parse((string)((ToggleButton)sender).Tag);
            byte busIndex2 = Byte.Parse((string)((ToggleButton)sender).Tag + 1);
            if ((bool)((ToggleButton)sender).IsChecked)
            {
                ((ArmViewModel)DataContext).LimitSwitchOverride(busIndex1);
                ((ArmViewModel)DataContext).LimitSwitchOverride(busIndex2);
            }
            else
            {
                ((ArmViewModel)DataContext).LimitSwitchUnOverride(busIndex1);
                ((ArmViewModel)DataContext).LimitSwitchUnOverride(busIndex2);
            }
        }
    }
}
