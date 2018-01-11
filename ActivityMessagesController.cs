using BotCommon.Activity;
using BotCommon.Processors;
using BotCommon.Storage;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace BotCommon
{
    /// <summary>
    /// Defines a basic <see cref="ApiController"/> that uses persistent <see cref="IStore"/> storage and standard <see cref="IActivityProcessor"/> activity processing.
    /// </summary>
    [BotAuthentication]
    public abstract class ActivityMessagesController : ApiController
    {
        /// <summary>
        /// Gets the persistent storage API.
        /// </summary>
        /// <remarks>
        /// This should *not* be created per request, but instead be created statically.
        /// </remarks>
        public abstract IStore Store { get; }

        /// <summary>
        /// Gets the <see cref="IActivityProcessor"/> that will process activities from users.
        /// </summary>
        /// <remarks>
        /// This should *not* be created per request, but instead be created statically.
        /// </remarks>
        public abstract IActivityProcessor ActivityProcessor { get; }

        /// <summary>
        /// Processes Bot Framework messages and returns responses.
        /// </summary>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Microsoft.Bot.Connector.Activity activity)
        {
            try
            {
                if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
                {
                    // Get and process
                    IMessageActivity message = activity.AsMessageActivity();

                    ActivityRequest request = new ActivityRequest(
                        recipient: message.Recipient.Name,
                        text: message.Text,
                        from: message.From.Name,
                        fromId: message.From.Id,
                        channelId: message.ChannelId,
                        conversationId: message.Conversation.Id,
                        isGroup: message.Conversation.IsGroup,
                        attachments: message.Attachments?.Select(
                            attachment => new AttachmentRequest(attachment.ContentUrl, attachment.ContentType)
                    ));

                    ActivityResponse response = await this.ActivityProcessor.ProcessActivityAsync(this.Store, request).ConfigureAwait(false);

                    // Reply (on a new network connection) back.
                    Microsoft.Bot.Connector.Activity reply = activity.CreateReply();
                    reply.Text = response.Text;
                    foreach (AttachmentResponse attachment in response.Attachments)
                    {
                        reply.Attachments.Add(new Attachment(attachment.ContentType, attachment.ContentUrl, null, attachment.Name));
                    }

                    // Send it either as a group message or individually, depending on how we received the message,
                    using (ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl)))
                    {
                        if (message.Conversation.IsGroup.HasValue && message.Conversation.IsGroup.Value)
                        {
                            await connector.Conversations.SendToConversationAsync(reply);
                        }
                        else
                        {
                            await connector.Conversations.ReplyToActivityAsync(reply);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl)))
                {
                    Microsoft.Bot.Connector.Activity reply = activity.CreateReply();
                    reply.Text = ex.Message + " " + ex.StackTrace;
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }

            // We always accept the message and send the response using the bot framework on another channel.
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
