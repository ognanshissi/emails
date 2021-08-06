using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Milochau.Core.Abstractions;
using Milochau.Emails.Models.Options;
using Milochau.Emails.Sdk.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Emails.DataAccess
{
    internal class StorageDataAccess : IStorageDataAccess
    {
        private readonly CoreHostOptions hostOptions;
        private readonly ILogger<StorageDataAccess> logger;
        private readonly string storageAccountUri;

        private const string defaultContainerName = "default";

        public StorageDataAccess(IOptions<CoreHostOptions> hostOptions,
            EmailsOptions options,
            ILogger<StorageDataAccess> logger)
        {
            this.hostOptions = hostOptions.Value;
            this.logger = logger;

            storageAccountUri = options.StorageAccountUri ?? $"{this.hostOptions.Application.OrganizationName}stg{this.hostOptions.Application.ApplicationName}1{this.hostOptions.Application.HostName}";
        }


        public async Task<Stream> ReadToStreamAsync(EmailAttachment attachment, CancellationToken cancellationToken)
        {
            var credential = new DefaultAzureCredential(hostOptions.Credential);
            var blobServiceClient = new BlobServiceClient(new Uri(storageAccountUri), credential);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(defaultContainerName);
            var blobClient = blobContainerClient.GetBlobClient(attachment.FileName);

            logger.LogDebug("Opening a blob from Storage Account...");
            return await blobClient.OpenReadAsync(new BlobOpenReadOptions(allowModifications: false), cancellationToken);
        }
    }
}
