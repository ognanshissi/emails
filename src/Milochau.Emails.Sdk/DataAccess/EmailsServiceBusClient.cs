using Milochau.Emails.Sdk.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Milochau.Emails.Sdk.Helpers;
using System.Linq;
using Azure.Messaging.ServiceBus;

namespace Milochau.Emails.Sdk.DataAccess
{
    /// <summary>Emails client, via Service Bus</summary>
    public class EmailsServiceBusClient : IEmailsClient
    {
        private readonly ServiceBusSender serviceBusSender;
        private readonly IEmailsValidationHelper emailsValidationHelper;
        private readonly ILogger<EmailsServiceBusClient> logger;

        /// <summary>Constructor</summary>
        public EmailsServiceBusClient(ServiceBusSender serviceBusSender,
            IEmailsValidationHelper emailsValidationHelper,
            ILogger<EmailsServiceBusClient> logger)
        {
            this.serviceBusSender = serviceBusSender;
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
                throw new ArgumentException(aggregatedErrors, nameof(email));
            }

            var message = new ServiceBusMessage(JsonSerializer.Serialize(email));

            await serviceBusSender.SendMessageAsync(message).ConfigureAwait(false);
        }
    }
}
