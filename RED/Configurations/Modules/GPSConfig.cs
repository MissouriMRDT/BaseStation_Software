using RED.Contexts.Modules;

namespace RED.Configurations.Modules
{
    internal static class GPSConfig
    {
        internal static GPSSettingsContext DefaultConfig = new GPSSettingsContext()
        {
            BaseStationLocationLatitude = 0,
            BaseStationLocationLongitude = 0,
            StartLocationLatitude = 38.406426,
            StartLocationLongitude = -110.791919,
            MapShowEmptyTiles = false
        };
    }
}
