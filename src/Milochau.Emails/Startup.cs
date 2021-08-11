using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Milochau.Core.Functions;
using Milochau.Emails.Sdk.Helpers;
using Milochau.Emails;
using Milochau.Emails.DataAccess;
using Milochau.Emails.Models.Options;
using Milochau.Emails.Services;
using Milochau.Emails.Services.EmailTemplates;
using SendGrid;
using System.Text.Encodings.Web;
using Milochau.Core.Abstractions;
using Microsoft.Extensions.Logging;
using Milochau.Core.Infrastructure.Hosting;
using Azure.Identity;
using Azure.Storage.Blobs;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Milochau.Emails
{
    public class Startup : CoreFunctionsStartup
    {
        protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var hostOptions = CoreOptionsFactory.GetCoreHostOptions(configuration);

            RegisterOptions(services, hostOptions);
            RegisterServices(services);
            RegisterDataAccess(services, hostOptions);
        }

        private void RegisterOptions(IServiceCollection services, CoreHostOptions hostOptions)
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

        private void RegisterDataAccess(IServiceCollection services, CoreHostOptions hostOptions)
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
