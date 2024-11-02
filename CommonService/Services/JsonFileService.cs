using CommonService.Extentions;
using System.Text.Json.Nodes;

namespace CommonService.Service
{
    public class JsonFileService(string filePath = "configurations.json")
    {
        private readonly string _filePath = filePath;

        public Dictionary<string, string> LoadFile()
        {
            if (!File.Exists(_filePath))
            {
                throw new ArgumentNullException(nameof(LoadFile));
            }

            var json = File.ReadAllText(_filePath);
            var jsonNode = JsonNode.Parse(json)?.AsObject();
            return jsonNode?.FlattenJson() ?? [];
        }

        public void SaveConfigurations(Dictionary<string, string> config)
        {
            var raw = config.RevertToNestedJson();
            var json = raw.ToString();
            SaveConfigurations(json);
        }

        public void SaveConfigurations(string json)
        {
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath);
            }
            File.WriteAllText(_filePath, json);
        }
    }
}
