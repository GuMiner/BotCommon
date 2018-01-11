using System;
using System.IO;
using System.Threading.Tasks;

namespace BotCommon.Storage
{
    /// <summary>
    /// Defines a persistent unstructured object storage interface for bots/
    /// </summary>
    public interface IStore
    {
        /// <summary>
        /// The container that this bot uses for storage.
        /// </summary>
        string BotContainer { get; }

        /// <summary>
        /// Returns true if the blob with the specified ID exists.
        /// </summary>
        Task<bool> ExistsAsync(string subContainer, string objectId);

        /// <summary>
        /// Deserializes the specified object to the specified type.
        /// </summary>
        /// <typeparam name="T">The object type to deserialize to.</typeparam>
        Task<T> GetAsync<T>(string subContainer, string objectId);

        /// <summary>
        /// Creates or overwrites the specified object with the new <paramref name="@object"/>.
        /// </summary>
        /// <typeparam name="T">The object type being serialized.</typeparam>
        Task CreateOrUpdateAsync<T>(string subContainer, string objectId, T @object);

        /// <summary>
        /// Creates or overwrites the specified object with whatever is written to the stream in <paramref name="saveAction"/>
        /// </summary>
        Task CreateOrUpdateAsync(string subContainer, string objectId, Func<Stream, Task> saveAction);

        /// <summary>
        /// Deletes the specified object if it exists.
        /// </summary>
        Task DeleteIfExistsAsync(string subContainer, string objectId);

        /// <summary>
        /// Gets a SAS URI for the specified blob that will be valid for <paramref name="lifetime"/> after <see cref="DateTime.UtcNow"/>
        /// </summary>
        Task<string> GetBlobSasUriAsync(string subContainer, string objectId, TimeSpan lifetime);
    }
}