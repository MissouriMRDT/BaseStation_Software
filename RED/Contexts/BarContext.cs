namespace RED.Contexts
{
    using System;

    public class BarContext
    {
        public DateTime ReceivedOn { get; private set; }
        public double Value { get; private set; }

        public BarContext(DateTime receivedOn, double value)
        {
            ReceivedOn = receivedOn;
            Value = value;
        }
    }
}
