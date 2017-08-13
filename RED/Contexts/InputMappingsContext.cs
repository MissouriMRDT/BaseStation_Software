using RED.ViewModels.Input;
using System.Xml.Serialization;

namespace RED.Contexts
{
    [XmlType(TypeName = "InputMappings")]
    public class InputMappingsContext : ConfigurationFile
    {
        public MappingViewModel[] Mappings;

        public InputMappingsContext()
        { }

        public InputMappingsContext(MappingViewModel[] mappings)
            : this()
        {
            Mappings = mappings;
        }
    }
}
