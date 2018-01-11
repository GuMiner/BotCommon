using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace BotCommon.Storage
{
    /// <summary>
    /// Simplifies Azure Blob storage read / write operations for persistent bot storage.
    /// </summary>
    public class AzureBlobStore : IStore
    {
        private readonly CloudBlobClient client;

        /// <summary>
        /// Creates a new <see cref="AzureBlobStore"/> that will store everything in the specified <paramref name="botContainer"/> for this bot.
        /// </summary>
        public AzureBlobStore(string connectionString, string botContainer)
        {
            this.client = CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();
            this.BotContainer = botContainer;
        }

        /// <inheritdoc />
        public string BotContainer { get; }
        
        /// <inheritdoc />
        public async Task<bool> ExistsAsync(string subContainer, string objectId)
        {
            CloudBlob blob = await this.GetCloudBlobAsync(subContainer, objectId).ConfigureAwait(false);
            return await blob.ExistsAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<T> GetAsync<T>(string subContainer, string objectId)
        {
            CloudBlockBlob blob = await this.GetCloudBlobAsync(subContainer, objectId).ConfigureAwait(false);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(memoryStream).ConfigureAwait(false);
                memoryStream.Position = 0;

                using (StreamReader sr = new StreamReader(memoryStream))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    return Serializer.JsonSerializer.Deserialize<T>(reader);
                }
            }
        }

        /// <inheritdoc />
        public async Task CreateOrUpdateAsync<T>(string subContainer, string objectId, T @object)
        {
            CloudBlockBlob blob = await this.GetCloudBlobAsync(subContainer, objectId).ConfigureAwait(false);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(memoryStream))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    string result = JsonConvert.SerializeObject(@object);
                    Serializer.JsonSerializer.Serialize(writer, @object);
                    await sw.FlushAsync().ConfigureAwait(false);
                    memoryStream.Position = 0;

                    await blob.UploadFromStreamAsync(memoryStream).ConfigureAwait(false);
                }
            }
        }

        /// <inheritdoc />
        public async Task CreateOrUpdateAsync(string subContainer, string objectId, Func<Stream, Task> saveAction)
        {
            CloudBlockBlob blob = await this.GetCloudBlobAsync(subContainer, objectId).ConfigureAwait(false);
            
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await saveAction(memoryStream).ConfigureAwait(false);

                memoryStream.Position = 0;
                await blob.UploadFromStreamAsync(memoryStream).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task DeleteIfExistsAsync(string subContainer, string objectId)
        {
            CloudBlob blob = await this.GetCloudBlobAsync(subContainer, objectId).ConfigureAwait(false);
            await blob.DeleteIfExistsAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<string> GetBlobSasUriAsync(string subContainer, string objectId, TimeSpan lifetime)
        {
            // Pulled from https://docs.microsoft.com/en-us/azure/storage/common/storage-dotnet-shared-access-signature-part-1?toc=%2fazure%2fstorage%2fblobs%2ftoc.json
            string sasBlobToken;

            CloudBlob blob = await this.GetCloudBlobAsync(subContainer, objectId).ConfigureAwait(false);
            SharedAccessBlobPolicy blobSas = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.Add(lifetime),
                Permissions = SharedAccessBlobPermissions.Read,
            };

            sasBlobToken = blob.GetSharedAccessSignature(blobSas);

            return $"{blob.Uri}{sasBlobToken}";
        }
        
        private async Task<CloudBlockBlob> GetCloudBlobAsync(string subContainer, string objectId)
        {
            CloudBlobContainer container = this.client.GetContainerReference($"{this.BotContainer}-{subContainer}");
            await container.CreateIfNotExistsAsync().ConfigureAwait(false);

            return container.GetBlockBlobReference(HttpUtility.UrlEncode(objectId));
        }
    }
}
