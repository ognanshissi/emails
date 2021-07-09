using Milochau.Emails.Sdk.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Emails.DataAccess
{
    public interface IStorageDataAccess
    {
        Task<Stream> ReadToStreamAsync(StorageAttachment attachment, CancellationToken cancellationToken);
    }
}
