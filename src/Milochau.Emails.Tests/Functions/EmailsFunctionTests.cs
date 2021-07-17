﻿using Milochau.Emails.Functions;
using Milochau.Emails.Sdk.Models;
using Milochau.Emails.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Milochau.Emails.Sdk.Helpers;

namespace Milochau.Emails.UnitTests.Functions
{
    [TestClass]
    public class EmailsFunctionTests
    {
        private Mock<IEmailsService> emailsService;
        private Mock<IEmailsValidationHelper> emailsValidationHelper;
        private Mock<ILogger> logger;

        private EmailsFunctions emailsFunctions;

        [TestInitialize]
        public void Intialize()
        {
            emailsService = new Mock<IEmailsService>();
            emailsValidationHelper = new Mock<IEmailsValidationHelper>();
            logger = new Mock<ILogger>();

            emailsFunctions = new EmailsFunctions(emailsService.Object, emailsValidationHelper.Object);
        }

        [TestMethod]
        public async Task SendEmailFromServiceBusAsync_Should_ReturnOkResult_When_CalledWithProperArguments()
        {
            // Arrange
            var email = new Email();
            emailsValidationHelper.Setup(x => x.ValidateEmail(It.IsAny<Email>())).Returns(Enumerable.Empty<string>());

            // Act
            await emailsFunctions.SendEmailFromServiceBusAsync(email, logger.Object, CancellationToken.None);

            // Assert
            emailsService.Verify(x => x.SendEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task SendEmailFromServiceBusAsync_ShouldThrowException_When_EmailServiceValidationFails()
        {
            // Arrange
            var email = new Email();
            emailsValidationHelper.Setup(x => x.ValidateEmail(It.IsAny<Email>())).Returns(new List<string> { "Error" });

            // Act
            await emailsFunctions.SendEmailFromServiceBusAsync(email, logger.Object, CancellationToken.None);

            // Assert
            emailsService.Verify(x => x.SendEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}