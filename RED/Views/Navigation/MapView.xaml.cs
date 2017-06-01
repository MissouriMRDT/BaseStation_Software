using RED.ViewModels.Navigation;
using System.Windows.Controls;

namespace RED.Views.Navigation
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        public MapView()
        {
            InitializeComponent();
        }

        private void Map_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext.GetType() == typeof(MapViewModel))
                ((MapViewModel)(DataContext)).SetMap(Gmap);
        }
    }
}
