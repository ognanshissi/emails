using Microsoft.Extensions.Options;
using Milochau.Emails.Sdk.Models;
using Milochau.Emails.DataAccess;
using Milochau.Emails.Models.Options;
using Milochau.Emails.Services.EmailTemplates;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Emails.Services
{
    public class EmailsService : IEmailsService
    {
        private readonly IEmailsDataAccess emailsDataAccess;
        private readonly IEmailTemplateFactory emailTemplateFactory;
        private readonly EmailsOptions options;

        public EmailsService(IEmailsDataAccess emailsDataAccess,
            IEmailTemplateFactory emailTemplateFactory,
            IOptionsSnapshot<EmailsOptions> options)
        {
            this.emailsDataAccess = emailsDataAccess;
            this.emailTemplateFactory = emailTemplateFactory;
            this.options = options.Value;
        }

        public async Task SendEmailAsync(Email email, CancellationToken cancellationToken)
        {
            // Format email
            FormatEmail(email);

            // Replace body thanks to email template
            var emailTemplate = emailTemplateFactory.Create(email.TemplateId);
            email.Body = emailTemplate.GetAsString(email);

            // Send email
            await emailsDataAccess.SendEmailAsync(email, cancellationToken);
        }

        internal void FormatEmail(Email email)
        {
            FormatRecipients(email);
            FormatReplyTo(email);
        }

        internal void FormatRecipients(Email email)
        {
            if (options.AuthorizedRecipientHosts != null && options.AuthorizedRecipientHosts.Any())
            {
                ReplaceUnauthorizedRecipientHosts(email.Tos);
                ReplaceUnauthorizedRecipientHosts(email.Ccs);
                ReplaceUnauthorizedRecipientHosts(email.Bccs);
            }
        }

        internal void ReplaceUnauthorizedRecipientHosts(List<EmailAddress> addresses)
        {
            addresses.RemoveAll(x => !options.AuthorizedRecipientHosts.Contains(new MailAddress(x.Email).Host));
        }

        internal void FormatReplyTo(Email email)
        {
            if (email.ReplyTo == null)
                email.ReplyTo = new EmailAddress();
            if (string.IsNullOrEmpty(email.ReplyTo.Email))
                email.ReplyTo.Email = email.From.Email;
        }
    }
}
