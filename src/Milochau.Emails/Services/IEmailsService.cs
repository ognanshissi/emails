using Milochau.Emails.Sdk.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Emails.Services
{
    public interface IEmailsService
    {
        Task SendEmailAsync(Email email, CancellationToken cancellationToken);
    }
}
