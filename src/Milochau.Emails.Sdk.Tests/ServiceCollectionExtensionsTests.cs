using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Emails.Sdk.DataAccess;
using Milochau.Emails.Sdk.Helpers;
using System;

namespace Milochau.Emails.Sdk.UnitTests
{
    [TestClass]
    public class ServiceCollectionExtensionsTests
    {
        private ServiceCollection serviceCollection;

        [TestInitialize]
        public void Initialize()
        {
            serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
        }

        [TestMethod]
        public void AddEmailsClients_Should_ThrowExceptionOnRuntime_When_NoSettingsIsConfigured()
        {
            // Arrange

            // Act
            serviceCollection.AddEmailsClients(settings =>
            {
            });

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Assert.IsNotNull(serviceProvider.GetService<IEmailsValidationHelper>());
            Assert.ThrowsException<ArgumentException>(() => serviceProvider.GetService<IEmailsClient>());
        }

        [TestMethod]
        public void AddEmailsClients_Should_RegisterEmailsClient_When_SettingsAreConfigured()
        {
            // Arrange

            // Act
            serviceCollection.AddEmailsClients(settings =>
            {
                settings.ServiceBusNamespace = "xxx.servicebus.windows.net";
            });

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Assert.IsNotNull(serviceProvider.GetService<IEmailsValidationHelper>());
            Assert.IsNotNull(serviceProvider.GetService<IEmailsClient>());
        }
    }
}
