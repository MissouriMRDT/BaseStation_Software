using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Contexts
{
    public class TelemetryContext<T>
    {
        public DateTime ReceivedOn { get; set; }
        public T Value { get; set; }

        public TelemetryContext(DateTime receivedOn, T value)
        {
            ReceivedOn = receivedOn;
            Value = value;
        }
        public TelemetryContext(T value)
        {
            ReceivedOn = DateTime.Now;
            Value = value;
        }
    }
}
