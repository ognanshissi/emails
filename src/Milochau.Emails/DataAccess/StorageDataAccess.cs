using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using Milochau.Emails.Sdk.Models;
using Milochau.Emails.Models.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Emails.DataAccess
{
    public class StorageDataAccess : IStorageDataAccess
    {
        private readonly StorageOptions options;

        public StorageDataAccess(IOptions<StorageOptions> options)
        {
            this.options = options.Value;
        }

        public async Task<Stream> ReadToStreamAsync(StorageAttachment attachment, CancellationToken cancellationToken)
        {
            var connectionString = GetConnectionString(attachment.StorageAccount);
            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(attachment.ContainerName);
            var blobClient = blobContainerClient.GetBlobClient(attachment.FileName);
            
            return await blobClient.OpenReadAsync(new BlobOpenReadOptions(allowModifications: false), cancellationToken);
        }

        private string GetConnectionString(StorageAccount account)
        {
            return account switch
            {
                StorageAccount.Default => options.DefaultConnectionString,
                _ => throw new ArgumentOutOfRangeException(nameof(account), account, $"Unknown storage account {account}")
            };
        }
    }
}
