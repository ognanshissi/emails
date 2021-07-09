using Milochau.Emails.DataAccess;
using Milochau.Emails.Models.Options;
using Milochau.Emails.Sdk.Models;
using Milochau.Emails.Services;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Milochau.Emails.Services.EmailTemplates;

namespace Milochau.Emails.UnitTests.Services
{
    [TestClass]
    public class EmailsServiceTests
    {
        private Mock<IEmailsDataAccess> emailsDataAccess;
        private Mock<IEmailTemplateFactory> emailTemplateFactory;
        private Mock<IEmailTemplate> emailTemplate;
        private Mock<IOptionsSnapshot<EmailsOptions>> options;
        private EmailsOptions optionsValues;

        private EmailsService emailsService;

        [TestInitialize]
        public void Intialize()
        {
            emailsDataAccess = new Mock<IEmailsDataAccess>();
            emailTemplateFactory = new Mock<IEmailTemplateFactory>();
            emailTemplate = new Mock<IEmailTemplate>();
            emailTemplateFactory.Setup(x => x.Create(It.IsAny<string>())).Returns(emailTemplate.Object);
            options = new Mock<IOptionsSnapshot<EmailsOptions>>();
            optionsValues = new EmailsOptions();
            options.SetupGet(x => x.Value).Returns(optionsValues);

            emailsService = new EmailsService(emailsDataAccess.Object, emailTemplateFactory.Object, options.Object);
        }

        [TestMethod]
        public void FormatRecipients_Should_DoNothing_When_NoHostIsAllowed()
        {
            // Arrange
            var email = new Email();
            email.Tos.Add(new EmailAddress { Email = "bill.gates@microsoft.com" });

            // Act
            emailsService.FormatRecipients(email);

            // Assert
            Assert.AreEqual(1, email.Tos.Count);
            Assert.AreEqual("bill.gates@microsoft.com", email.Tos.First().Email);
        }

        [TestMethod]
        public void FormatRecipients_Should_RemoveEmail_When_HostIsNotAllowed()
        {
            // Arrange
            var email = new Email();
            email.Tos.Add(new EmailAddress { Email = "bill.gates@microsoft.com" });

            optionsValues.AuthorizedRecipientHosts.Add("google.com");

            // Act
            emailsService.FormatRecipients(email);

            // Assert
            Assert.AreEqual(0, email.Tos.Count);
        }

        [TestMethod]
        public void FormatRecipients_Should_RemoveEmail_When_UnauthorizedEmailIsCc()
        {
            // Arrange
            var email = new Email();
            email.Ccs.Add(new EmailAddress { Email = "bill.gates@microsoft.com" });

            optionsValues.AuthorizedRecipientHosts.Add("google.com");

            // Act
            emailsService.FormatRecipients(email);

            // Assert
            Assert.AreEqual(0, email.Ccs.Count);
        }

        [TestMethod]
        public void FormatRecipients_Should_RmoveEmail_When_UnauthorizedEmailIsBcc()
        {
            // Arrange
            var email = new Email();
            email.Bccs.Add(new EmailAddress { Email = "bill.gates@microsoft.com" });

            optionsValues.AuthorizedRecipientHosts.Add("google.com");

            // Act
            emailsService.FormatRecipients(email);

            // Assert
            Assert.AreEqual(0, email.Bccs.Count);
        }

        [TestMethod]
        public void FormatReplyTo_Should_AddReplyTo_When_ReplyToIsNull()
        {
            // Arrange
            var email = new Email
            {
                From = new EmailAddress { Email = "bill.gates@microsoft.com" }
            };

            // Act
            emailsService.FormatReplyTo(email);

            // Assert
            Assert.IsNotNull(email.ReplyTo);
            Assert.AreEqual(email.From.Email, email.ReplyTo.Email);
        }

        [TestMethod]
        public void FormatReplyTo_Should_AddReplyTo_When_ReplyToIsEmpty()
        {
            // Arrange
            var email = new Email
            {
                From = new EmailAddress { Email = "bill.gates@microsoft.com" },
                ReplyTo = new EmailAddress()
            };

            // Act
            emailsService.FormatReplyTo(email);

            // Assert
            Assert.IsNotNull(email.ReplyTo);
            Assert.AreEqual(email.From.Email, email.ReplyTo.Email);
        }

        [TestMethod]
        public async Task SendEmailAsync_Should_SendWithDataAccess_When_Called()
        {
            // Arrange
            var email = new Email();

            // Act
            await emailsService.SendEmailAsync(email, CancellationToken.None);

            // Assert
            emailsDataAccess.Verify(x => x.SendEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
