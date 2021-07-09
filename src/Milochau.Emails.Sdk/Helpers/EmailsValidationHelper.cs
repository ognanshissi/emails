using Milochau.Emails.Sdk.Models;
using System.Collections.Generic;
using System.Linq;

namespace Milochau.Emails.Sdk.Helpers
{
    /// <summary>Validation helper for emails</summary>
    public class EmailsValidationHelper : IEmailsValidationHelper
    {
        /// <summary>Validate model before sending email</summary>
        public IEnumerable<string> ValidateEmail(Email email)
        {
            if (email == null)
                return new[] { "An email must be defined." };

            return ValidateBasics(email);
        }

        private static IEnumerable<string> ValidateBasics(Email email)
        {
            if (email.Tos == null || !email.Tos.Any())
                yield return "A recipient must be included.";
            if (string.IsNullOrEmpty(email.Subject))
                yield return "A subject must be included.";
            if (string.IsNullOrEmpty(email.Body))
                yield return "A body must be included.";
        }
    }
}
