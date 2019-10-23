using System.Windows.Controls;
﻿using HelixToolkit.Wpf;

namespace RED.Views.Modules
{
    /// <summary>
    /// Interaction logic for GPSView.xaml
    /// </summary>
    public partial class GPSView : UserControl
    {
        public GPSView()
        {
            InitializeComponent();
        }
        private void Create3DViewPort()
        {
            HelixViewport3D hVp3D = new HelixViewport3D();
            var lights = new DefaultLights();
            var teaPot = new Teapot();
            hVp3D.Children.Add(lights);
            hVp3D.Children.Add(teaPot);
            AddChild(hVp3D);
        }
    }
}