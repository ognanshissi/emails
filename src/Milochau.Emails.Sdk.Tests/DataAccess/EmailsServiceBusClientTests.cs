using Milochau.Emails.Sdk.DataAccess;
using Milochau.Emails.Sdk.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Milochau.Emails.Sdk.Helpers;
using Microsoft.Extensions.Logging;

namespace Milochau.Emails.Sdk.UnitTests.DataAccess
{
    [TestClass]
    public class EmailsServiceBusClientTests
    {
        private Mock<IQueueClient> queueClient;
        private Mock<IEmailsValidationHelper> emailsValidationHelper;
        private Mock<ILogger<EmailsServiceBusClient>> logger;

        private EmailsServiceBusClient emailsServiceBusClient;

        [TestInitialize]
        public void Initialize()
        {
            queueClient = new Mock<IQueueClient>();
            emailsValidationHelper = new Mock<IEmailsValidationHelper>();
            logger = new Mock<ILogger<EmailsServiceBusClient>>();

            emailsServiceBusClient = new EmailsServiceBusClient(queueClient.Object, emailsValidationHelper.Object, logger.Object);
        }

        [TestMethod]
        public async Task SendEmailAsync_Should_CallQueueClient_When_Called()
        {
            // Arrange
            var email = new Email();

            // Act
            await emailsServiceBusClient.SendEmailAsync(email, CancellationToken.None);

            // Assert
            queueClient.Verify(x => x.SendAsync(It.IsAny<Message>()));
        }
    }
}
