namespace BotCommon.Activity
{
    /// <summary>
    /// Defines attachments retrieved as part of an <see cref="ActivityRequest"/>
    /// </summary>
    public class AttachmentRequest
    {
        public AttachmentRequest(string contentUrl, string contentType)
        {
            this.ContentUrl = contentUrl;
            this.ContentType = contentType;
        }

        public string ContentUrl { get; }
        public string ContentType { get; }
    }
}
