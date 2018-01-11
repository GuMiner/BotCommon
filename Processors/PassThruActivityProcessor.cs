using BotCommon.Activity;
using BotCommon.Storage;
using System.Threading.Tasks;

namespace BotCommon.Processors
{
    /// <summary>
    /// Defines an activity processor that mirrors the requests out to responses.
    /// </summary>
    public class PassThruActivityProcessor : IActivityProcessor
    {
        /// <summary>
        /// Returns <see cref="ActivityResponse"/> objects with the input from <paramref name="request.Text"/>
        /// </summary>
        public Task<ActivityResponse> ProcessActivityAsync(IStore store, ActivityRequest request)
        {
            return Task.FromResult(new ActivityResponse(request.Text));
        }
    }
}
