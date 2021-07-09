using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Emails.Sdk.Helpers;
using Milochau.Emails.Sdk.Models;
using System.Collections.Generic;
using System.Linq;

namespace Milochau.Emails.Sdk.UnitTests.Helpers
{
    [TestClass]
    public class EmailsValidationHelperTests
    {
        private EmailsValidationHelper emailsValidationHelper;

        [TestInitialize]
        public void Initialize()
        {
            emailsValidationHelper = new EmailsValidationHelper();
        }

        [TestMethod]
        public void ValidateEmail_Should_ReturnErrorsList_When_ModelIsNull()
        {
            // Arrange
            Email email = null;

            // Act
            var errors = emailsValidationHelper.ValidateEmail(email);

            // Assert
            Assert.IsTrue(errors.Any());
        }

        [TestMethod]
        public void ValidateEmail_Should_ReturnErrorsList_When_ModelHasNoTos()
        {
            // Arrange
            var email = new Email
            {
                Subject = "subject",
                Body = "body"
            };

            // Act
            var errors = emailsValidationHelper.ValidateEmail(email);

            // Assert
            Assert.IsTrue(errors.Any());
        }

        [TestMethod]
        public void ValidateEmail_Should_ReturnErrorsList_When_ModelHasNoSubject()
        {
            // Arrange
            var email = new Email
            {
                Tos = new List<EmailAddress> { new EmailAddress { Email = "bill.gates@microsoft.com" } },
                Body = "body"
            };

            // Act
            var errors = emailsValidationHelper.ValidateEmail(email);

            // Assert
            Assert.IsTrue(errors.Any());
        }

        [TestMethod]
        public void ValidateEmail_Should_ReturnErrorsList_When_ModelHasNoBody()
        {
            // Arrange
            var email = new Email
            {
                Tos = new List<EmailAddress> { new EmailAddress { Email = "bill.gates@microsoft.com" } },
                Subject = "subject"
            };

            // Act
            var errors = emailsValidationHelper.ValidateEmail(email);

            // Assert
            Assert.IsTrue(errors.Any());
        }

    }
}
