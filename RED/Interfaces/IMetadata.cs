namespace RED.Interfaces
{
    public interface IMetadata
    {
        ushort Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Datatype { get; set; }
        string ServerAddress { get; set; }
    }
}