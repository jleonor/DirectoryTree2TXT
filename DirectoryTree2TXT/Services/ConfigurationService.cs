using DirectoryTreeViewer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectoryTreeViewer.Services
{
    public class ConfigurationService
    {
        private readonly string _configFolder;

        public ConfigurationService(string? configFolder = null)
        {
            _configFolder = configFolder ?? Path.Combine(Environment.CurrentDirectory, "search_configs");
            Directory.CreateDirectory(_configFolder);
        }


        /// <summary>
        /// Saves the configuration to a JSON file.
        /// </summary>
        public void SaveConfiguration(SearchConfiguration config)
        {
            var json = JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented);
            var fileName = $"config_{DateTime.Now:yyyyMMddHHmmss}.json";
            var filePath = Path.Combine(_configFolder, fileName);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Returns all configuration files.
        /// </summary>
        public List<FileInfo> LoadConfigurations()
        {
            Directory.CreateDirectory(_configFolder);
            return new DirectoryInfo(_configFolder).GetFiles("*.json").ToList();
        }

        /// <summary>
        /// Loads a configuration from a given file.
        /// </summary>
        public SearchConfiguration? LoadConfiguration(FileInfo file)
        {
            if (file == null || !file.Exists) return null;
            var json = File.ReadAllText(file.FullName);
            return JsonConvert.DeserializeObject<SearchConfiguration>(json);
        }


        /// <summary>
        /// Deletes the specified configuration file.
        /// </summary>
        public void DeleteConfiguration(FileInfo file)
        {
            if (file != null && file.Exists)
            {
                File.Delete(file.FullName);
            }
        }

        /// <summary>
        /// Renames the specified configuration file.
        /// </summary>
        public void RenameConfiguration(FileInfo file, string newName)
        {
            if (file != null && file.Exists && !string.IsNullOrEmpty(newName))
            {
                var newPath = Path.Combine(_configFolder, newName);
                File.Move(file.FullName, newPath);
            }
        }
    }
}
