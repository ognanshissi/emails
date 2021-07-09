using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using Milochau.Emails.Sdk.DataAccess;
using Milochau.Emails.Sdk.Helpers;

namespace Milochau.Emails.Sdk.UnitTests
{
    [TestClass]
    public class ServiceCollectionExtensionsTests
    {
        private ServiceCollection serviceCollection;
        private HealthChecksBuilder healthChecksBuilder;

        [TestInitialize]
        public void Initialize()
        {
            serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            healthChecksBuilder = new HealthChecksBuilder(serviceCollection);
        }

        [TestMethod]
        public void AddEmailsClients_Should_NotRegisterEmailsClient_When_NoSettingsIsConfigured()
        {
            // Arrange

            // Act
            serviceCollection.AddEmailsClients(settings =>
            {
            });

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Assert.IsNotNull(serviceProvider.GetService<IEmailsValidationHelper>());
            Assert.IsNull(serviceProvider.GetService<IEmailsClient>());
        }

        [TestMethod]
        public void AddEmailsClients_Should_RegisterEmailsClient_When_SettingsAreConfigured()
        {
            // Arrange

            // Act
            serviceCollection.AddEmailsClients(settings =>
            {
                settings.ServiceBusConnectionString = "Endpoint=sb://xxx.servicebus.windows.net/;SharedAccessKeyName=development;SharedAccessKey=xxx";
                settings.ServiceBusQueueName = "ServiceBusQueueName";
            });

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Assert.IsNotNull(serviceProvider.GetService<IEmailsValidationHelper>());
            Assert.IsNotNull(serviceProvider.GetService<IEmailsClient>());
        }
        [TestMethod]
        public void ConfigureServices_Should_AddNoHealthCheck_When_NoSettingsIsConfigured()
        {
            // Arrange
            var settings = new Models.EmailsServiceSettings();

            // Act
            healthChecksBuilder.AddServiceHealthChecks(settings);

            // Assert
            Assert.AreEqual(0, healthChecksBuilder.Registrations.Count);
        }

        [TestMethod]
        public void ConfigureServices_Should_AddHealthCheck_When_SettingsAreConfigured()
        {
            // Arrange
            var settings = new Models.EmailsServiceSettings
            {
                ServiceBusConnectionString = "ServiceBusConnectionString",
                ServiceBusQueueName = "ServiceBusQueueName"
            };

            // Act
            healthChecksBuilder.AddServiceHealthChecks(settings);

            // Assert
            Assert.AreEqual(1, healthChecksBuilder.Registrations.Count);
        }

        private class HealthChecksBuilder : IHealthChecksBuilder
        {
            public HealthChecksBuilder(IServiceCollection services)
            {
                Services = services;
            }

            public IServiceCollection Services { get; }

            public ICollection<HealthCheckRegistration> Registrations { get; } = new List<HealthCheckRegistration>();

            public IHealthChecksBuilder Add(HealthCheckRegistration registration)
            {
                Registrations.Add(registration);

                return this;
            }
        }
    }
}
