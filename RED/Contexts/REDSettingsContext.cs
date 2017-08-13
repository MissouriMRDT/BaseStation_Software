using System.Xml.Serialization;

namespace RED.Contexts
{
    [XmlType(TypeName = "REDSettings")]
    public class REDSettingsContext : ConfigurationFile
    {
        public DriveSettingsContext Drive;
        public XboxControllerSettingsContext Xbox1;
        public GPSSettingsContext GPS;
        public ScienceSettingsContext Science;
        public PowerSettingsContext Power;
    }
}
