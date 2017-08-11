namespace RED.Interfaces
{
    public interface IConfigurationManager
    {
        T GetConfig<T>(string name) where T : IConfigurationFile;
        void SetConfig<T>(T config) where T : IConfigurationFile;
    }
}
