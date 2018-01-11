namespace BotCommon.Activity
{
    /// <summary>
    /// Defines attachments returned as part of an <see cref="ActivityResponse"/>
    /// </summary>
    public class AttachmentResponse
    {
        public AttachmentResponse(string contentUrl, string contentType, string name)
        {
            this.ContentUrl = contentUrl;
            this.ContentType = contentType;
            this.Name = name;
        }

        public string ContentUrl { get; }
        public string ContentType { get; }
        public string Name { get; }
    }
}
