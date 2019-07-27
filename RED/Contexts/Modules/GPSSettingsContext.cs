using Core.Contexts;
using RED.Addons.Navigation;

namespace RED.Contexts.Modules
{
    public class GPSSettingsContext : ConfigurationFile
    {
        public double BaseStationLocationLatitude;
        public double BaseStationLocationLongitude;
        public double StartLocationLatitude;
        public double StartLocationLongitude;
        public bool MapShowEmptyTiles;
        public GPSCoordinate Offset;
    }
}
