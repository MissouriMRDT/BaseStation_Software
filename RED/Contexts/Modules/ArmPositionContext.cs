using System.Xml.Serialization;

namespace RED.Contexts.Modules
{
    [XmlType(TypeName = "ArmPositions")]
    public class ArmPositionsContext : ConfigurationFile
    {
        public ArmPositionContext[] Positions;

        public ArmPositionsContext()
        { }

        public ArmPositionsContext(ArmPositionContext[] positions)
            : this()
        {
            Positions = positions;
        }
    }

    [XmlType(TypeName = "Selection")]
    public class ArmPositionContext : ConfigurationFile
    {
        public string Name;
        public float J1;
        public float J2;
        public float J3;
        public float J4;
        public float J5;
        public float J6;

        private ArmPositionContext()
        { }

        public ArmPositionContext(string name, float j1, float j2, float j3, float j4, float j5, float j6)
            : this()
        {
            Name = name;
            J1 = j1;
            J2 = j2;
            J3 = j3;
            J4 = j4;
            J5 = j5;
            J6 = j6;
        }
    }
}
