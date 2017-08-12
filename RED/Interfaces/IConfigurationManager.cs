using RED.Contexts;

namespace RED.Interfaces
{
    public interface IConfigurationManager
    {
        void AddRecord<T>(string name, T defaultConfig) where T : ConfigurationFile;
        T GetConfig<T>(string name) where T : ConfigurationFile;
        void SetConfig<T>(string name, T config) where T : ConfigurationFile;
    }
}
