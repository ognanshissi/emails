using Milochau.Emails.Sdk.Models;
using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Milochau.Emails.Sdk.Helpers;
using System.Linq;

namespace Milochau.Emails.Sdk.DataAccess
{
    /// <summary>Emails client, via Service Bus</summary>
    public class EmailsServiceBusClient : IEmailsClient
    {
        private readonly IQueueClient queueClient;
        private readonly IEmailsValidationHelper emailsValidationHelper;
        private readonly ILogger<EmailsServiceBusClient> logger;

        /// <summary>Constructor</summary>
        public EmailsServiceBusClient(IQueueClient queueClient,
            IEmailsValidationHelper emailsValidationHelper,
            ILogger<EmailsServiceBusClient> logger)
        {
            this.queueClient = queueClient;
            this.emailsValidationHelper = emailsValidationHelper;
            this.logger = logger;
        }

        /// <summary>Send an email</summary>
        /// <param name="email">Email content and metadata</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task SendEmailAsync(Email email, CancellationToken cancellationToken)
        {
            var errors = emailsValidationHelper.ValidateEmail(email);
            if (errors != null && errors.Any())
            {
                var aggregatedErrors = errors.Aggregate((a, b) => a + Environment.NewLine + b);
                logger.LogWarning("Email has not been sent, due do validation problems." + Environment.NewLine + aggregatedErrors);
                throw new ArgumentException(nameof(email), aggregatedErrors);
            }

            var message = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(email))
            };

            await queueClient.SendAsync(message).ConfigureAwait(false);
        }
    }
}
