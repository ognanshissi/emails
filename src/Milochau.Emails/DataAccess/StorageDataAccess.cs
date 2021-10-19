using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Milochau.Emails.Sdk.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Emails.DataAccess
{
    internal class StorageDataAccess : IStorageDataAccess
    {
        private readonly BlobContainerClient blobContainerClient;
        private readonly ILogger<StorageDataAccess> logger;

        internal const string DefaultContainerName = "attachments";

        public StorageDataAccess(BlobContainerClient blobContainerClient,
            ILogger<StorageDataAccess> logger)
        {
            this.blobContainerClient = blobContainerClient;
            this.logger = logger;
        }


        public async Task<Stream> ReadToStreamAsync(EmailAttachment attachment, CancellationToken cancellationToken)
        {
            var blobClient = blobContainerClient.GetBlobClient(attachment.FileName);

            logger.LogDebug("Opening a blob from Storage Account...");
            return await blobClient.OpenReadAsync(new BlobOpenReadOptions(allowModifications: false), cancellationToken);
        }
    }
}
