using BotCommon.Activity;
using BotCommon.Storage;
using System.Threading.Tasks;

namespace BotCommon.Processors
{
    /// <summary>
    /// Defines an <see cref="IActivityProcessor"/> that saves and restores conversation context.
    /// </summary>
    /// <typeparam name="T">The data context that is </typeparam>
    public abstract class ConversationActivityProcessor<T> : IActivityProcessor
    {
        /// <summary>
        /// Defines the subcontainer used to store conversations of this nature in the bot.
        /// </summary>
        protected abstract string SubContainer { get; }

        /// <summary>
        /// Called when a conversation is first initiated. 
        /// <see cref="ContinueConversationAsync(IStore, ActivityRequest, T)"/> is not called until subsequent replies by the bot user.
        /// </summary>
        /// <param name="store">The persistent store to save conversations in</param>
        /// <param name="request">The first request from the user to start the conversation.</param>
        /// <returns>A <see cref="ConversationResponse{T}"/> defining what conversation context to save and what specific response to return.</returns>
        protected abstract Task<ConversationResponse<T>> StartConversationAsync(IStore store, ActivityRequest request);

        /// <summary>
        /// Called when a conversation continues.
        /// </summary>
        /// <param name="store">The persistent store to save conversations in</param>
        /// <param name="request">The first request from the user to start the conversation.</param>
        /// <param name="priorConversation">The prior conversation saved.</param>
        /// <returns>A <see cref="ConversationResponse{T}"/> defining what conversation context to save and what specific response to return.</returns>
        protected abstract Task<ConversationResponse<T>> ContinueConversationAsync(IStore store, ActivityRequest request, T priorConversation);

        /// <inheritdoc />
        public async Task<ActivityResponse> ProcessActivityAsync(IStore store, ActivityRequest request)
        {
            ConversationResponse<T> response;
            if (!(await store.ExistsAsync(this.SubContainer, request.ConversationId)))
            {
                response = await this.StartConversationAsync(store, request).ConfigureAwait(false);
            }
            else
            {
                T priorConversation = await store.GetAsync<T>(this.SubContainer, request.ConversationId).ConfigureAwait(false);
                response = await this.ContinueConversationAsync(store, request, priorConversation).ConfigureAwait(false);
            }

            await store.CreateOrUpdateAsync(this.SubContainer, request.ConversationId, response.ConversationContext).ConfigureAwait(false);
            return response.SpecificResponse;
        }
    }
}
