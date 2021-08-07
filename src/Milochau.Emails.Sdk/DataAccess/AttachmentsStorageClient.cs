using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Milochau.Emails.Sdk.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Emails.Sdk.DataAccess
{
    internal class AttachmentsStorageClient : IAttachmentsClient
    {
        private readonly BlobContainerClient blobContainerClient;
        private readonly ILogger<AttachmentsStorageClient> logger;

        public AttachmentsStorageClient(BlobContainerClient blobContainerClient,
            ILogger<AttachmentsStorageClient> logger)
        {
            this.blobContainerClient = blobContainerClient;
            this.logger = logger;
        }

        /// <summary>Write an attachment into a stream</summary>
        /// <param name="attachment">Attachment content</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The document URI</returns>
        public async Task<Uri> WriteFromStreamAsync(EmailAttachmentContent attachment, CancellationToken cancellationToken)
        {
            var fileName = Guid.NewGuid().ToString();
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(attachment.Content, overwrite: false, cancellationToken).ConfigureAwait(false);
            logger.LogDebug("Uploaded an attachment into the Emails Storage Account");

            return blobClient.Uri;
        }
    }
}
