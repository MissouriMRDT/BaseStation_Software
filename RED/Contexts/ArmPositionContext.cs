using Caliburn.Micro;
using System;
using System.Xml.Serialization;

namespace RED.Contexts
{
    [XmlType(TypeName = "Selection")]
    [Serializable]
    public class ArmPositionContext : PropertyChangedBase
    {
        private string _Name;
        private float _J1;
        private float _J2;
        private float _J3;
        private float _J4;
        private float _J5;
        private float _J6;

        public string Name { get { return _Name; } set { _Name = value; NotifyOfPropertyChange(() => Name); } }
        public float J1 { get { return _J1; } set { _J1 = value; NotifyOfPropertyChange(() => J1); } }
        public float J2 { get { return _J2; } set { _J2 = value; NotifyOfPropertyChange(() => J2); } }
        public float J3 { get { return _J3; } set { _J3 = value; NotifyOfPropertyChange(() => J3); } }
        public float J4 { get { return _J4; } set { _J4 = value; NotifyOfPropertyChange(() => J4); } }
        public float J5 { get { return _J5; } set { _J5 = value; NotifyOfPropertyChange(() => J5); } }
        public float J6 { get { return _J6; } set { _J6 = value; NotifyOfPropertyChange(() => J6); } }

        public ArmPositionContext()
        { }
    }
}
