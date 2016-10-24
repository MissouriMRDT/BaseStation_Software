namespace RED.Addons
{
    public struct GPSCoordinate
    {
        public double Latitude;
        public double Longitude;

        public GPSCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
