using Milochau.Emails.Sdk.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Emails.Sdk.DataAccess
{
    /// <summary>Emails client</summary>
    public interface IEmailsClient
    {
        /// <summary>Send an email</summary>
        /// <param name="email">Email content and metadata</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task SendEmailAsync(Email email, CancellationToken cancellationToken);
    }
}
