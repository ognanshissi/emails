using Microsoft.Extensions.Logging;
using Milochau.Emails.Sdk.Models;
using Polly;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Emails.DataAccess
{
    internal class EmailsSendGridClient : IEmailsDataAccess
    {
        private readonly ISendGridClient sendGridClient;
        private readonly IStorageDataAccess storageDataAccess;
        private readonly ILogger<EmailsSendGridClient> logger;

        public EmailsSendGridClient(ISendGridClient sendGridClient,
            IStorageDataAccess storageDataAccess,
            ILogger<EmailsSendGridClient> logger)
        {
            this.sendGridClient = sendGridClient;
            this.storageDataAccess = storageDataAccess;
            this.logger = logger;
        }

        public async Task SendEmailAsync(Email email, CancellationToken cancellationToken)
        {
            var sendGridMessage = await CreateSendGridMessageAsync(email, cancellationToken);

            var policy = Policy
                .Handle<Exception>()
                .RetryAsync((exception, count) =>
                {
                    logger.LogWarning(exception, $"Error with attempt #{count} to send email with SendGrid");
                });

            await policy.ExecuteAsync((ctx) => SendEmailAsync(sendGridMessage, ctx), cancellationToken);
        }

        public async Task SendEmailAsync(SendGridMessage sendGridMessage, CancellationToken cancellationToken)
        {
            await sendGridClient.SendEmailAsync(sendGridMessage, cancellationToken);
        }

        public async Task<SendGridMessage> CreateSendGridMessageAsync(Email email, CancellationToken cancellationToken)
        {
            var sendGridMessage = new SendGridMessage
            {
                From = new SendGrid.Helpers.Mail.EmailAddress(email.From.Email, email.From.Name),
                HtmlContent = email.Body,
                Subject = email.Subject
            };

            email.Tos.ForEach(x => sendGridMessage.AddTo(x.Email, x.Name));

            if (email.Ccs != null)
                email.Ccs.ForEach(x => sendGridMessage.AddCc(x.Email, x.Name));

            if (email.Bccs != null)
                email.Bccs.ForEach(x => sendGridMessage.AddBcc(x.Email, x.Name));

            if (email.ReplyTo != null)
                sendGridMessage.ReplyTo = new SendGrid.Helpers.Mail.EmailAddress(email.ReplyTo.Email, email.ReplyTo.Name);

            if (email.Attachments != null)
            {
                foreach (var attachment in email.Attachments)
                {
                    var fileStream = await storageDataAccess.ReadToStreamAsync(attachment, cancellationToken);
                    await sendGridMessage.AddAttachmentAsync(attachment.PublicFileName ?? attachment.FileName, fileStream, null, null, null, cancellationToken);
                }
            }

            SetImportance(sendGridMessage, email);
            SetPriority(sendGridMessage, email);

            return sendGridMessage;
        }

        public void SetImportance(SendGridMessage sendGridMessage, Email email)
        {
            switch (email.Importance)
            {
                case ImportanceType.Low:
                    sendGridMessage.AddHeader("Importance", "low");
                    break;
                case ImportanceType.High:
                    sendGridMessage.AddHeader("Importance", "high");
                    break;
                case ImportanceType.Normal:
                default:
                    break;
            }
        }

        public void SetPriority(SendGridMessage sendGridMessage, Email email)
        {
            switch (email.Priority)
            {
                case PriorityType.NonUrgent:
                    sendGridMessage.AddHeader("Priority", "non-urgent");
                    break;
                case PriorityType.Urgent:
                    sendGridMessage.AddHeader("Priority", "urgent");
                    break;
                case PriorityType.Normal:
                default:
                    break;
            }
        }
    }
}
