using System.Collections.Generic;
using System.Linq;

namespace BotCommon.Activity
{
    /// <summary>
    /// Defines a response to an <see cref="ActivityRequest"/>
    /// </summary>
    public class ActivityResponse
    {
        public ActivityResponse(string text, IEnumerable<AttachmentResponse> attachments = null)
        {
            this.Text = text;
            this.Attachments = attachments?.ToList() ?? new List<AttachmentResponse>();
        }

        public string Text { get; }

        public IReadOnlyList<AttachmentResponse> Attachments { get; }
    }
}
