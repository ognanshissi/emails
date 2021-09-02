using Milochau.Emails.DataAccess;
using Milochau.Emails.Sdk.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Emails.Tests.DataAccess
{
    [TestClass]
    public class EmailsSendGridClientTests
    {
        private Mock<ISendGridClient> sendGridClient;
        private Mock<IStorageDataAccess> storageDataAccess;
        private Mock<ILogger<EmailsSendGridClient>> logger;

        private EmailsSendGridClient emailsSendGridClient;

        [TestInitialize]
        public void Intiialize()
        {
            sendGridClient = new Mock<ISendGridClient>();
            storageDataAccess = new Mock<IStorageDataAccess>();
            logger = new Mock<ILogger<EmailsSendGridClient>>();

            emailsSendGridClient = new EmailsSendGridClient(sendGridClient.Object, storageDataAccess.Object, logger.Object);
        }

        [TestMethod]
        public async Task SendEmailAsync_Should_CallSendGridClient_When_CalledWithEmptyEmail()
        {
            // Arrange
            var email = new Email();

            // Act
            await emailsSendGridClient.SendEmailAsync(email, CancellationToken.None);

            // Assert
            sendGridClient.Verify(x => x.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task SendEmailAsync_Should_ContainProperRecipient_When_Called()
        {
            // Arrange
            var recipient = "bill.gates@microsoft.com";
            var email = new Email
            {
                Tos = new List<Emails.Sdk.Models.EmailAddress>
                {
                    new Emails.Sdk.Models.EmailAddress { Email = recipient }
                }
            };

            // Act
            await emailsSendGridClient.SendEmailAsync(email, CancellationToken.None);

            // Assert
            sendGridClient.Verify(x => x.SendEmailAsync(It.Is<SendGridMessage>(x => x.Personalizations.Any(p => p.Tos != null && p.Tos.Any(t => t.Email == recipient))), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task SendEmailAsync_Should_ContainProperReplyTo_When_Called()
        {
            // Arrange
            var sender = "bill.gates@microsoft.com";
            var email = new Email
            {
                ReplyTo = new Emails.Sdk.Models.EmailAddress { Email = sender }
            };

            // Act
            await emailsSendGridClient.SendEmailAsync(email, CancellationToken.None);

            // Assert
            sendGridClient.Verify(x => x.SendEmailAsync(It.Is<SendGridMessage>(x => x.ReplyTo.Email == sender), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod("SetImportance - all cases")]
        [DataRow(ImportanceType.High, "high")]
        [DataRow(ImportanceType.Low, "low")]
        public void SetImportance_Should_AddHeaders_When_Called(ImportanceType emailImportance, string sendGridHeaderValue)
        {
            // Arrange
            var sendGridMessage = new SendGridMessage();
            var email = new Email { Importance = emailImportance };

            // Act
            emailsSendGridClient.SetImportance(sendGridMessage, email);

            // Assert
            Assert.AreEqual(sendGridHeaderValue, sendGridMessage.Personalizations[0].Headers["Importance"]);
        }

        [TestMethod("SetPriority - all cases")]
        [DataRow(PriorityType.Urgent, "urgent")]
        [DataRow(PriorityType.NonUrgent, "non-urgent")]
        public void SetPriority_Should_AddHeaders_When_Called(PriorityType emailPriority, string sendGridHeaderValue)
        {
            // Arrange
            var sendGridMessage = new SendGridMessage();
            var email = new Email { Priority = emailPriority };

            // Act
            emailsSendGridClient.SetPriority(sendGridMessage, email);

            // Assert
            Assert.AreEqual(sendGridHeaderValue, sendGridMessage.Personalizations[0].Headers["Priority"]);
        }
    }
}
