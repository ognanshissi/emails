using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using Milochau.Emails.Sdk.Models;
using Milochau.Emails.Models.Options;
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
            var blobServiceClient = new BlobServiceClient(options.DefaultConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(attachment.ContainerName);
            var blobClient = blobContainerClient.GetBlobClient(attachment.FileName);
            
            return await blobClient.OpenReadAsync(new BlobOpenReadOptions(allowModifications: false), cancellationToken);
        }
    }
}
