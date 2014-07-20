namespace RED.RoverComs
{
    public interface IProtocol<T>
    {
        int Id { get; set; }
        T Value { get; set; }
    }
}
