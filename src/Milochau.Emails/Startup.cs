using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Milochau.Core.Functions;
using Milochau.Emails.Sdk.Helpers;
using Milochau.Emails.DataAccess;
using Milochau.Emails.Models.Options;
using Milochau.Emails.Services;
using Milochau.Emails.Services.EmailTemplates;
using SendGrid;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Azure.Identity;
using Azure.Storage.Blobs;
using System;

namespace Milochau.Emails
{
    public class Startup : CoreFunctionsStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            RegisterOptions(services);
            RegisterServices(services);
            RegisterDataAccess(services);
        }

        private void RegisterOptions(IServiceCollection services)
        {
            services.AddOptions<EmailsOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.Bind("Emails", settings);
                    settings.StorageAccountUri ??= $"https://{hostOptions.Application.OrganizationName}stg{hostOptions.Application.ApplicationName}1{hostOptions.Application.HostName}.blob.core.windows.net/";
                });
            services.AddOptions<SendGridOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.Bind("SendGrid", settings);
                });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IEmailsService, EmailsService>();
            services.AddScoped<ITranslationService, TranslationService>();
            services.AddScoped<IEmailTemplateFactory, EmailTemplateFactory>();

            services.AddScoped<IEmailsValidationHelper, EmailsValidationHelper>();
        }

        private void RegisterDataAccess(IServiceCollection services)
        {
            services.AddSingleton<IEmailsDataAccess, EmailsSendGridClient>();

            services.AddSingleton<IStorageDataAccess>(serviceProvider =>
            {
                var emailsOptions = serviceProvider.GetService<IOptions<EmailsOptions>>().Value;
                var logger = serviceProvider.GetRequiredService<ILogger<StorageDataAccess>>();

                var credential = new DefaultAzureCredential(hostOptions.Credential);
                var blobServiceClient = new BlobServiceClient(new Uri(emailsOptions.StorageAccountUri), credential);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(StorageDataAccess.DefaultContainerName);
                return new StorageDataAccess(blobContainerClient, logger);
            });
            
            services.AddSingleton<ISendGridClient>(serviceProvider =>
            {
                var options = serviceProvider.GetService<IOptions<SendGridOptions>>().Value;
                var sendGridKey = options.Key;
                return new SendGridClient(sendGridKey);
            });

            services.AddSingleton(HtmlEncoder.Default);
        }
    }
}
