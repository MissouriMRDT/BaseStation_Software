using RED.Contexts.Input.Controllers;
using RED.Contexts.Modules;
using RED.Contexts.Network;
using System.Xml.Serialization;

namespace RED.Contexts
{
    [XmlType(TypeName = "REDSettings")]
    public class REDSettingsContext : ConfigurationFile
    {
        public DriveSettingsContext Drive;
        public XboxControllerSettingsContext Xbox1;
        public XboxControllerSettingsContext Xbox2;
        public XboxControllerSettingsContext Xbox3;
        public XboxControllerSettingsContext Xbox4;
        public GPSSettingsContext GPS;
        public ScienceSettingsContext Science;
        public PowerSettingsContext Power;
        public NetworkManagerSettingsContext Network;
    }
}
