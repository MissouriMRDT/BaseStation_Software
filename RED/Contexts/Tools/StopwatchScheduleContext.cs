using System.Xml.Serialization;

namespace RED.Contexts.Tools
{
    [XmlType(TypeName = "StopwatchSchedules")]
    public class StopwatchContext : ConfigurationFile
    {
        public StopwatchScheduleContext[] Schedules;

        private StopwatchContext()
        { }

        public StopwatchContext(StopwatchScheduleContext[] schedules)
            : this()
        {
            Schedules = schedules;
        }
    }

    [XmlType(TypeName = "Schedule")]
    public class StopwatchScheduleContext : ConfigurationFile
    {
        public string Name;
        public StopwatchPhaseContext[] Phases;

        private StopwatchScheduleContext()
        { }

        public StopwatchScheduleContext(string name, StopwatchPhaseContext[] phases)
            : this()
        {
            Name = name;
            Phases = phases;
        }
    }

    [XmlType(TypeName = "Phase")]
    public class StopwatchPhaseContext : ConfigurationFile
    {
        public string Name;
        public int Duration;

        private StopwatchPhaseContext()
        { }

        public StopwatchPhaseContext(string name, int duration)
            : this()
        {
            Name = name;
            Duration = duration;
        }
    }
}
