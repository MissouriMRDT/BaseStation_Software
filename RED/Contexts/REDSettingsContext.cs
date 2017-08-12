using System.Xml.Serialization;

namespace RED.Contexts
{
    [XmlType(TypeName = "REDSettings")]
    public class REDSettingsContext : ConfigurationFile
    {
        public DriveSettingsContext Drive;
        public ScienceSettingsContext Science;
    }
}
