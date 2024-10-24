using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace CommonService.Extentions
{
    public static class JsonExtentions
    {
        public static Dictionary<string, string> FlattenJson(this JsonObject jsonObject, string parentKey = "")
        {
            var result = new Dictionary<string, string>();
            FlattenJsonInternal(jsonObject, parentKey, result);
            return result;
        }

        private static void FlattenJsonInternal(JsonObject jsonObject, string parentKey, Dictionary<string, string> result)
        {
            foreach (var kvp in jsonObject)
            {
                var newKey = string.IsNullOrEmpty(parentKey) ? kvp.Key : $"{parentKey}:{kvp.Key}";

                if (kvp.Value is JsonObject nestedObject)
                {
                    if (nestedObject.Count > 0)
                    {
                        FlattenJsonInternal(nestedObject, newKey, result);
                    }
                    else
                    {
                        result[newKey] = "";
                    }
                }
                else if (kvp.Value is JsonArray jsonArray)
                {
                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        FlattenJsonInternal(jsonArray[i].AsObject(), $"{newKey}[{i}]", result);
                    }
                }
                else
                {
                    result[newKey] = kvp.Value?.ToString() ?? "";
                }
            }
        }

        public static JObject RevertToNestedJson(this Dictionary<string, string> flatJson)
        {
            var result = new JObject();

            foreach (var kvp in flatJson)
            {
                var keys = kvp.Key.Split(':');
                JObject current = result;

                for (int i = 0; i < keys.Length; i++)
                {
                    if (i == keys.Length - 1)
                    {
                        current[keys[i]] = JToken.FromObject(kvp.Value);
                    }
                    else
                    {
                        if (current[keys[i]] == null)
                        {
                            current[keys[i]] = new JObject();
                        }
                        current = (JObject)current[keys[i]];
                    }
                }
            }

            return result;
        }

        public static Dictionary<string, string> ReduceLevels(this Dictionary<string, string> flatJson, int level = 1)
        {
            var reduced = flatJson.Select(kvp =>
            {
                var keyPath = kvp.Key.Split(':');
                var newKey = kvp.Key;
                if (keyPath.Length > 1)
                {
                    newKey = string.Join(':', keyPath.Skip(level));
                }

                return new KeyValuePair<string, string>(newKey, kvp.Value);
            });

            return reduced.ToDictionary();
        }
    }
}
