using Milochau.Emails.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Milochau.Emails.Sdk.Models;
using System.Linq;
using Milochau.Emails.Sdk.Helpers;
using Microsoft.Azure.Functions.Worker;
using System.Threading;

namespace Milochau.Emails.Functions
{
    public class EmailsFunctions
    {
        private readonly IEmailsService emailsService;
        private readonly IEmailsValidationHelper emailsValidationHelper;

        public EmailsFunctions(IEmailsService emailsService,
            IEmailsValidationHelper emailsValidationHelper)
        {
            this.emailsService = emailsService;
            this.emailsValidationHelper = emailsValidationHelper;
        }

        /// <summary>Send an email from Service Bus endpoint.</summary>
        /// <remarks>
        /// The queue item must be an object of type <see cref="Email"/>
        /// </remarks>
        [Function("SendEmailFromServiceBus")]
        public async Task SendEmailFromServiceBusAsync([ServiceBusTrigger("emails")] Email email, FunctionContext context)
        {
            var logger = context.GetLogger<EmailsFunctions>();
            logger.LogDebug("Start running SendEmailFromServiceBus Function...");

            var errors = emailsValidationHelper.ValidateEmail(email);
            if (errors != null && errors.Any())
            {
                var aggregatedErrors = errors.Aggregate((a, b) => a + Environment.NewLine + b);
                logger.LogWarning("Email has not been sent, due do validation problems." + Environment.NewLine + aggregatedErrors);
                throw new ArgumentException(nameof(Email), aggregatedErrors);
            }

            await emailsService.SendEmailAsync(email, CancellationToken.None);

            logger.LogDebug("Email has been sent.");
        }
    }
}
