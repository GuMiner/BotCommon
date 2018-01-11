using BotCommon.Activity;

namespace BotCommon.Processors
{
    /// <summary>
    /// Defines a response returned from a conversation.
    /// </summary>
    /// <typeparam name="T">The context used to understand where a conversation is at.</typeparam>
    public class ConversationResponse<T>
    {
        public ConversationResponse(T conversationContext, ActivityResponse specificResponse)
        {
            this.ConversationContext = conversationContext;
            this.SpecificResponse = specificResponse;
        }

        public T ConversationContext { get; }

        public ActivityResponse SpecificResponse { get; }
    }
}
