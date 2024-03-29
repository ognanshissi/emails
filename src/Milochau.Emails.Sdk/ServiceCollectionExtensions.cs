﻿using Milochau.Emails.Sdk.DataAccess;
using Milochau.Emails.Sdk.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using Milochau.Emails.Sdk.Helpers;
using Microsoft.Extensions.Logging;
using Azure.Identity;
using Milochau.Core.Abstractions;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Azure.Storage.Blobs;

namespace Milochau.Emails.Sdk
{
    /// <summary>Extensions for <see cref="IServiceCollection"/></summary>
    public static class ServiceCollectionExtensions
    {
        private const string serviceBusQueueNameEmails = "emails";
        private const string azureStorageContainerName = "attachments";

        /// <summary>Register emails clients, to be accessed from dependency injection</summary>
        /// <param name="services">Service collection</param>
        /// <param name="settings">Settings</param>
        public static IServiceCollection AddEmailsClients(this IServiceCollection services, Action<EmailsServiceSettings> settings)
        {
            var settingsValue = new EmailsServiceSettings();
            settings.Invoke(settingsValue);

            // Add helpers
            services.AddSingleton<IEmailsValidationHelper, EmailsValidationHelper>();

            // Add services for Azure Storage Account
            services.AddSingleton<IAttachmentsClient>(serviceProvider =>
            {
                var hostOptions = serviceProvider.GetService<IOptions<CoreHostOptions>>();
                var logger = serviceProvider.GetRequiredService<ILogger<AttachmentsStorageClient>>();

                var credential = new DefaultAzureCredential(hostOptions.Value.Credential);
                var blobServiceClient = new BlobServiceClient(settingsValue.StorageAccountUri, credential);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(azureStorageContainerName);
                return new AttachmentsStorageClient(blobContainerClient, logger);
            });

            // Add services for Azure Service Bus
            services.AddSingleton<IEmailsClient>(serviceProvider =>
            {
                var hostOptions = serviceProvider.GetService<IOptions<CoreHostOptions>>();
                var emailsValidationHelper = serviceProvider.GetRequiredService<IEmailsValidationHelper>();
                var logger = serviceProvider.GetRequiredService<ILogger<EmailsServiceBusClient>>();

                var credential = new DefaultAzureCredential(hostOptions.Value.Credential);
                var serviceBusClient = new ServiceBusClient(settingsValue.ServiceBusNamespace, credential);
                var serviceBusSender = serviceBusClient.CreateSender(serviceBusQueueNameEmails);

                return new EmailsServiceBusClient(serviceBusSender, emailsValidationHelper, logger);
            });

            return services;
        }
    }
}
