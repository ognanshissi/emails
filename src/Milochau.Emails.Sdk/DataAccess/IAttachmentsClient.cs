using Milochau.Emails.Sdk.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Emails.Sdk.DataAccess
{
    /// <summary>Client to store documents used by the emails application</summary>
    public interface IAttachmentsClient
    {
        /// <summary>Write an attachment into a stream</summary>
        /// <param name="attachment">Attachment content</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Document storage information</returns>
        Task<EmailAttachmentContentResult> WriteFromStreamAsync(EmailAttachmentContent attachment, CancellationToken cancellationToken);
    }
}
