using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BotCommon.Storage
{
    /// <summary>
    /// Standardizes serialization settings by providing a common <see cref="Newtonsoft.Json.JsonSerializer"/> to use
    /// </summary>
    public class Serializer
    {
        public static JsonSerializerSettings Settings { get; } = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static JsonSerializer JsonSerializer { get; } = JsonSerializer.Create(Serializer.Settings);
    }
}
