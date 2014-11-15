namespace RED.Contexts
{
    using System;

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
