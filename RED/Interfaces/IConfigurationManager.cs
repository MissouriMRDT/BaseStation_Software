namespace RED.Interfaces
{
    public interface IConfigurationManager
    {
        void AddRecord(string name, IConfigurationFile defaultConfig);
        T GetConfig<T>(string name) where T : IConfigurationFile;
        void SetConfig<T>(string name, T config) where T : IConfigurationFile;
    }
}
