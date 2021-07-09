using Milochau.Emails.Sdk.Models;

namespace Milochau.Emails.Services.EmailTemplates
{
    public interface IEmailTemplate
    {
        string GetAsString(Email email);
    }
}
