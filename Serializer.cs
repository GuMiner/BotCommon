using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace BotCommon
{
    /// <summary>
    /// Standardizes serialization settings by providing a common <see cref="Newtonsoft.Json.JsonSerializer"/> to use
    /// </summary>
    public class Serializer
    {
        private static Lazy<JsonSerializer> serializer = new Lazy<JsonSerializer>(() =>
        {
            return JsonSerializer.Create(new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        });

        public static JsonSerializer JsonSerializer { get; } = Serializer.serializer.Value;
    }
}
