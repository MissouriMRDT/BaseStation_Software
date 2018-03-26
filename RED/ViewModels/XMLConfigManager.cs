using RED.Contexts;
using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace RED.ViewModels
{
    public class XMLConfigManager : IConfigurationManager
    {
        private const string StoragePath = "REDConfig/";

        private readonly ILogger _log;

        //we save the default configs as they come in into a dictionary. We also save them 
        //as xml files after they get loaded in. The reason we maintain both is so that the dictionary 
        //can act as a backup in case the xml files get deleted or lost in the middle of the program runtime
        //for any reason.
        private Dictionary<string, ConfigurationFile> DefaultConfigs;

        public XMLConfigManager(ILogger log)
        {
            _log = log;
            DefaultConfigs = new Dictionary<string, ConfigurationFile>();

            if (!Directory.Exists(StoragePath))
                Directory.CreateDirectory(StoragePath);
        }

        public void AddRecord<T>(string name, T defaultConfig) where T : ConfigurationFile
        {
            if (DefaultConfigs.ContainsKey(name))
                throw new ArgumentException("A config with this name has already been loaded");

            DefaultConfigs.Add(name, defaultConfig);
            if (!File.Exists(GetPathFromConfigName(name)))
                SetConfig(name, defaultConfig);
        }

        public T GetConfig<T>(string name) where T : ConfigurationFile
        {
            if (name == String.Empty)
                throw new ArgumentException("Config name cannot be empty");
            if (!DefaultConfigs.ContainsKey(name))
            {
                _log.Log($"Error! No config loaded for \"{name}\".");
                throw new ArgumentException("Config name not loaded");
            }

            try
            {
                using (var file = ReadFile(name))
                    return DeserializeFile<T>(file);
            }
            catch (FileNotFoundException)
            {
                _log.Log($"Missing config file for \"{name}\". Using default config instead.");
            }
            catch (DirectoryNotFoundException)
            {
                _log.Log($"Missing config directory when reading config file for \"{name}\". Using default config instead.");
            }
            catch (IOException)
            {
                _log.Log($"Unknown IO error when reading config file for \"{name}\". Using default config instead.");
            }
            catch (InvalidOperationException)
            {
                _log.Log($"Deserialization error when reading config file for \"{name}\". Using default config instead.");
            }
            catch (SerializationException)
            {
                _log.Log($"General deserialization error when reading config file for \"{name}\". Using default config instead.");
            }

            return (T)DefaultConfigs[name];
        }
        public void SetConfig<T>(string name, T config) where T : ConfigurationFile
        {
            if (name == String.Empty)
                throw new ArgumentException("Config name cannot be empty");
            try
            {
                using (var file = WriteFile(name))
                    SerializeFile(file, config);
            }
            catch (FileNotFoundException)
            {
                _log.Log($"Missing file when writing config for \"{name}\".");
            }
            catch (DirectoryNotFoundException)
            {
                _log.Log($"Missing config directory when writing config for \"{name}\".");
            }
            catch (IOException)
            {
                _log.Log($"Unknown IO error when writing config file for \"{name}\".");
            }
            catch (InvalidOperationException)
            {
                _log.Log($"Serialization error when writing config file for \"{name}\".");
            }
            catch (SerializationException)
            {
                _log.Log($"General serialization error when writing config file for \"{name}\".");
            }
        }

        private string GetPathFromConfigName(string name)
        {
            return Path.Combine(StoragePath, name + ".xml");
        }

        private FileStream ReadFile(string name)
        {
            string file = GetPathFromConfigName(name);
            return File.OpenRead(file);
        }
        private FileStream WriteFile(string name)
        {
            string file = GetPathFromConfigName(name);
            return File.Create(file);
        }

        private T DeserializeFile<T>(Stream stream) where T : ConfigurationFile
        {
            var obj = new XmlSerializer(typeof(T));
            return (T)obj.Deserialize(stream);
        }
        private void SerializeFile<T>(Stream stream, T config) where T : ConfigurationFile
        {
            var obj = new XmlSerializer(typeof(T));
            obj.Serialize(stream, config);
        }
    }
}
