using BotCommon.Activity;
using BotCommon.Storage;
using System.Threading.Tasks;

namespace BotCommon.Processors
{
    /// <summary>
    /// Defines how to process activities and return responses for the bot framework.
    /// </summary>
    public interface IActivityProcessor
    {
        /// <summary>
        /// Given a <paramref name="store"/> and an <paramref name="request"/>, determines how to form an appropriate <see cref="ActivityResponse"/>
        /// </summary>
        Task<ActivityResponse> ProcessActivityAsync(IStore store, ActivityRequest request);
    }
}
