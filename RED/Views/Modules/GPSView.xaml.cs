using RED.Addons;
using RED.ViewModels.Modules;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RED.Views.Modules
{
    /// <summary>
    /// Interaction logic for GPSView.xaml
    /// </summary>
    public partial class GPSView : UserControl
    {
        // Marker
        public GPSView()
        {
            InitializeComponent();
        }

        private void AddWaypointBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double yCoord = Utilities.ParseCoordinate(LatitudeTextBox.Text);
                double xCoord = Utilities.ParseCoordinate(LongitudeTextBox.Text);
                //((GPSViewModel)DataContext).Waypoints.Add(new Addons.GPSCoordinate(yCoord, xCoord));
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Invalid Longitude or Latitude. Must be a floating point number.");
            }
        }
    }
}