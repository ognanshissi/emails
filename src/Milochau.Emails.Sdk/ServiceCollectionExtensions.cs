using Milochau.Emails.Sdk.DataAccess;
using Milochau.Emails.Sdk.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using System;
using Milochau.Emails.Sdk.Helpers;

namespace Milochau.Emails.Sdk
{
    /// <summary>Extensions for <see cref="IServiceCollection"/></summary>
    public static class ServiceCollectionExtensions
    {
        private const string serviceBusEndpointName = "Emails microservice (Service Bus)";

        /// <summary>Register emails clients, to be accessed from dependency injection</summary>
        /// <param name="services">Service collection</param>
        /// <param name="settings">Settings</param>
        public static IServiceCollection AddEmailsClients(this IServiceCollection services, Action<EmailsServiceSettings> settings)
        {
            var settingsValue = new EmailsServiceSettings();
            settings.Invoke(settingsValue);

            // Add helpers
            services.AddScoped<IEmailsValidationHelper, EmailsValidationHelper>();

            // Add services for ServiceBus communication
            if (!string.IsNullOrEmpty(settingsValue.ServiceBusConnectionString) && !string.IsNullOrEmpty(settingsValue.ServiceBusQueueName))
            {
                services.AddScoped<IEmailsClient, EmailsServiceBusClient>();
                services.AddScoped<IQueueClient>(serviceProvider => new QueueClient(settingsValue.ServiceBusConnectionString, settingsValue.ServiceBusQueueName));
            }

            // Add health checks
            services.AddHealthChecks().AddServiceHealthChecks(settingsValue);

            return services;
        }

        /// <summary>Add emails microservice to the health checks collection</summary>
        internal static IHealthChecksBuilder AddServiceHealthChecks(this IHealthChecksBuilder builder, EmailsServiceSettings settingsValue)
        {
            if (!string.IsNullOrEmpty(settingsValue.ServiceBusConnectionString) && !string.IsNullOrEmpty(settingsValue.ServiceBusQueueName))
            {
                builder.AddAzureServiceBusQueue(settingsValue.ServiceBusConnectionString, settingsValue.ServiceBusQueueName, serviceBusEndpointName);
            }
            return builder;
        }
    }
}
