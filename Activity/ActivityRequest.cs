using System.Collections.Generic;
using System.Linq;

namespace BotCommon.Activity
{
    /// <summary>
    /// Defines a request received from a user that the bot shoud process.
    /// </summary>
    public class ActivityRequest
    {
        public ActivityRequest(
            string recipient,
            string text,
            string from,
            string fromId,
            string channelId,
            string conversationId,
            bool? isGroup,
            IEnumerable<AttachmentRequest> attachments)
        {
            this.Recipient = recipient;
            this.Text = text;
            this.From = from;
            this.FromId = fromId;
            this.ChannelId = channelId;
            this.ConversationId = conversationId;
            this.IsGroup = isGroup;
            this.Attachments = attachments?.ToList() ?? new List<AttachmentRequest>();
        }

        public string Recipient { get; }

        public string Text { get; }

        public string From { get; }

        public string FromId { get; }

        public string ChannelId { get; }

        public string ConversationId { get; }

        public bool? IsGroup { get; }

        public IReadOnlyList<AttachmentRequest> Attachments { get; }

        /// <summary>
        /// Defines a field that will uniquely identify a user
        /// </summary>
        public string UserId => $"{ChannelId}-{From}";

        /// <summary>
        /// Returns the input text without any mention of the recipient, newlines, and leading / trailing whitespace.
        /// </summary>
        /// <example>
        /// Input: &lt;at&gt;gustave&lt;/at$gt; how are you today?
        /// Output: how are you today?</example>
        public string SanitizedText => this.Text.Replace($"<at>{this.Recipient}</at>", string.Empty).Replace('\r', ' ').Replace('\n', ' ').Trim();
    }
}
