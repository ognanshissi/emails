using Milochau.Emails.Sdk.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Emails.DataAccess
{
    public interface IEmailsDataAccess
    {
        Task SendEmailAsync(Email email, CancellationToken cancellationToken);
    }
}
