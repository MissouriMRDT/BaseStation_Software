using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace RED.ViewModels
{
    public class XMLConfigManager : IConfigurationManager
    {
        private const string StoragePath = "REDConfig/";

        ILogger _log;

        private HashSet<IConfigurationFile> DefaultConfigs;

        public XMLConfigManager(ILogger log)
        {
            _log = log;
            DefaultConfigs = new HashSet<IConfigurationFile>();

            if (!Directory.Exists(StoragePath))
                Directory.CreateDirectory(StoragePath);
        }

        public void AddRecord(IConfigurationFile defaultConfig)
        {
            DefaultConfigs.Add(defaultConfig);
            if (!File.Exists(defaultConfig.ConfigName))
                SetConfig(defaultConfig);
        }
        public void AddRecords(IConfigurationFile[] defaultConfigs)
        {
            foreach (var config in defaultConfigs)
                AddRecord(config);
        }

        public T GetConfig<T>(string name) where T : IConfigurationFile
        {
            if (!DefaultConfigs.Any(x => x.ConfigName == name))
            {
                _log.Log("Error! No config loaded for \"{0}\".", name);
                throw new ArgumentException("Config name not loaded");
            }

            try
            {
                using (var file = ReadFile(name))
                    return DeserializeFile<T>(file);
            }
            catch (FileNotFoundException)
            {
                _log.Log("Missing config file for \"{0}\". Using default config instead.", name);
            }
            catch (DirectoryNotFoundException)
            {
                _log.Log("Missing config directory for \"{0}\". Using default config instead.", name);
            }
            catch (IOException)
            {
                _log.Log("Unknown IO error when reading config file for \"{0}\". Using default config instead.", name);
            }
            catch (InvalidOperationException)
            {
                _log.Log("Deserialization error when reading config file for \"{0}\". Using default config instead.", name);
            }
            catch (SerializationException)
            {
                _log.Log("Deserialization error when reading config file for \"{0}\". Using default config instead.", name);
            }

            return (T)DefaultConfigs.First(x => x.ConfigName == name);
        }
        public void SetConfig<T>(T config) where T : IConfigurationFile
        {
            try
            {
                using (var file = WriteFile(config.ConfigName))
                    SerializeFile(file, config);
            }
            catch (FileNotFoundException)
            {
                _log.Log("Missing file when writing config for \"{0}\".", config.ConfigName);
            }
            catch (DirectoryNotFoundException)
            {
                _log.Log("Missing config directory when writing config for \"{0}\".", config.ConfigName);
            }
            catch (IOException)
            {
                _log.Log("Unknown IO error when writing config file for \"{0}\".", config.ConfigName);
            }
            catch (InvalidOperationException)
            {
                _log.Log("Serialization error when writing config file for \"{0}\".", config.ConfigName);
            }
            catch (SerializationException)
            {
                _log.Log("Serialization error when writing config file for \"{0}\".", config.ConfigName);
            }
        }

        private string GetPathFromConfigName(string name)
        {
            return Path.Combine(StoragePath, name, ".xml");
        }

        private FileStream ReadFile(string name)
        {
            string file = GetPathFromConfigName(name);
            return File.OpenRead(file);
        }
        private FileStream WriteFile(string name)
        {
            string file = GetPathFromConfigName(name);
            return File.OpenWrite(file);
        }

        private T DeserializeFile<T>(Stream stream) where T : IConfigurationFile
        {
            var obj = new XmlSerializer(typeof(T));
            return (T)obj.Deserialize(stream);
        }
        private void SerializeFile<T>(Stream stream, T config) where T : IConfigurationFile
        {
            var obj = new XmlSerializer(typeof(T));
            obj.Serialize(stream, config);
        }
    }
}
