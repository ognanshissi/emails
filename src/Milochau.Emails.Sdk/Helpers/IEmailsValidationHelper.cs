using Milochau.Emails.Sdk.Models;
using System.Collections.Generic;

namespace Milochau.Emails.Sdk.Helpers
{
    /// <summary>Validation helper for emails</summary>
    public interface IEmailsValidationHelper
    {
        /// <summary>Validate model before sending email</summary>
        IEnumerable<string> ValidateEmail(Email email);
    }
}