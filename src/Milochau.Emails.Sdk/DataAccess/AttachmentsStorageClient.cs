using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Milochau.Core.Abstractions;
using Milochau.Emails.Sdk.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Emails.Sdk.DataAccess
{
    internal class AttachmentsStorageClient : IAttachmentsClient
    {
        private readonly IOptions<CoreHostOptions> hostOptions;
        private readonly EmailsServiceSettings options;
        private readonly ILogger<AttachmentsStorageClient> logger;

        private const string defaultContainerName = "default";

        public AttachmentsStorageClient(IOptions<CoreHostOptions> hostOptions,
            EmailsServiceSettings options,
            ILogger<AttachmentsStorageClient> logger)
        {
            this.hostOptions = hostOptions;
            this.options = options;
            this.logger = logger;
        }

        /// <summary>Write an attachment into a stream</summary>
        /// <param name="attachment">Attachment content</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The document URI</returns>
        public async Task<Uri> WriteFromStreamAsync(EmailAttachmentContent attachment, CancellationToken cancellationToken)
        {
            var credential = new DefaultAzureCredential(hostOptions?.Value.Credential);
            var blobServiceClient = new BlobServiceClient(options.StorageAccountUri, credential);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(defaultContainerName);
            var fileName = Guid.NewGuid().ToString();
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(attachment.Content, overwrite: false, cancellationToken).ConfigureAwait(false);
            logger.LogDebug("Uploaded an attachment into the Emails Storage Account");

            return blobClient.Uri;
        }
    }
}
