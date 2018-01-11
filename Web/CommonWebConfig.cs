using BotCommon.Storage;
using Newtonsoft.Json;
using System.Web.Http;

namespace BotCommon.Web
{
    /// <summary>
    /// Defines common web configuration for bots
    /// </summary>
    public class CommonWebConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = Serializer.Settings.NullValueHandling;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = Serializer.Settings.ContractResolver;
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Serializer.Settings.Formatting;
            JsonConvert.DefaultSettings = () => Serializer.Settings;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
