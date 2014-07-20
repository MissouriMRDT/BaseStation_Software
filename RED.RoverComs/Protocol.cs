namespace RED.RoverComs
{
    public class Protocol<T> : IProtocol<T>
    {
        public int Id { get; set; }
        public T Value { get; set; }

        public Protocol()
        {
            
        }
        public Protocol(T value)
        {
            Id = 0;
            Value = value;
        }
        public Protocol(int id, T value)
        {
            Value = value;
            Id = id;
        }
    }
}
