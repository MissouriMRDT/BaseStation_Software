namespace RoverNetworkManager.Interfaces
{
    public interface IDataIdResolver
    {
        ushort GetId(string name);
        string GetName(ushort dataId);
    }
}
