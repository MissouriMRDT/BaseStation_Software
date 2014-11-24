namespace RED.Interfaces
{
    interface IMetadata
    {
        byte Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Datatype { get; set; }
    }
}